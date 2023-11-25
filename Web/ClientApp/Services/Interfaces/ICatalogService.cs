using Microsoft.AspNetCore.Mvc.Rendering;
using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;

namespace ClientApp.Services.Interfaces;

public interface ICatalogService
{
    Task<CatalogResponseDto> GetCatalogItems(CatalogRequestDto catalogRequest);

    Task<ItemResponseDto> GetCatalogItemById(int id);

    Task<IEnumerable<SelectListItem>> GetBrands();

    Task<IEnumerable<SelectListItem>> GetTypes();
}