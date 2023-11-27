using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;

namespace ClientApp.Services.Interfaces;

public interface IBasketService
{
    Task<bool> AddToBasketAsync(BasketRequestDto data);

    Task<BasketResponseDto> GetBasketAsync();
}