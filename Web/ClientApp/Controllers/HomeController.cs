using Microsoft.AspNetCore.Mvc;
using ClientApp.DTOs.Requests;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels.HomeViewModels;
using ClientApp.ViewModels.CommonViewModels;

namespace ClientApp.Controllers;

public class HomeController : Controller
{
    private const int ItemsPerPage = 6;

    private readonly ICatalogService _catalogService;

    public HomeController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public async Task<IActionResult> Index(int? brandId, int? typeId, int? page)
    {
        // Making a query string for the request
        var request = new CatalogRequestDto
        {
            Page = page ?? 1,
            Limit = ItemsPerPage,
            BrandId = brandId,
            TypeId = typeId
        };

        // Fetching catalog items
        var catalogItems = await _catalogService.GetCatalogItems(request);

        if (catalogItems == null)
        {
            return View("Error");
        }

        // Fetching catalog brands
        var brands = await _catalogService.GetBrands();

        if (brands == null)
        {
            return View("Error");
        }

        // Fetching catalog types
        var types = await _catalogService.GetTypes();

        if (types == null)
        {
            return View("Error");
        }

        // Making a pagination view model the pagination
        var paginationViewModel = new PaginationViewModel
        {
            CurrentPage = request.Page,
            ItemsPerPage = catalogItems.Data.Count(),
            TotalItems = catalogItems.Count,
            TotalPages = (int)Math.Ceiling((decimal)catalogItems.Count / request.Limit)
        };

        // Making a catalog view model for the home view
        var catalogViewModel = new CatalogViewModel
        {
            CatalogItems = catalogItems.Data,
            Brands = brands,
            Types = types,
            PaginationViewModel = paginationViewModel
        };

        // Making the pagination prev and next buttons disabled
        var isNextPageDisabled = catalogViewModel.PaginationViewModel.CurrentPage == catalogViewModel.PaginationViewModel.TotalPages;
        var isPrevPageDisabled = catalogViewModel.PaginationViewModel.CurrentPage == 1;

        catalogViewModel.PaginationViewModel.IsNextDisabled = isNextPageDisabled;
        catalogViewModel.PaginationViewModel.IsPreviousDisabled = isPrevPageDisabled;

        return View(catalogViewModel);
    }
}
