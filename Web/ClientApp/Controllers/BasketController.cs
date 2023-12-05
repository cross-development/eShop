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
    private readonly ILogger<BasketController> _logger;
    private readonly IBasketService _basketService;
    private readonly IOrderService _orderService;

    public BasketController(
        ILogger<BasketController> logger,
        IBasketService basketService,
        IOrderService orderService)
    {
        _logger = logger;
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

    [HttpPost]
    public async Task<IActionResult> AddToBasket(CatalogItem catalogItem)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("[BasketController: AddToBasket] ==> MODEL STATE IS NOT VALID\n");

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
            _logger.LogInformation("[BasketController: AddToBasket] ==> ITEM HAS NOT BEEN ADDED\n");

            return View("Error");
        }

        return RedirectToAction("Index", "Home");
    }

    [HttpPost]
    public async Task<IActionResult> DeleteFromBasket(int id)
    {
        _logger.LogInformation($"[BasketController: DeleteFromBasket] ==> PROVIDED ID {id}\n");

        if (!ModelState.IsValid)
        {
            _logger.LogInformation("[BasketController: DeleteFromBasket] ==> MODEL STATE IS NOT VALID\n");

            return View("Error");
        }

        var result = await _basketService.DeleteFromBasketByIdAsync(id);

        if (!result)
        {
            _logger.LogInformation("[BasketController: DeleteFromBasket] ==> ITEM HAS NOT BEEN DELETED\n");

            return View("Error");
        }

        return RedirectToAction("Index", "Basket");
    }

    [HttpPost]
    public async Task<IActionResult> Checkout(BasketItemsViewModel basketItemsViewModel)
    {
        if (!ModelState.IsValid)
        {
            _logger.LogInformation("[BasketController: Checkout] ==> MODEL STATE IS NOT VALID\n");

            return View("Error");
        }

        var addOrderRequestDto = new AddOrderRequestDto
        {
            Name = $"Order #{DateTimeOffset.Now.ToString().ToSha256().Substring(0, 10)}",
            Date = DateTimeOffset.Now,
            Quantity = (uint)basketItemsViewModel.BasketItems.Sum(data => data.Amount),
            TotalPrice = basketItemsViewModel.BasketItems.Sum(data => data.Price * data.Amount),
            Products = string.Join(", ", basketItemsViewModel.BasketItems.Select(data => data.Name)),
        };

        var result = await _orderService.AddOrderAsync(addOrderRequestDto);

        if (result == null)
        {
            _logger.LogInformation("[BasketController: Checkout] ==> ORDER HAS NOT BEEN ADDED\n");

            return View("Error");
        }

        var isDeleted = await _basketService.DeleteAllFromBasketAsync();

        if (!isDeleted)
        {
            _logger.LogInformation("[BasketController: Checkout] ==> BASKET HAS NOT BEEN CLEARED\n");

            return View("Error");
        }

        return View();
    }
}
