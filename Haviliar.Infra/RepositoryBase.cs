using System.Linq.Expressions;
using Haviliar.Domain;
using Haviliar.Infra.Context;
using Microsoft.EntityFrameworkCore;

namespace Haviliar.Infra;

public class RepositoryBase<T> : IRepositoryBase<T> where T : class
{

    protected readonly AppDbContext _context;
    protected readonly DbSet<T> _dbSet;

    public RepositoryBase(AppDbContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();

    }

    public Task<bool> AlreadyExistAsync(Expression<Func<T, bool>> filter, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }

    public Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        throw new NotImplementedException();
    }
}
