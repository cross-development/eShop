using Catalog.Host.Data.Entities;
using Catalog.Host.Models.DTOs;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;

namespace Catalog.Host.Services.Interfaces;

public interface ICatalogItemService
{
    Task<PaginatedResponse<CatalogItemDto>> GetCatalogItemsAsync(PaginatedItemRequest request);

    Task<CatalogItemDto> GetCatalogItemByIdAsync(int id);

    Task<CatalogItem> FindCatalogItemAsync(int id);

    Task<CatalogItem> AddCatalogItemAsync(AddItemRequest request);

    Task<CatalogItem> UpdateCatalogItemAsync(UpdateItemRequest request, CatalogItem catalogItem);

    Task<bool> DeleteCatalogItemAsync(CatalogItem catalogItem);
}