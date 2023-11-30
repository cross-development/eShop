using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Infrastructure.Exceptions;
using Basket.Host.Models.Requests;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope(AuthScope.BasketApi)]
[Route(ComponentDefaults.DefaultRouteV1)]
public sealed class BasketItemController : ControllerBase
{
    private readonly IBasketService _basketService;

    public BasketItemController(IBasketService basketService)
    {
        _basketService = basketService;
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Add([FromBody] AddItemRequest request)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

        if (userId == null)
        {
            return NotFound(new BusinessException("Invalid user"));
        }

        var hasAdded = await _basketService.AddItemAsync(userId, request.Data);

        if (!hasAdded)
        {
            return BadRequest(new BusinessException("Something went wrong while adding the item to the basket"));
        }

        return CreatedAtAction(nameof(Add), hasAdded);
    }

    [HttpDelete("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

        if (userId == null)
        {
            return BadRequest(new BusinessException("Invalid user"));
        }

        var hasDeleted = await _basketService.DeleteItemAsync(userId, id);

        if (!hasDeleted)
        {
            return NotFound(new BusinessException("Item with provided id not found"));
        }

        return Ok(hasDeleted);
    }
}