namespace Order.Host.Data.Entities;

public class OrderItem
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime Date { get; set; }

    public string Products { get; set; }

    public uint Quantity { get; set; }

    public uint TotalPrice { get; set; }

    public string UserId { get; set; }
}