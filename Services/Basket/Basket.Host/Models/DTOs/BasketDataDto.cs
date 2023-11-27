namespace Basket.Host.Models.DTOs;

public sealed class BasketDataDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public int Amount { get; set; }

    public decimal Price { get; set; }

    public string PictureUrl { get; set; }
}