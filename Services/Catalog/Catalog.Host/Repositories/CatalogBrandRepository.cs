using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Interfaces;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public sealed class CatalogBrandRepository : BaseRepository<CatalogBrand>, ICatalogBrandRepository
{
    private readonly ApplicationDbContext _dbContext;

    public CatalogBrandRepository(IApplicationDbContextWrapper<ApplicationDbContext> dbContextWrapper)
        : base(dbContextWrapper)
    {
        _dbContext = dbContextWrapper.ApplicationDbContext;
    }

    public override async Task<IEnumerable<CatalogBrand>> GetAllAsync()
    {
        return await _dbContext.CatalogBrands
            .OrderBy(brand => brand.Brand)
            .ToListAsync();
    }

    public override async Task<CatalogBrand> GetByIdAsync(int id)
    {
        return await _dbContext.CatalogBrands
            .Include(brand => brand.CatalogItems)
            .FirstOrDefaultAsync(brand => brand.Id == id);
    }
}