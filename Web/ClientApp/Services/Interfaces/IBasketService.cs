using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;

namespace ClientApp.Services.Interfaces;

public interface IBasketService
{
    Task<BasketResponseDto> GetBasketAsync();

    Task<bool> AddToBasketAsync(BasketRequestDto request);
}