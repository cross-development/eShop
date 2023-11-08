using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Interfaces;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Requests;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public sealed class CatalogTypeRepository : BaseRepository<CatalogType>, ICatalogTypeRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CatalogTypeRepository(IApplicationDbContextWrapper<ApplicationDbContext> dbContextWrapper)
        : base(dbContextWrapper)
    {
        _dbContext = dbContextWrapper.ApplicationDbContext;
    }

    public override async Task<IEnumerable<CatalogType>> GetAllAsync(PaginatedItemRequest request)
    {
        return await _dbContext.CatalogTypes
            .OrderBy(type => type.Type)
            .ToListAsync();
    }

    public override async Task<CatalogType> GetByIdAsync(int id)
    {
       return await _dbContext.CatalogTypes
            .Include(type => type.CatalogItems)
            .FirstOrDefaultAsync(item => item.Id == id);
    }
}