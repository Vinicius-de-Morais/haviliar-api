using System.Security.Claims;
using Haviliar.Domain.Users.Entities;
using Haviliar.Domain.Users.Repositories;
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

    public int? GetCurrentUserId(CancellationToken cancellationToken)
    {
        if (!IsUserAuthenticated())
        {
            return null;
        }

        string? idClaim = _httpContextAccessor.HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        bool parsed = int.TryParse(idClaim, out int idUsuario);

        return parsed ? idUsuario : null;

    }

    public bool IsUserAuthenticated()
    {
        return _httpContextAccessor.HttpContext?.User.Identity?.IsAuthenticated ?? false;
    }
}
