﻿using Basket.Host.Models.DTOs;
using Basket.Host.Models.Responses;
using Basket.Host.Services.Interfaces;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        _logger.LogInformation($"[BasketService: GetBasketAsync] ==> USER ID: {userId}");

        var result = await _cacheService.GetAsync<List<BasketDataDto>>(userId);

        _logger.LogInformation($"[BasketService: GetBasketAsync] ==> REQUESTED DATA: {result}");

        return new GetBasketResponse { Data = result ?? Enumerable.Empty<BasketDataDto>() };
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

    public async Task<bool> DeleteItemAsync(string userId, int id)
    {
        _logger.LogInformation($"[BasketService: DeleteItemAsync] ==> USER ID: {userId}");
        _logger.LogInformation($"[BasketService: DeleteItemAsync] ==> PROVIDED DATA: {id}");

        var result = await _cacheService.GetAsync<List<BasketDataDto>>(userId);

        if (result == null || !result.Any())
        {
            _logger.LogInformation("[BasketService: DeleteItemAsync] ==> THERE IS NOTHING TO DELETE");

            return false;
        }

        var requestedItem = result.FirstOrDefault(data => data.Id == id);

        if (requestedItem == null)
        {
            _logger.LogInformation("[BasketService: DeleteItemAsync] ==> ITEM WITH REQUESTED ID NOT FOUND");

            return false;
        }

        var filteredResult = result.Where(data => data.Id != id).ToList();

        return await _cacheService.AddOrUpdateAsync(userId, filteredResult);
    }
}