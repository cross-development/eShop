using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;

namespace ClientApp.Services.Interfaces;

public interface IBasketService
{
    Task<bool> AddToBasketAsync(BasketRequestDto request);

    Task<BasketResponseDto> GetBasketAsync();

    Task<bool> DeleteAllFromBasketAsync();

    Task<bool> DeleteFromBasketByIdAsync(int id);
}