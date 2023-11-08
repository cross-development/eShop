using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Interfaces;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.Requests;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public sealed class CatalogItemRepository : BaseRepository<CatalogItem>, ICatalogItemRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CatalogItemRepository(IApplicationDbContextWrapper<ApplicationDbContext> dbContextWrapper)
        : base(dbContextWrapper)
    {
        _dbContext = dbContextWrapper.ApplicationDbContext;
    }

    public override async Task<IEnumerable<CatalogItem>> GetAllAsync(PaginatedItemRequest request)
    {
        IQueryable<CatalogItem> query = _dbContext.CatalogItems;

        if (request?.BrandId != null)
        {
            query = query.Where(item => item.CatalogBrandId == request.BrandId);
        }

        if (request?.TypeId != null)
        {
            query = query.Where(item => item.CatalogTypeId == request.TypeId);
        }

        if (request?.Page > 0 && request?.Limit > 0)
        {
            query = query.Skip((request.Page - 1) * request.Limit).Take(request.Limit);
        }

        return await query
            .OrderBy(item => item.Name)
            .Include(item => item.CatalogBrand)
            .Include(item => item.CatalogType)
            .ToListAsync();
    }

    public override async Task<CatalogItem> GetByIdAsync(int id)
    {
        return await _dbContext.CatalogItems
            .Include(item => item.CatalogBrand)
            .Include(item => item.CatalogType)
            .FirstOrDefaultAsync(item => item.Id == id);
    }
}