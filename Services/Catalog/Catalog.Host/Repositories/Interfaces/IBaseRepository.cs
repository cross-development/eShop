using Catalog.Host.Models.Requests;

namespace Catalog.Host.Repositories.Interfaces;

public interface IBaseRepository<TEntity>
{
    Task<IEnumerable<TEntity>> GetAllAsync();

    Task<IEnumerable<TEntity>> GetAllAsync(PaginatedItemRequest request);

    Task<TEntity> GetByIdAsync(int id);

    Task<TEntity> FindOneAsync(int id);

    Task<int> GetCountAsync();

    Task<TEntity> AddAsync(TEntity entity);

    Task<TEntity> UpdateAsync(TEntity entity);

    Task<bool> DeleteAsync(TEntity entity);
}