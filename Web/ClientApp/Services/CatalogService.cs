using Microsoft.Extensions.Options;
using Microsoft.AspNetCore.Mvc.Rendering;
using ClientApp.Models;
using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;
using ClientApp.Configurations;
using ClientApp.Services.Interfaces;

namespace ClientApp.Services;

public sealed class CatalogService : ICatalogService
{
    private readonly ILogger<CatalogService> _logger;
    private readonly IHttpClientService _httpClientService;
    private readonly ApiConfiguration _apiOptions;

    public CatalogService(
        ILogger<CatalogService> logger,
        IHttpClientService httpClientService, 
        IOptions<ApiConfiguration> apiOptions)
    {
        _logger = logger;
        _httpClientService = httpClientService;
        _apiOptions = apiOptions.Value;
    }

    public async Task<PaginatedResponseDto<CatalogItem>> GetCatalogItemsAsync(CatalogRequestDto request)
    {
        _logger.LogInformation("[CatalogService: GetCatalogItemsAsync] ==> FETCHING CATALOG ITEMS DATA...\n");

        var result = await _httpClientService.SendAsync<PaginatedResponseDto<CatalogItem>, CatalogRequestDto>(
            $"{_apiOptions.CatalogUrl}/catalog-bff/items",
            HttpMethod.Get,
            request);

        return result;
    }

    public async Task<ItemResponseDto> GetCatalogItemByIdAsync(int id)
    {
        _logger.LogInformation("[CatalogService: GetCatalogItemByIdAsync] ==> FETCHING CATALOG ITEM DATA BY ID...\n");

        var result = await _httpClientService.SendAsync<ItemResponseDto>(
            $"{_apiOptions.CatalogUrl}/catalog-bff/items/{id}",
            HttpMethod.Get);

        return result;
    }

    public async Task<IEnumerable<SelectListItem>> GetBrandsAsync()
    {
        _logger.LogInformation("[CatalogService: GetBrandsAsync] ==> FETCHING CATALOG BRANDS DATA...\n");

        var result = await _httpClientService.SendAsync<BrandResponseDto>(
            $"{_apiOptions.CatalogUrl}/catalog-bff/brands",
            HttpMethod.Get);

        var listOfBrands = result.Data
            .Select(catalogBrand => new SelectListItem
            {
                Value = $"{catalogBrand.Id}",
                Text = catalogBrand.Brand
            })
            .ToList();

        return listOfBrands;
    }

    public async Task<IEnumerable<SelectListItem>> GetTypesAsync()
    {
        _logger.LogInformation("[CatalogService: GetTypesAsync] ==> FETCHING CATALOG TYPES DATA...\n");

        var result = await _httpClientService.SendAsync<TypeResponseDto>(
            $"{_apiOptions.CatalogUrl}/catalog-bff/types",
            HttpMethod.Get);

        var listOfTypes = result.Data
            .Select(catalogType => new SelectListItem
            {
                Value = $"{catalogType.Id}",
                Text = catalogType.Type
            })
            .ToList();

        return listOfTypes;
    }
}