using Haviliar.Domain.OperationCenters.Entities;
using Haviliar.Domain.OperationCenters.Repositories.Filters;
using Haviliar.Domain.OperationCenters.Repositories.Projections;

namespace Haviliar.Domain.OperationCenters.Repositories;

public interface IOperationCenterRepository : IRepositoryBase<OperationCenter>
{
    Task<IEnumerable<OperationCenterPaginatedProjection>> GetOperationCentersPaginatedAsync(OperationCenterFilter filter, CancellationToken cancellationToken)
}
