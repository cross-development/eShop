﻿using Microsoft.AspNetCore.Mvc;
using ClientApp.Models;
using ClientApp.DTOs.Requests;
using ClientApp.Services.Interfaces;
using ClientApp.ViewModels.HomeViewModels;

namespace ClientApp.Controllers;

public sealed class HomeController : Controller
{
    private const int ItemsPerPage = 8;

    private readonly ICatalogService _catalogService;

    public HomeController(ICatalogService catalogService)
    {
        _catalogService = catalogService;
    }

    public async Task<IActionResult> Index(int? brandId, int? typeId, int? page)
    {
        // Making a query for the request
        var request = new CatalogRequestDto
        {
            Page = page ?? 1,
            Limit = ItemsPerPage,
            BrandId = brandId,
            TypeId = typeId
        };

        // Fetching catalog items
        var catalogItems = await _catalogService.GetCatalogItemsAsync(request);

        if (catalogItems == null)
        {
            return View("Error");
        }

        // Fetching catalog brands
        var brands = await _catalogService.GetBrandsAsync();

        if (brands == null)
        {
            return View("Error");
        }

        // Fetching catalog types
        var types = await _catalogService.GetTypesAsync();

        if (types == null)
        {
            return View("Error");
        }

        // Making a pagination view model for the pagination view
        var paginationViewModel = new PaginationWithFilterViewModel
        {
            BrandId = brandId,
            TypeId = typeId,
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

    public async Task<IActionResult> Details(int id)
    {
        var catalogItem = await _catalogService.GetCatalogItemByIdAsync(id);

        if (catalogItem == null)
        {
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
