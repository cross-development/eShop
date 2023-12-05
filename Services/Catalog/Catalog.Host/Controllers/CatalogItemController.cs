using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Infrastructure.Exceptions;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope(AuthScope.CatalogApiItems)]
[Route(ComponentDefaults.DefaultRouteV1)]
public sealed class CatalogItemController : ControllerBase
{
    private readonly ILogger<CatalogItemController> _logger;
    private readonly ICatalogItemService _catalogItemService;

    public CatalogItemController(
        ILogger<CatalogItemController> logger,
        ICatalogItemService catalogItemService)
    {
        _logger = logger;
        _catalogItemService = catalogItemService;
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AddItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] AddItemRequest request)
    {
        var result = await _catalogItemService.AddCatalogItemAsync(request);

        if (result == null)
        {
            _logger.LogInformation("[CatalogItemController: Add] ==> CATALOG ITEM HAS NOT BEEN ADDED\n");

            return BadRequest(new BusinessException("Could not add the catalog item"));
        }

        return CreatedAtAction(nameof(Add), new AddItemResponse { Id = result.Id });
    }

    [HttpPatch("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AddItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateItemRequest request)
    {
        var item = await _catalogItemService.FindCatalogItemAsync(id);

        if (item == null)
        {
            _logger.LogInformation("[CatalogItemController: Update] ==> ITEM WITH PROVIDED ID NOT FOUND\n");

            return NotFound(new BusinessException("Item with provided id not found"));
        }

        var result = await _catalogItemService.UpdateCatalogItemAsync(request, item);

        if (result == null)
        {
            _logger.LogInformation("[CatalogItemController: Update] ==> CATALOG ITEM HAS NOT BEEN UPDATED\n");

            return BadRequest(new BusinessException("Could not update the catalog type"));
        }

        return Ok(new AddItemResponse { Id = result.Id });
    }

    [HttpDelete("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var item = await _catalogItemService.FindCatalogItemAsync(id);

        if (item == null)
        {
            _logger.LogInformation("[CatalogItemController: Delete] ==> ITEM WITH PROVIDED ID NOT FOUND\n");

            return NotFound(new BusinessException("Item with provided id not found"));
        }

        var result = await _catalogItemService.DeleteCatalogItemAsync(item);

        if (!result)
        {
            _logger.LogInformation("[CatalogItemController: Delete] ==> CATALOG ITEM HAS NOT BEEN DELETED\n");

            return BadRequest(new BusinessException("Could not delete the catalog item"));
        }

        return NoContent();
    }
}
