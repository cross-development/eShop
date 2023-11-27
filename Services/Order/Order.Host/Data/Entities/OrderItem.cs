namespace Order.Host.Data.Entities;

public class OrderItem
{
    public int Id { get; set; }

    public string Name { get; set; }

    public DateTime Date { get; set; }

    public IEnumerable<string> Products { get; set; }

    public int Quantity { get; set; }

    public int TotalPrice { get; set; }
}