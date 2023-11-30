namespace Order.Host.Models.Requests;

public sealed class AddOrderRequest
{
    public string Name { get; set; }

    public DateTime Date { get; set; }

    public string Products { get; set; }

    public int Quantity { get; set; }

    public int TotalPrice { get; set; }

    public string UserId { get; set; }
}