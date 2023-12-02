using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using ClientApp.Models;
using ClientApp.DTOs.Requests;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels.CommonViewModels;
using ClientApp.ViewModels.OrderViewModels;

namespace ClientApp.Controllers;

[Authorize]
public sealed class OrderController : Controller
{
    private const int ItemsPerPage = 5;

    private readonly IOrderService _orderService;

    public OrderController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    public async Task<IActionResult> Index(int? page)
    {
        // Making a query for the request
        var request = new OrderRequestDto
        {
            Page = page ?? 1,
            Limit = ItemsPerPage
        };

        // Fetching order items
        var orderItems = await _orderService.GetOrderItemsAsync(request);

        if (orderItems == null)
        {
            return View("Error");
        }

        // Making a pagination view model for the pagination view
        var paginationViewModel = new PaginationViewModel
        {
            CurrentPage = request.Page,
            ItemsPerPage = orderItems.Data.Count(),
            TotalItems = orderItems.Count,
            TotalPages = (int)Math.Ceiling((decimal)orderItems.Count / request.Limit)
        };

        // Making an order view model for the orders view
        var ordersViewModel = new OrderListViewModel
        {
            OrderItems = orderItems.Data,
            PaginationViewModel = paginationViewModel
        };

        // Making the pagination prev and next buttons disabled
        var isNextPageDisabled = ordersViewModel.PaginationViewModel.CurrentPage == ordersViewModel.PaginationViewModel.TotalPages;
        var isPrevPageDisabled = ordersViewModel.PaginationViewModel.CurrentPage == 1;

        ordersViewModel.PaginationViewModel.IsNextDisabled = isNextPageDisabled;
        ordersViewModel.PaginationViewModel.IsPreviousDisabled = isPrevPageDisabled;

        return View(ordersViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var orderItem = await _orderService.GetOrderItemByIdAsync(id);

        if (orderItem == null)
        {
            return View("Error");
        }

        var orderDetailsViewModel = new OrderDetailsViewModel
        {
            Item = new OrderItem
            {
                Id = orderItem.Id,
                Name = orderItem.Name,
                Date = orderItem.Date,
                Quantity = orderItem.Quantity,
                TotalPrice = orderItem.TotalPrice,
                Products = orderItem.Products,
            }
        };

        return View(orderDetailsViewModel);
    }
}
