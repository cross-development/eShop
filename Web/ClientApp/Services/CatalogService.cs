using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;
using ClientApp.Configurations;
using ClientApp.Services.Interfaces;

namespace ClientApp.Services;

public sealed class CatalogService : ICatalogService
{
    private readonly IHttpClientService _httpClientService;
    private readonly ApiConfiguration _apiOptions;

    public CatalogService(IHttpClientService httpClientService, IOptions<ApiConfiguration> apiOptions)
    {
        _httpClientService = httpClientService;
        _apiOptions = apiOptions.Value;
    }

    public async Task<CatalogResponseDto> GetCatalogItems(CatalogRequestDto catalogRequest)
    {
        var result = await _httpClientService.SendAsync<CatalogResponseDto, CatalogRequestDto>(
            $"{_apiOptions.CatalogUrl}{_apiOptions.CatalogPath}/items",
            HttpMethod.Get,
            catalogRequest);

        return result;
    }

    public async Task<ItemResponseDto> GetCatalogItemById(int id)
    {
        var result = await _httpClientService.SendAsync<ItemResponseDto>(
            $"{_apiOptions.CatalogUrl}{_apiOptions.CatalogPath}/items/{id}",
            HttpMethod.Get);

        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrands()
    {
        var result = await _httpClientService.SendAsync<BrandResponseDto>(
            $"{_apiOptions.CatalogUrl}{_apiOptions.CatalogPath}/brands",
            HttpMethod.Get);

        var listOfBrands = result.Data
            .Select(catalogBrand => new SelectListItem { Value = $"{catalogBrand.Id}", Text = catalogBrand.Brand })
            .ToList();

        return listOfBrands;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypes()
    {
        var result = await _httpClientService.SendAsync<TypeResponseDto>(
            $"{_apiOptions.CatalogUrl}{_apiOptions.CatalogPath}/types",
            HttpMethod.Get);

        var listOfTypes = result.Data
            .Select(catalogType => new SelectListItem { Value = $"{catalogType.Id}", Text = catalogType.Type })
            .ToList();

        return listOfTypes;
    }
}