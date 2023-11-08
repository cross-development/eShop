using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Infrastructure.Helpers;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Controllers;

[ApiController]
[Route(ComponentDefaults.DefaultRouteV1)]
public sealed class CatalogItemController : ControllerBase
{
    private readonly ICatalogItemService _catalogItemService;

    public CatalogItemController(ICatalogItemService catalogItemService)
    {
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
            return BadRequest("Could not add the catalog item");
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
            return NotFound();
        }

        var result = await _catalogItemService.UpdateCatalogItemAsync(request, item);

        if (result == null)
        {
            return BadRequest("Could not update the catalog type");
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
            return NotFound();
        }

        var result = await _catalogItemService.DeleteCatalogItemAsync(item);

        if (!result)
        {
            return BadRequest("Could not delete the catalog item");
        }

        return NoContent();
    }
}
