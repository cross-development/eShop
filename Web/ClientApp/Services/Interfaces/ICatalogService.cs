using Microsoft.AspNetCore.Mvc.Rendering;
using ClientApp.Models;
using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;

namespace ClientApp.Services.Interfaces;

public interface ICatalogService
{
    Task<PaginatedResponseDto<CatalogItem>> GetCatalogItemsAsync(CatalogRequestDto request);

    Task<ItemResponseDto> GetCatalogItemByIdAsync(int id);

    Task<IEnumerable<SelectListItem>> GetBrandsAsync();

    Task<IEnumerable<SelectListItem>> GetTypesAsync();
}