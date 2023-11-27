using ClientApp.Models;

namespace ClientApp.DTOs.Responses;

public sealed class BasketResponseDto
{
    public IEnumerable<BasketData> Data { get; set; }
}