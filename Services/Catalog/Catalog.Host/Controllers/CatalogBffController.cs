﻿using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Infrastructure.Exceptions;
using Catalog.Host.Models.DTOs;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(ComponentDefaults.DefaultRouteV1)]
public sealed class CatalogBffController : ControllerBase
{
    private readonly ILogger<CatalogBffController> _logger;
    private readonly ICatalogItemService _catalogItemService;
    private readonly ICatalogBrandService _catalogBrandService;
    private readonly ICatalogTypeService _catalogTypeService;

    public CatalogBffController(
        ILogger<CatalogBffController> logger,
        ICatalogItemService catalogItemService,
        ICatalogBrandService catalogBrandService,
        ICatalogTypeService catalogTypeService)
    {
        _logger = logger;
        _catalogItemService = catalogItemService;
        _catalogBrandService = catalogBrandService;
        _catalogTypeService = catalogTypeService;
    }

    [HttpGet]
    [AllowAnonymous]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PaginatedResponse<CatalogItemDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Items([FromQuery] PaginatedItemRequest request)
    {
        var result = await _catalogItemService.GetCatalogItemsAsync(request);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(CatalogItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Items([FromRoute] int id)
    {
        var result = await _catalogItemService.GetCatalogItemByIdAsync(id);

        if (result == null)
        {
            _logger.LogInformation("[CatalogBffController: Items_Id] ==> ITEM WITH PROVIDED ID NOT FOUND\n");

            return NotFound(new BusinessException("Item with provided id not found"));
        }

        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PaginatedResponse<CatalogBrandDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Brands()
    {
        var result = await _catalogBrandService.GetCatalogBrandsAsync();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(CatalogBrandDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Brands([FromRoute] int id)
    {
        var result = await _catalogBrandService.GetCatalogBrandByIdAsync(id);

        if (result == null)
        {
            _logger.LogInformation("[CatalogBffController: Brands_Id] ==> BRAND WITH PROVIDED ID NOT FOUND\n");

            return NotFound(new BusinessException("Item with provided id not found"));
        }

        return Ok(result);
    }

    [HttpGet]
    [AllowAnonymous]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PaginatedResponse<CatalogTypeDto>), StatusCodes.Status200OK)]
    public async Task<IActionResult> Types()
    {
        var result = await _catalogTypeService.GetCatalogTypesAsync();

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [AllowAnonymous]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(CatalogTypeDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Types([FromRoute] int id)
    {
        var result = await _catalogTypeService.GetCatalogTypeByIdAsync(id);

        if (result == null)
        {
            _logger.LogInformation("[CatalogBffController: Types_Id] ==> TYPE WITH PROVIDED ID NOT FOUND\n");

            return NotFound(new BusinessException("Item with provided id not found"));
        }

        return Ok(result);
    }
}