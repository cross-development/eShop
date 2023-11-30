namespace Order.Host.Models.DTOs;

public sealed class OrderItemDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime Date { get; set; }

    public string Products { get; set; }

    public int Quantity { get; set; }

    public int TotalPrice { get; set; }

    public string UserId { get; set; }
}