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

    public async Task<bool> AddItemAsync(string userId, string data)
    {
        _logger.LogInformation($"[BasketService: AddItemAsync] --> The user id is {userId}");
        _logger.LogInformation($"[BasketService: AddItemAsync] --> Provided data is {data}");

        return await _cacheService.AddOrUpdateAsync(userId, data);
    }

    public async Task<GetBasketResponse> GetBasketAsync(string userId)
    {
        _logger.LogInformation($"[BasketService: GetBasketAsync] --> The user id is {userId}");

        var result = await _cacheService.GetAsync<string>(userId);

        _logger.LogInformation($"[BasketService: GetBasketAsync] --> The response data is {result}");

        return new GetBasketResponse { Data = result };
    }
}