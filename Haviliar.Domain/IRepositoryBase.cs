using System.Linq.Expressions;

namespace Haviliar.Domain;

public interface IRepositoryBase<T> where T : class
{
    Task<bool> AlreadyExistAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken);
    Task UpdateAsync(T entity, CancellationToken cancellationToken = default);
    Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default);
    Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default);
}
