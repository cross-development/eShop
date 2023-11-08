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

public sealed class CatalogTypeService : BaseDataService<ApplicationDbContext>, ICatalogTypeService
{
    private readonly ICatalogTypeRepository _catalogTypeRepository;
    private readonly IMapper _mapper;

    public CatalogTypeService(
        IApplicationDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogTypeRepository catalogTypeRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogTypeRepository = catalogTypeRepository;
        _mapper = mapper;
    }

    public async Task<DataResponse<CatalogTypeDto>> GetCatalogTypesAsync()
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogTypeRepository.GetAllAsync();

            return new DataResponse<CatalogTypeDto>
            {
                Data = result.Select(type => _mapper.Map<CatalogTypeDto>(type)).ToList()
            };
        });
    }

    public async Task<CatalogTypeDto> GetCatalogTypeByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var item = await _catalogTypeRepository.GetByIdAsync(id);

            return _mapper.Map<CatalogTypeDto>(item);
        });
    }

    public async Task<CatalogType> FindCatalogTypeAsync(int id)
    {
        return await ExecuteSafeAsync(() => _catalogTypeRepository.FindOneAsync(id));
    }

    public async Task<CatalogType> AddCatalogTypeAsync(AddTypeRequest request)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var catalogType = _mapper.Map<CatalogType>(request);

            return await _catalogTypeRepository.AddAsync(catalogType);
        });
    }

    public async Task<CatalogType> UpdateCatalogTypeAsync(UpdateTypeRequest request, CatalogType catalogType)
    {
        return await ExecuteSafeAsync(async () =>
        {
            catalogType.Type = request.Type ?? catalogType.Type;

            return await _catalogTypeRepository.UpdateAsync(catalogType);
        });
    }

    public async Task<bool> DeleteCatalogTypeAsync(CatalogType catalogType)
    {
        return await ExecuteSafeAsync( () => _catalogTypeRepository.DeleteAsync(catalogType));
    }
}