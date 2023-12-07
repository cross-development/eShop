using Microsoft.Extensions.Options;
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

    public async Task<T> GetAsync<T>(string key)
    {
        _logger.LogInformation($"[CacheService: GetAsync] ==> KEY FOR GETTING CACHE DATA: {key}\n");

        var redis = GetRedisDatabase();

        var serialized = await redis.StringGetAsync(key);

        _logger.LogInformation($"[CacheService: GetAsync] ==> SERIALIZED CACHE DATA: {serialized}\n");

        var cachedData = serialized.HasValue ? JsonConvert.DeserializeObject<T>(serialized.ToString()) : default;

        _logger.LogInformation($"[CacheService: GetAsync] ==> DESERIALIZED CACHE DATA: {cachedData}");

        return cachedData;
    }

    public async Task<bool> AddOrUpdateAsync<T>(string key, T value)
    {
        _logger.LogInformation($"[CacheService: AddOrUpdateAsync] ==> KEY FOR GETTING CACHE DATA: {key}\n");

        var redis = GetRedisDatabase();

        var serialized = JsonConvert.SerializeObject(value);

        _logger.LogInformation($"[CacheService: AddOrUpdateAsync] ==> SERIALIZED DATA: {serialized}\n");

        var isDataCached = await redis.StringSetAsync(key, serialized, _config.CacheTimeout);

        _logger.LogInformation(isDataCached
            ? $"[CacheService: AddOrUpdateAsync] ==> DATA WAS CACHED WITH KEY: {key}\n"
            : $"[CacheService: AddOrUpdateAsync] ==> DATA WAS NOT CACHED WITH KEY: {key}\n");

        return isDataCached;
    }

    private IDatabase GetRedisDatabase() => _redisCacheConnectionService.Connection.GetDatabase();
}