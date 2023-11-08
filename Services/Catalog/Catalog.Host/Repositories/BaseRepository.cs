using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Interfaces;
using Catalog.Host.Data;
using Catalog.Host.Models.Requests;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public abstract class BaseRepository<TEntity> : IBaseRepository<TEntity>
    where TEntity : class
{
    private readonly ApplicationDbContext _dbContext;

    protected BaseRepository(IApplicationDbContextWrapper<ApplicationDbContext> dbContextWrapper)
    {
        _dbContext = dbContextWrapper.ApplicationDbContext;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
    {
        return await _dbContext.Set<TEntity>().ToListAsync();
    }

    public virtual async Task<IEnumerable<TEntity>> GetAllAsync(PaginatedItemRequest request)
    {
        IQueryable<TEntity> query = _dbContext.Set<TEntity>();

        if (request is { Page: > 0, Limit: > 0 })
        {
            query = query.Skip((request.Page - 1) * request.Limit).Take(request.Limit);
        }

        return await query.ToListAsync();
    }

    public abstract Task<TEntity> GetByIdAsync(int id);

    public async Task<TEntity> FindOneAsync(int id)
    {
        return await _dbContext.Set<TEntity>().FindAsync(id);
    }

    public async Task<int> GetCountAsync()
    {
        return await _dbContext.Set<TEntity>().CountAsync();
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        var item = await _dbContext.AddAsync(entity);

        await _dbContext.SaveChangesAsync();

        return item.Entity;
    }

    public async Task<TEntity> UpdateAsync(TEntity entity)
    {
        var item = _dbContext.Set<TEntity>().Update(entity);

        await _dbContext.SaveChangesAsync();

        return item.Entity;
    }

    public async Task<bool> DeleteAsync(TEntity entity)
    {
        _dbContext.Remove(entity);

        return await _dbContext.SaveChangesAsync() > 0;
    }
}