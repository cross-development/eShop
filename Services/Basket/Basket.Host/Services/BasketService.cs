﻿using Basket.Host.Models.DTOs;
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

    public async Task<bool> AddItemAsync(string userId, BasketDataDto data)
    {
        _logger.LogInformation($"[BasketService: AddItemAsync] ==> USER ID: {userId}");
        _logger.LogInformation($"[BasketService: AddItemAsync] ==> PROVIDED DATA: {data}");

        var result = await _cacheService.GetAsync<List<BasketDataDto>>(userId);

        if (result == null)
        {
            return await _cacheService.AddOrUpdateAsync(userId, new List<BasketDataDto> { data });
        }

        result.Add(data);

        return await _cacheService.AddOrUpdateAsync(userId, result);
    }

    public async Task<GetBasketResponse> GetBasketAsync(string userId)
    {
        _logger.LogInformation($"[BasketService: GetBasketAsync] ==> USER ID: {userId}");

        var result = await _cacheService.GetAsync<List<BasketDataDto>>(userId);

        _logger.LogInformation($"[BasketService: GetBasketAsync] ==> REQUESTED DATA: {result}");

        return new GetBasketResponse { Data = result ?? Enumerable.Empty<BasketDataDto>() };
    }
}