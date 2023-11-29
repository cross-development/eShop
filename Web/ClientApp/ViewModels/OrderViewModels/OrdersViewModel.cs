using ClientApp.Models;
using ClientApp.ViewModels.CommonViewModels;

namespace ClientApp.ViewModels.OrderViewModels;

public sealed class OrdersViewModel
{
    public IEnumerable<OrderItem> OrderItems { get; set; }

    public PaginationViewModel PaginationViewModel { get; set; }
}