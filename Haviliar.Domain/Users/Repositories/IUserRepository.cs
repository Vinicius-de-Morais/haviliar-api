using Haviliar.Domain.Users.Entities;
using Haviliar.Domain.Users.Repositories.Filters;
using Haviliar.Domain.Users.Repositories.Projections;

namespace Haviliar.Domain.Users.Repositories;

public interface IUserRepository : IRepositoryBase<User>
{
    Task<IEnumerable<UsersPaginatedProjection>> GetUsersPaginatedAsync(UserFilter userFilter, CancellationToken cancellationToken);
    int? GetCurrentUserId();
}
