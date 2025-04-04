using Domain.Repositories.Common;
using Infrastructure.Database.EntityFramework.Context;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Database.EntityFramework.Repositories.Common;

public class GenericRepository<TEntity> where TEntity : class
{
    protected readonly HelpDeskDbContext _context;
    protected readonly DbSet<TEntity> _dbSet;

    public GenericRepository(HelpDeskDbContext context)
    {
        _context = context;
        _dbSet = context.Set<TEntity>();
    }

    // Estos métodos ahora solo modifican el estado del DbContext
    // La llamada a SaveChangesAsync se hará explícitamente donde se necesite.

    public virtual async Task<TEntity> CreateAsyncBase(TEntity entity) // Renombrado para evitar colisión
    {
        var result = await _dbSet.AddAsync(entity);
        // NO SaveChangesAsync()
        return result.Entity;
    }

    public virtual Task<TEntity> UpdateAsyncBase(TEntity entity) // Renombrado para evitar colisión
    {
        _context.Entry(entity).State = EntityState.Modified;
        // NO SaveChangesAsync()
        return Task.FromResult(entity);
    }

    public virtual async Task<bool> DeleteAsyncBase(int id) // Renombrado para evitar colisión
    {
        // Asume que TEntity tiene Id o implementa IIdentifiable
        var entity = await _dbSet.FindAsync(id);
        if (entity == null) return false;
        _dbSet.Remove(entity);
        // NO SaveChangesAsync()
        return true;
    }

    // Métodos de lectura
    public virtual async Task<TEntity?> GetByIdAsyncBase(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public virtual async Task<List<TEntity>> GetAllAsyncBase()
    {
        return await _dbSet.AsNoTracking().ToListAsync();
    }
}