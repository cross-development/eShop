using AutoMapper;
using Infrastructure.Services;
using Infrastructure.Data.Interfaces;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.DTOs;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Repositories.Interfaces;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Services;

public sealed class CatalogBrandService : BaseDataService<ApplicationDbContext>, ICatalogBrandService
{
    private readonly ICatalogBrandRepository _catalogBrandRepository;
    private readonly IMapper _mapper;

    public CatalogBrandService(
        IApplicationDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogBrandRepository catalogBrandRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogBrandRepository = catalogBrandRepository;
        _mapper = mapper;
    }

    public async Task<DataResponse<CatalogBrandDto>> GetCatalogBrandsAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogBrandRepository.GetAllAsync();

            return new DataResponse<CatalogBrandDto>
            {
                Data = result.Select(brand => _mapper.Map<CatalogBrandDto>(brand)).ToList()
            };
        });
    }

    public async Task<CatalogBrandDto> GetCatalogBrandByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var item = await _catalogBrandRepository.GetByIdAsync(id);

            return _mapper.Map<CatalogBrandDto>(item);
        });
    }

    public async Task<CatalogBrand> FindCatalogBrandAsync(int id)
    {
        return await ExecuteSafeAsync(() => _catalogBrandRepository.FindOneAsync(id));
    }

    public async Task<CatalogBrand> AddCatalogBrandAsync(AddBrandRequest request)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var catalogBrand = _mapper.Map<CatalogBrand>(request);

            return await _catalogBrandRepository.AddAsync(catalogBrand);
        });
    }

    public async Task<CatalogBrand> UpdateCatalogBrandAsync(UpdateBrandRequest request, CatalogBrand catalogBrand)
    {
        return await ExecuteSafeAsync(async () =>
        {
            catalogBrand.Brand = request.Brand ?? catalogBrand.Brand;

            return await _catalogBrandRepository.UpdateAsync(catalogBrand);
        });
    }

    public async Task<bool> DeleteCatalogBrandAsync(CatalogBrand catalogBrand)
    {
        return await ExecuteSafeAsync(() => _catalogBrandRepository.DeleteAsync(catalogBrand));
    }
}