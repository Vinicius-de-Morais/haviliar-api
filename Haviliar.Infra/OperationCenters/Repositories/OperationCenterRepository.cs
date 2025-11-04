using Haviliar.Domain.OperationCenters.Entities;
using Haviliar.Domain.OperationCenters.Repositories;
using Haviliar.Domain.OperationCenters.Repositories.Filters;
using Haviliar.Domain.OperationCenters.Repositories.Projections;
using Haviliar.Domain.Users.Repositories.Filters;
using Haviliar.Domain.Users.Repositories.Projections;
using Haviliar.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Haviliar.Infra.OperationCenters.Repositories;

public class OperationCenterRepository : RepositoryBase<OperationCenter>, IOperationCenterRepository
{
    public OperationCenterRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<OperationCenterPaginatedProjection>> GetOperationCentersPaginatedAsync(OperationCenterFilter filter, CancellationToken cancellationToken)
    {
        var collation = "Latin1_General_CI_AI";
        var query = _dbSet
        .AsNoTracking()
            .Where(u => filter.Search == null ||
            EF.Functions.Collate(u.Name, collation).Contains(EF.Functions.Collate(filter.Search, collation)));

        filter.TotalItems = await query.CountAsync(cancellationToken);

        var paginatedQuery = GetAllPaginated(query, filter);


        return await paginatedQuery.Select(op => new OperationCenterPaginatedProjection
        {
            OperationCenterId = op.OperationCenterId,
            Name = op.Name,
            IsActive = op.IsActive
        }).ToListAsync(cancellationToken);
    }
}
