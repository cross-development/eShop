namespace ClientApp.DTOs.Requests;

public sealed class AddOrderRequestDto
{
    public string Name { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Products { get; set; }

    public int Quantity { get; set; }

    public int TotalPrice { get; set; }

    public string UserId { get; set; }
}