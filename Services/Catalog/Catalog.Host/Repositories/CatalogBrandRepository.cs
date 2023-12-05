using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Interfaces;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Repositories.Interfaces;

namespace Catalog.Host.Repositories;

public sealed class CatalogBrandRepository : BaseRepository<CatalogBrand>, ICatalogBrandRepository
{
    private readonly ILogger<CatalogBrandRepository> _logger;
    private readonly ApplicationDbContext _dbContext;

    public CatalogBrandRepository(
        ILogger<CatalogBrandRepository> logger,
        IApplicationDbContextWrapper<ApplicationDbContext> dbContextWrapper)
        : base(dbContextWrapper)
    {
        _logger = logger;
        _dbContext = dbContextWrapper.ApplicationDbContext;
    }

    public override async Task<IEnumerable<CatalogBrand>> GetAllAsync()
    {
        _logger.LogInformation("[CatalogBrandRepository: GetAllAsync] ==> GETTING CATALOG BRANDS DATA FROM DATABASE...\n");

        return await _dbContext.CatalogBrands
            .OrderBy(brand => brand.Brand)
            .ToListAsync();
    }

    public override async Task<CatalogBrand> GetByIdAsync(int id)
    {
        _logger.LogInformation("[CatalogBrandRepository: GetByIdAsync] ==> GETTING CATALOG BRAND DATA BY ID FROM DATABASE...\n");

        return await _dbContext.CatalogBrands
            .Include(brand => brand.CatalogItems)
            .FirstOrDefaultAsync(brand => brand.Id == id);
    }
}