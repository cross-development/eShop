using Microsoft.AspNetCore.Mvc.Rendering;
using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;

namespace ClientApp.Services.Interfaces;

public interface ICatalogService
{
    Task<CatalogResponseDto> GetCatalogItemsAsync(CatalogRequestDto catalogRequest);

    Task<ItemResponseDto> GetCatalogItemByIdAsync(int id);

    Task<IEnumerable<SelectListItem>> GetBrandsAsync();

    Task<IEnumerable<SelectListItem>> GetTypesAsync();
}