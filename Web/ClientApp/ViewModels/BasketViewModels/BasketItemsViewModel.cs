using ClientApp.Models;

namespace ClientApp.ViewModels.BasketViewModels;

public sealed class BasketItemsViewModel
{
    public IEnumerable<BasketData> BasketItems { get; set; }
}