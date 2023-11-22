namespace Basket.Host.Services.Interfaces;

public interface ICacheService
{
    Task<T> GetAsync<T>(string key);

    Task<bool> AddOrUpdateAsync<T>(string key, T value);
}