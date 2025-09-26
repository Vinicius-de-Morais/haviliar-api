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

    public async Task<bool> AlreadyExistAsync(Expression<Func<T, bool>> filtro, CancellationToken cancellationToken)
    {
        return await _dbSet.AnyAsync(filtro, cancellationToken);
    }

    public async Task UpdateAsync(T entity, CancellationToken cancellationToken = default)
    {
        _context.Update(entity);

        await _context.SaveChangesAsync(cancellationToken);
    }

    public async Task<T?> GetByIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await _dbSet.FindAsync(id, cancellationToken);
    }

    public async Task<T> InsertAsync(T entity, CancellationToken cancellationToken = default)
    {
        await _dbSet.AddAsync(entity, cancellationToken);

        await _context.SaveChangesAsync(cancellationToken);

        return entity;
    }
}
