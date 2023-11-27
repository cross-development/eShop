﻿using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using StackExchange.Redis;
using Basket.Host.Configurations;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Services;

public sealed class CacheService : ICacheService
{
    private readonly ILogger<CacheService> _logger;
    private readonly RedisConfiguration _config;
    private readonly IRedisCacheConnectionService _redisCacheConnectionService;

    public CacheService(
        ILogger<CacheService> logger,
        IOptions<RedisConfiguration> config,
        IRedisCacheConnectionService redisCacheConnectionService)
    {
        _logger = logger;
        _config = config.Value;
        _redisCacheConnectionService = redisCacheConnectionService;
    }

    private IDatabase GetRedisDatabase() => _redisCacheConnectionService.Connection.GetDatabase();

    public async Task<T> GetAsync<T>(string key)
    {
        _logger.LogInformation($"[CacheService: GetAsync] ==> KEY TO GET CACHE DATA: {key}");

        var redis = GetRedisDatabase();

        var serialized = await redis.StringGetAsync(key);

        _logger.LogInformation($"[CacheService: GetAsync] ==> SERIALIZED CACHE DATA: {serialized}");

        var cachedData = serialized.HasValue ? JsonConvert.DeserializeObject<T>(serialized.ToString()) : default;

        _logger.LogInformation($"[CacheService: GetAsync] ==> RECEIVED CACHE DATA: {cachedData}");

        return cachedData;
    }

    public async Task<bool> AddOrUpdateAsync<T>(string key, T value)
    {
        _logger.LogInformation($"[CacheService: AddOrUpdateAsync] ==> KEY TO CACHE DATA: {key}");

        var redis = GetRedisDatabase();

        var serialized = JsonConvert.SerializeObject(value);

        _logger.LogInformation($"[CacheService: AddOrUpdateAsync] ==> SERIALIZED DATA: {serialized}");

        var isDataCached = await redis.StringSetAsync(key, serialized, _config.CacheTimeout);

        _logger.LogInformation(isDataCached
            ? $"[CacheService: AddOrUpdateAsync] ==> DATA WAS CACHED WITH KEY: {key}"
            : $"[CacheService: AddOrUpdateAsync] ==> DATA WAS UPDATED WITH KEY: {key}");

        return isDataCached;
    }
}