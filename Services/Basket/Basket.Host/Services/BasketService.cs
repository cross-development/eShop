using Basket.Host.Models.DTOs;
using Basket.Host.Models.Responses;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Services;

public sealed class BasketService : IBasketService
{
    private readonly ILogger<BasketService> _logger;
    private readonly ICacheService _cacheService;

    public BasketService(ILogger<BasketService> logger, ICacheService cacheService)
    {
        _logger = logger;
        _cacheService = cacheService;
    }

    public async Task<GetBasketResponse> GetBasketAsync(string userId)
    {
        _logger.LogInformation($"[BasketService: GetBasketAsync] ==> USER ID {userId}\n");

        var result = await _cacheService.GetAsync<List<BasketDataDto>>(userId);

        _logger.LogInformation($"[BasketService: GetBasketAsync] ==> REQUESTED DATA {result}\n");

        return new GetBasketResponse { Data = result ?? Enumerable.Empty<BasketDataDto>() };
    }

    public async Task<bool> AddItemAsync(string userId, BasketDataDto data)
    {
        _logger.LogInformation($"[BasketService: AddItemAsync] ==> USER ID {userId}\n");
        _logger.LogInformation($"[BasketService: AddItemAsync] ==> PROVIDED DATA {data}\n");

        var result = await _cacheService.GetAsync<List<BasketDataDto>>(userId);

        if (result == null)
        {
            _logger.LogInformation("[BasketService: AddItemAsync] ==> ADDING DATA FIRST TIME\n");

            return await _cacheService.AddOrUpdateAsync(userId, new List<BasketDataDto> { data });
        }

        var existingItemIndex = result.FindIndex(basketData => basketData.Id == data.Id);

        if (existingItemIndex == -1)
        {
            _logger.LogInformation("[BasketService: AddItemAsync] ==> ITEM WAS ADDED TO THE BASKET\n");

            result.Add(data);
        }
        else
        {
            _logger.LogInformation("[BasketService: AddItemAsync] ==> AMOUNT OF ITEMS WAS INCREASED\n");

            result[existingItemIndex].Amount++;
        }

        return await _cacheService.AddOrUpdateAsync(userId, result);
    }

    public async Task<bool> DeleteAllAsync(string userId)
    {
        _logger.LogInformation($"[BasketService: DeleteAllAsync] ==> USER ID {userId}\n");

        var result = await _cacheService.GetAsync<List<BasketDataDto>>(userId);

        if (result == null || !result.Any())
        {
            _logger.LogInformation("[BasketService: DeleteAllAsync] ==> THERE IS NOTHING TO DELETE\n");

            return false;
        }

        return await _cacheService.AddOrUpdateAsync(userId, Enumerable.Empty<BasketDataDto>());
    }

    public async Task<bool> DeleteItemAsync(string userId, int id)
    {
        _logger.LogInformation($"[BasketService: DeleteItemAsync] ==> USER ID {userId}\n");
        _logger.LogInformation($"[BasketService: DeleteItemAsync] ==> PROVIDED DATA {id}\n");

        var result = await _cacheService.GetAsync<List<BasketDataDto>>(userId);

        if (result == null || !result.Any())
        {
            _logger.LogInformation("[BasketService: DeleteItemAsync] ==> THERE IS NOTHING TO DELETE\n");

            return false;
        }

        var existingItemIndex = result.FindIndex(data => data.Id == id);

        if (existingItemIndex == -1)
        {
            _logger.LogInformation("[BasketService: DeleteItemAsync] ==> ITEM WITH REQUESTED ID NOT FOUND\n");

            return false;
        }

        if (result[existingItemIndex].Amount > 1)
        {
            _logger.LogInformation($"[BasketService: DeleteItemAsync] ==> AMOUNT OF ITEMS {result[existingItemIndex].Amount}\n");

            result[existingItemIndex].Amount--;
        }
        else
        {
            _logger.LogInformation("[BasketService: DeleteItemAsync] ==> AMOUNT OF ITEMS IS 1\n");

            result = result.Where(data => data.Id != id).ToList();
        }

        return await _cacheService.AddOrUpdateAsync(userId, result);
    }
}