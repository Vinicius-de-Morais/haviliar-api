using Haviliar.Domain.OperationCenters.Entities;
using Haviliar.Domain.OperationCenters.Repositories.Filters;
using Haviliar.Domain.OperationCenters.Repositories.Projections;
using Haviliar.Domain.Users.Entities;
using Haviliar.Domain.Users.Enums;
using Haviliar.Domain.Users.Repositories;
using Haviliar.Infra.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Haviliar.Infra.Users.Repositories;

public class UserOperationCenterRepository : RepositoryBase<UserOperationCenter>, IUserOperationCenterRepository
{
    public UserOperationCenterRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<UserOperationCenter>> GetAllAsync(
    Expression<Func<UserOperationCenter, bool>> predicate,
    CancellationToken cancellationToken = default)
    {
        return await _dbSet.Where(predicate).ToListAsync(cancellationToken);
    }

    public async Task<OperationCenter?> GetByAuthUser(int operationCenterId, int userId, CancellationToken cancellationToken)
    {
        OperationCenter? operationCenter = await _dbSet.Where(x => x.OperationCenterId == operationCenterId &&
            (x.UserId == userId || x.User.UserType == UserTypeEnum.Admin)).
            Select(x => x.OperationCenter).FirstOrDefaultAsync();

        return operationCenter;
    }

    public async Task<IEnumerable<OperationCenterPaginatedProjection>> GetOperationCentersPaginatedAsync(OperationCenterFilter filter, int userId, CancellationToken cancellationToken)
    {

        var collation = "Latin1_General_CI_AI";
        var query = _dbSet
        .AsNoTracking()
            .Where(u => (u.UserId == userId || u.User.UserType == UserTypeEnum.Admin) && (filter.Search == null ||
            EF.Functions.Collate(u.OperationCenter.Name, collation).Contains(EF.Functions.Collate(filter.Search, collation))));

        filter.TotalItems = await query.CountAsync(cancellationToken);

        var paginatedQuery = GetAllPaginated(query, filter);


        return await paginatedQuery.Select(op => new OperationCenterPaginatedProjection
        {
            OperationCenterId = op.OperationCenterId,
            Name = op.OperationCenter.Name,
            IsActive = op.OperationCenter.IsActive,
        }).ToListAsync(cancellationToken);

    }

}
