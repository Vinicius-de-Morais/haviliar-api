using System.Security.Claims;
using Haviliar.Domain.Users.Entities;
using Haviliar.Domain.Users.Repositories;
using Haviliar.Domain.Users.Repositories.Filters;
using Haviliar.Domain.Users.Repositories.Projections;
using Haviliar.Infra.Context;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Haviliar.Infra.Users.Repositories;

public class UserRepository : RepositoryBase<User>, IUserRepository
{
    private readonly IHttpContextAccessor _httpContextAccessor;
    public UserRepository(AppDbContext context, IHttpContextAccessor httpContextAccessor) : base(context)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    public async Task<User?> GetByUserNameAsync(string nome, CancellationToken cancellationToken)
    {
        User? usuario = await _dbSet.FirstOrDefaultAsync(u => u.UserName == nome, cancellationToken);

        return usuario;
    }

    public int? GetCurrentUserId()
    {
        if (!IsUserAuthenticated())
        {
            return null;
        }

        string? idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        bool parsed = int.TryParse(idClaim, out int idUsuario);

        return parsed ? idUsuario : null;

    }

    public async Task<IEnumerable<UsersPaginatedProjection>> GetUsersPaginatedAsync(UserFilter userFilter, CancellationToken cancellationToken)
    {
        var collation = "Latin1_General_CI_AI";

        var query = _dbSet
            .AsNoTracking()
            .Where(u => userFilter.Search == null ||
            EF.Functions.Collate(u.UserName, collation).Contains(EF.Functions.Collate(userFilter.Search, collation))||
            u.CreatedAt.ToString().Contains(userFilter.Search));

        userFilter.TotalItems = await query.CountAsync(cancellationToken);

        var paginatedQuery = GetAllPaginated(query, userFilter);


        return await paginatedQuery.Select(u => new UsersPaginatedProjection
        {
            UserId = u.UserId,
            UserName = u.UserName,
            CreatedAt = u.CreatedAt,
            UpdatedAt = u.UpdatedAt,
            UserType = u.UserType
        }).ToListAsync(cancellationToken);
    }

    public bool IsUserAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}
