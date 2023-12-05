using Microsoft.Extensions.Options;
using ClientApp.Configurations;
using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;
using ClientApp.Services.Interfaces;

namespace ClientApp.Services;

public sealed class BasketService : IBasketService
{
    private readonly ILogger<BasketService> _logger;
    private readonly IHttpClientService _httpClientService;
    private readonly ApiConfiguration _apiOptions;

    public BasketService(
        ILogger<BasketService> logger,
        IHttpClientService httpClientService,
        IOptions<ApiConfiguration> apiOptions)
    {
        _logger = logger;
        _httpClientService = httpClientService;
        _apiOptions = apiOptions.Value;
    }
    public async Task<BasketResponseDto> GetBasketAsync()
    {
        _logger.LogInformation("[BasketService: GetBasketAsync] ==> FETCHING BASKET DATA...\n");

        var result = await _httpClientService.SendAsync<BasketResponseDto>(
            $"{_apiOptions.BasketUrl}/basket-bff/items",
            HttpMethod.Get);

        return result;
    }

    public async Task<bool> AddToBasketAsync(BasketRequestDto request)
    {
        _logger.LogInformation("[BasketService: AddToBasketAsync] ==> ADDING BASKET DATA...\n");

        var result = await _httpClientService.SendAsync<bool, object, BasketRequestDto>(
            $"{_apiOptions.BasketUrl}/basket-item/add",
            HttpMethod.Post,
            null,
            request);

        return result;
    }

    public async Task<bool> DeleteAllFromBasketAsync()
    {
        _logger.LogInformation("[BasketService: DeleteAllFromBasketAsync] ==> DELETING ALL BASKET DATA...\n");

        var result = await _httpClientService.SendAsync<bool>(
            $"{_apiOptions.BasketUrl}/basket-item/delete",
            HttpMethod.Delete);

        return result;
    }

    public async Task<bool> DeleteFromBasketByIdAsync(int id)
    {
        _logger.LogInformation("[BasketService: DeleteFromBasketByIdAsync] ==> DELETING BY ID FROM BASKET DATA...\n");

        var result = await _httpClientService.SendAsync<bool>(
            $"{_apiOptions.BasketUrl}/basket-item/delete/{id}",
            HttpMethod.Delete);

        return result;
    }
}