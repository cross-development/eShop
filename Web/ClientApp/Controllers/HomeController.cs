using Microsoft.AspNetCore.Mvc;
using ClientApp.Models;
using ClientApp.DTOs.Requests;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels.HomeViewModels;

namespace ClientApp.Controllers;

public sealed class HomeController : Controller
{
    private const int ItemsPerPage = 8;

    private readonly ILogger<HomeController> _logger;
    private readonly ICatalogService _catalogService;

    public HomeController(ILogger<HomeController> logger, ICatalogService catalogService)
    {
        _logger = logger;
        _catalogService = catalogService;
    }

    public async Task<IActionResult> Index(int? brandId, int? typeId, int? page)
    {
        var request = new CatalogRequestDto
        {
            Page = page ?? 1,
            Limit = ItemsPerPage,
            BrandId = brandId,
            TypeId = typeId
        };

        var catalogItems = await _catalogService.GetCatalogItemsAsync(request);

        if (catalogItems == null)
        {
            _logger.LogInformation("[HomeController: Index] ==> ERROR OCCURRED WHILE FETCHING CATALOG ITEMS\n");

            return View("Error");
        }

        var brands = await _catalogService.GetBrandsAsync();

        if (brands == null)
        {
            _logger.LogInformation("[HomeController: Index] ==> ERROR OCCURRED WHILE FETCHING CATALOG BRANDS\n");

            return View("Error");
        }

        var types = await _catalogService.GetTypesAsync();

        if (types == null)
        {
            _logger.LogInformation("[HomeController: Index] ==> ERROR OCCURRED WHILE FETCHING CATALOG TYPES\n");

            return View("Error");
        }

        var paginationViewModel = new PaginationWithFilterViewModel
        {
            BrandId = brandId,
            TypeId = typeId,
            CurrentPage = request.Page,
            ItemsPerPage = catalogItems.Data.Count(),
            TotalItems = catalogItems.Count,
            TotalPages = (int)Math.Ceiling((decimal)catalogItems.Count / request.Limit)
        };

        var catalogViewModel = new CatalogViewModel
        {
            CatalogItems = catalogItems.Data,
            Brands = brands,
            Types = types,
            PaginationViewModel = paginationViewModel
        };

        var isNextPageDisabled = catalogViewModel.PaginationViewModel.CurrentPage == catalogViewModel.PaginationViewModel.TotalPages;
        var isPrevPageDisabled = catalogViewModel.PaginationViewModel.CurrentPage == 1;

        catalogViewModel.PaginationViewModel.IsNextDisabled = isNextPageDisabled;
        catalogViewModel.PaginationViewModel.IsPreviousDisabled = isPrevPageDisabled;

        return View(catalogViewModel);
    }

    public async Task<IActionResult> Details(int id)
    {
        var catalogItem = await _catalogService.GetCatalogItemByIdAsync(id);

        if (catalogItem == null)
        {
            _logger.LogInformation("[HomeController: Details] ==> ERROR OCCURRED WHILE FETCHING CATALOG ITEM DETAILS\n");

            return View("Error");
        }

        var itemDetailsViewModel = new ItemDetailsViewModel
        {
            Item = new CatalogItem
            {
                Id = catalogItem.Id,
                Name = catalogItem.Name,
                Description = catalogItem.Description,
                Price = catalogItem.Price,
                PictureUrl = catalogItem.PictureUrl,
                AvailableStock = catalogItem.AvailableStock,
                CatalogBrand = catalogItem.CatalogBrand,
                CatalogType = catalogItem.CatalogType
            }
        };

        return View(itemDetailsViewModel);
    }
}
