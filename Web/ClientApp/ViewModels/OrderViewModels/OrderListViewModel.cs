using ClientApp.Models;
using ClientApp.ViewModels.CommonViewModels;

namespace ClientApp.ViewModels.OrderViewModels;

public sealed class OrderListViewModel
{
    public IEnumerable<OrderItem> OrderItems { get; set; }

    public PaginationViewModel PaginationViewModel { get; set; }
}