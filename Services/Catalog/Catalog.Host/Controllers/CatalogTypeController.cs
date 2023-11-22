using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Catalog.Host.Models.Requests;
using Catalog.Host.Models.Responses;
using Catalog.Host.Services.Interfaces;

namespace Catalog.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope(AuthScope.CatalogApi)]
[Route(ComponentDefaults.DefaultRouteV1)]
public sealed class CatalogTypeController : ControllerBase
{
    private readonly ICatalogTypeService _catalogTypeService;

    public CatalogTypeController(ICatalogTypeService catalogTypeService)
    {
        _catalogTypeService = catalogTypeService;
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AddItemResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Add([FromBody] AddTypeRequest request)
    {
        var result = await _catalogTypeService.AddCatalogTypeAsync(request);

        if (result == null)
        {
            return BadRequest("Could not add the catalog type");
        }

        return CreatedAtAction(nameof(Add), new AddItemResponse { Id = result.Id });
    }

    [HttpPatch("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AddItemResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateTypeRequest request)
    {
        var item = await _catalogTypeService.FindCatalogTypeAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        var result = await _catalogTypeService.UpdateCatalogTypeAsync(request, item);

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
        var item = await _catalogTypeService.FindCatalogTypeAsync(id);

        if (item == null)
        {
            return NotFound();
        }

        var result = await _catalogTypeService.DeleteCatalogTypeAsync(item);

        if (!result)
        {
            return BadRequest("Could not delete the catalog type");
        }

        return NoContent();
    }
}
