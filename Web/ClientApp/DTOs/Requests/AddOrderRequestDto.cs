namespace ClientApp.DTOs.Requests;

public sealed class AddOrderRequestDto
{
    public string Name { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Products { get; set; }

    public uint Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}