using Haviliar.Domain.OperationCenters.Entities;
using Haviliar.Domain.OperationCenters.Repositories.Filters;
using Haviliar.Domain.OperationCenters.Repositories.Projections;
using Haviliar.Domain.Users.Entities;

namespace Haviliar.Domain.Users.Repositories;

public interface IUserOperationCenterRepository : IRepositoryBase<UserOperationCenter>
{
    Task<OperationCenter?> GetByAuthUser(int operationCenterId, int userId, CancellationToken cancellationToken);
    Task<IEnumerable<OperationCenterPaginatedProjection>> GetOperationCentersPaginatedAsync(OperationCenterFilter filter, int userId, CancellationToken cancellationToken);
}
