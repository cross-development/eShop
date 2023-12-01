using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClientApp.Models;
using ClientApp.DTOs.Requests;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels.BasketViewModels;
using IdentityModel;

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
            return View("Error");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFromBasket(int id)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine($"[BasketController : DeleteFromBasket] ====> MODEL STATE IS NOT VALID");

            return View("Error");
        }

        Console.WriteLine($"[BasketController: DeleteFromBasket] =====> ID: {id}");

        var result = await _basketService.DeleteFromBasketAsync(id);

        if (!result)
        {
            return View("Error");
        }

        return RedirectToAction("Index", "Basket");
    }

    public async Task<IActionResult> Checkout(BasketItemsViewModel basketItemsViewModel)
    {
        if (!ModelState.IsValid)
        {
            Console.WriteLine($"[BasketController : Checkout] ====> MODEL STATE IS NOT VALID");

            return View("Error");
        }

        var addOrderRequestDto = new AddOrderRequestDto
        {
            Name = $"Order #{DateTimeOffset.Now.ToString().ToSha256()}",
            Date = DateTimeOffset.Now,
            Quantity = (uint)basketItemsViewModel.BasketItems.Count(),
            TotalPrice = basketItemsViewModel.BasketItems.Sum(item => item.Price),
            Products = ""
        };

        //var result = await _orderService.AddOrderAsync(addOrderRequestDto);

        //if (result == null)
        //{
        //    Console.WriteLine($"[BasketController : Checkout] ====> RESULT IS NULL");

        //    return View("Error");
        //}

        return View();
    }
}
