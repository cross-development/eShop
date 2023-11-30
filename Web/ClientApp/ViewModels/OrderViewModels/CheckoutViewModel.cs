using ClientApp.Models;

namespace ClientApp.ViewModels.OrderViewModels;

public sealed class CheckoutViewModel
{
    public IEnumerable<BasketData> BasketItems { get; set; }
}