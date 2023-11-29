using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ClientApp.Models;
using ClientApp.DTOs.Requests;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels.BasketViewModels;

namespace ClientApp.Controllers;

[Authorize]
public sealed class BasketController : Controller
{
    private readonly IBasketService _basketService;

    public BasketController(IBasketService basketService)
    {
        _basketService = basketService;
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
}
