using Microsoft.Extensions.Options;
using ClientApp.Configurations;
using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;
using ClientApp.Services.Interfaces;

namespace ClientApp.Services;

public sealed class BasketService : IBasketService
{
    private readonly IHttpClientService _httpClientService;
    private readonly ApiConfiguration _apiOptions;

    public BasketService(IHttpClientService httpClientService, IOptions<ApiConfiguration> apiOptions)
    {
        _httpClientService = httpClientService;
        _apiOptions = apiOptions.Value;
    }
    public async Task<BasketResponseDto> GetBasketAsync()
    {
        var result = await _httpClientService.SendAsync<BasketResponseDto>(
            $"{_apiOptions.BasketUrl}/basket-bff/items",
            HttpMethod.Get);

        return result;
    }

    public async Task<bool> AddToBasketAsync(BasketRequestDto request)
    {
        var result = await _httpClientService.SendAsync<bool, object, BasketRequestDto>(
            $"{_apiOptions.BasketUrl}/basket-item/add",
            HttpMethod.Post,
            null,
            request);

        return result;
    }
    
    public async Task<bool> DeleteAllFromBasketAsync()
    {
        var result = await _httpClientService.SendAsync<bool>(
            $"{_apiOptions.BasketUrl}/basket-item/delete",
            HttpMethod.Delete);

        return result;
    }

    public async Task<bool> DeleteFromBasketByIdAsync(int id)
    {
        var result = await _httpClientService.SendAsync<bool>(
            $"{_apiOptions.BasketUrl}/basket-item/delete/{id}",
            HttpMethod.Delete);

        return result;
    }
}