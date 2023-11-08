using AutoMapper;
using Infrastructure.Services;
using Infrastructure.Data.Interfaces;
using Catalog.Host.Data;
using Catalog.Host.Data.Entities;
using Catalog.Host.Models.DTOs;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Interfaces;
using Catalog.Host.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Catalog.Host.Services;

public sealed class CatalogItemService : BaseDataService<ApplicationDbContext>, ICatalogItemService
{
    private readonly ICatalogItemRepository _catalogItemRepository;
    private readonly IMapper _mapper;

    public CatalogItemService(
        IApplicationDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        ICatalogItemRepository catalogItemRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _catalogItemRepository = catalogItemRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<CatalogItemDto>> GetCatalogItemsAsync(PaginatedItemRequest request)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _catalogItemRepository.GetAllAsync(request);

            var totalCount = await _catalogItemRepository.GetCountAsync();

            return new PaginatedResponse<CatalogItemDto>
            {
                Page = request.Page,
                Limit = request.Limit,
                Count = totalCount,
                Data = result.Select(item => _mapper.Map<CatalogItemDto>(item)).ToList()
            };
        });
    }

    public async Task<CatalogItemDto> GetCatalogItemByIdAsync(int id)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var item = await _catalogItemRepository.GetByIdAsync(id);

            return _mapper.Map<CatalogItemDto>(item);
        });
    }

    public async Task<CatalogItem> FindCatalogItemAsync(int id)
    {
        return await ExecuteSafeAsync(() => _catalogItemRepository.FindOneAsync(id));
    }

    public async Task<CatalogItem> AddCatalogItemAsync(AddItemRequest request)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var catalogItem = _mapper.Map<CatalogItem>(request);

            return await _catalogItemRepository.AddAsync(catalogItem);
        });
    }

    public async Task<CatalogItem> UpdateCatalogItemAsync(UpdateItemRequest request, CatalogItem catalogItem)
    {
        return await ExecuteSafeAsync(async () =>
        {
            catalogItem.Name = request.Name ?? catalogItem.Name;
            catalogItem.Description = request.Description ?? catalogItem.Description;
            catalogItem.Price = request.Price ?? catalogItem.Price;
            catalogItem.AvailableStock = request.AvailableStock ?? catalogItem.AvailableStock;
            catalogItem.CatalogTypeId = request.CatalogTypeId ?? catalogItem.CatalogTypeId;
            catalogItem.CatalogBrandId = request.CatalogBrandId ?? catalogItem.CatalogBrandId;
            catalogItem.PictureFileName = request.PictureFileName ?? catalogItem.PictureFileName;

            return await _catalogItemRepository.UpdateAsync(catalogItem);
        });
    }

    public async Task<bool> DeleteCatalogItemAsync(CatalogItem catalogItem)
    {
        return await ExecuteSafeAsync(() => _catalogItemRepository.DeleteAsync(catalogItem));
    }
}