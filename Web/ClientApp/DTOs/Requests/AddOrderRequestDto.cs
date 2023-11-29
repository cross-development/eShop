namespace ClientApp.DTOs.Requests;

public sealed class AddOrderRequestDto
{
    public string Name { get; set; }

    public DateTime Date { get; set; }

    public IEnumerable<string> Products { get; set; }

    public int Quantity { get; set; }

    public int TotalPrice { get; set; }

    public string UserId { get; set; }
}