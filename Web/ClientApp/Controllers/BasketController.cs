using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using IdentityModel;
using ClientApp.Models;
using ClientApp.DTOs.Requests;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels.BasketViewModels;

namespace ClientApp.Controllers;

[Authorize]
public sealed class BasketController : Controller
{
    private readonly IBasketService _basketService;
    private readonly IOrderService _orderService;

    public BasketController(IBasketService basketService, IOrderService orderService)
    {
        _basketService = basketService;
        _orderService = orderService;
    }

    public async Task<IActionResult> Index()
    {
        var result = await _basketService.GetBasketAsync();

        var basketItemsViewModel = new BasketItemsViewModel
        {
            BasketItems = result?.Data
        };

        return View(basketItemsViewModel);
    }

    public async Task<IActionResult> AddToBasket(CatalogItem catalogItem)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine($"[BasketController : AddToBasket] ====> MODEL STATE IS NOT VALID");

            return View("Error");
        }

        var basketDto = new BasketRequestDto
        {
            Data = new BasketData
            {
                Id = catalogItem.Id,
                Name = catalogItem.Name,
                Price = catalogItem.Price,
                PictureUrl = catalogItem.PictureUrl,
                Amount = 1
            }
        };

        var result = await _basketService.AddToBasketAsync(basketDto);

        if (!result)
        {
            Console.WriteLine($"[BasketController : AddToBasket] ====> ITEM HAS NOT BEEN ADDED");

            return View("Error");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFromBasket(int id)
    {
        Console.WriteLine($"[BasketController: DeleteFromBasket] =====> ID: {id}");

        if (!ModelState.IsValid)
        {
            Console.WriteLine($"[BasketController : DeleteFromBasket] ====> MODEL STATE IS NOT VALID");

            return View("Error");
        }

        var result = await _basketService.DeleteFromBasketByIdAsync(id);

        if (!result)
        {
            Console.WriteLine($"[BasketController : DeleteFromBasket] ====> ITEM HAS NOT BEEN DELETED");

            return View("Error");
        }

        return RedirectToAction("Index", "Basket");
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(BasketItemsViewModel basketItemsViewModel)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine($"[BasketController : Checkout] ====> MODEL STATE IS NOT VALID");

            return View("Error");
        }

        var products = basketItemsViewModel.BasketItems.Select(item => item.Name);

        var addOrderRequestDto = new AddOrderRequestDto
        {
            Name = $"Order #{DateTimeOffset.Now.ToString().ToSha256().Substring(0, 10)}",
            Date = DateTimeOffset.Now,
            Quantity = (uint)basketItemsViewModel.BasketItems.Sum(data => data.Amount),
            TotalPrice = basketItemsViewModel.BasketItems.Sum(data => data.Price * data.Amount),
            Products = string.Join(", ", products),
        };

        var result = await _orderService.AddOrderAsync(addOrderRequestDto);

        if (result == null)
        {
            Console.WriteLine($"[BasketController : Checkout] ====> ORDER HAS NOT BEEN ADDED");

            return View("Error");
        }

        var isDeleted = await _basketService.DeleteAllFromBasketAsync();

        if (!isDeleted)
        {
            Console.WriteLine($"[BasketController : Checkout] ====> BASKET HAS NOT BEEN CLEARED");

            return View("Error");
        }

        return View();
    }
}
