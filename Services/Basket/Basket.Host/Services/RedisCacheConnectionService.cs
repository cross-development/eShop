using Microsoft.Extensions.Options;
using StackExchange.Redis;
using Basket.Host.Configurations;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Services;

public sealed class RedisCacheConnectionService : IRedisCacheConnectionService, IDisposable
{
    private bool _disposed;
    private readonly Lazy<ConnectionMultiplexer> _connectionLazy;

    public RedisCacheConnectionService(IOptions<RedisConfiguration> config)
    {
        var redisConfigurationOptions = ConfigurationOptions.Parse(config.Value.Host);

        _connectionLazy = new Lazy<ConnectionMultiplexer>(() =>
            ConnectionMultiplexer.Connect(redisConfigurationOptions));
    }

    public IConnectionMultiplexer Connection => _connectionLazy.Value;

    public void Dispose()
    {
        if (!_disposed)
        {
            Connection.Dispose();

            _disposed = true;
        }
    }
}