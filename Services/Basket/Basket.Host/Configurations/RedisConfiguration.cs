namespace Basket.Host.Configurations;

public sealed class RedisConfiguration
{
    public string Host { get; set; }

    public TimeSpan CacheTimeout { get; set; }
}