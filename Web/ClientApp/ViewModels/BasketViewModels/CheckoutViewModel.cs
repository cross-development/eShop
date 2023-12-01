namespace ClientApp.ViewModels.BasketViewModels;

public sealed class CheckoutViewModel
{
    public string Name { get; set; }

    public DateTimeOffset Date { get; set; }

    public string Products { get; set; }

    public int Quantity { get; set; }

    public int TotalPrice { get; set; }
}