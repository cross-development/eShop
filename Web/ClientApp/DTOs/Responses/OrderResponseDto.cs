namespace ClientApp.DTOs.Responses;

public sealed class OrderResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Products { get; set; }

    public uint Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}