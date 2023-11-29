namespace ClientApp.DTOs.Responses;

public sealed class OrderResponseDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime Date { get; set; }

    public IEnumerable<string> Products { get; set; }

    public int Quantity { get; set; }

    public int TotalPrice { get; set; }

    public string UserId { get; set; }
}