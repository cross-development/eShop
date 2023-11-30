using Basket.Host.Models.DTOs;
using Basket.Host.Models.Responses;

namespace Basket.Host.Services.Interfaces;

public interface IBasketService
{
    Task<GetBasketResponse> GetBasketAsync(string userId);

    Task<bool> AddItemAsync(string userId, BasketDataDto data);

    Task<bool> DeleteItemAsync(string userId, int id);
}