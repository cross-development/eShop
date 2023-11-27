using Basket.Host.Models.DTOs;

namespace Basket.Host.Models.Responses;

public sealed class GetBasketResponse
{
    public IEnumerable<BasketDataDto> Data { get; set; }
}