namespace Order.Host.Models.Requests;

public sealed class AddOrderRequest
{
    public string Name { get; set; }

    public DateTime Date { get; set; }

    public string Products { get; set; }

    public uint Quantity { get; set; }

    public decimal TotalPrice { get; set; }
}