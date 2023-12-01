namespace Order.Host.Models.DTOs;

public sealed class OrderItemDto
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime Date { get; set; }

    public string Products { get; set; }

    public uint Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}