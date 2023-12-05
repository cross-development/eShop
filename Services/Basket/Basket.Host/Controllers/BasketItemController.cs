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
    private readonly ILogger<BasketItemController> _logger;
    private readonly IBasketService _basketService;

    public BasketItemController(ILogger<BasketItemController> logger, IBasketService basketService)
    {
        _logger = logger;
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
            _logger.LogInformation("[BasketItemController: Add] ==> USER ID IS NULL\n");

            return NotFound(new BusinessException("Invalid user"));
        }

        var hasAdded = await _basketService.AddItemAsync(userId, request.Data);

        if (!hasAdded)
        {
            _logger.LogInformation("[BasketItemController: Add] ==> BASKET ITEM HAS NOT BEEN ADDED\n");

            return BadRequest(new BusinessException("Something went wrong while adding the item to the basket"));
        }

        return CreatedAtAction(nameof(Add), hasAdded);
    }

    [HttpDelete]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(bool), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Delete()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

        if (userId == null)
        {
            _logger.LogInformation("[BasketItemController: Delete] ==> USER ID IS NULL\n");

            return NotFound(new BusinessException("Invalid user"));
        }

        var hasDeleted = await _basketService.DeleteAllAsync(userId);

        if (!hasDeleted)
        {
            _logger.LogInformation("[BasketItemController: Delete] ==> BASKET ITEM HAS NOT BEEN DELETED\n");

            return NotFound(new BusinessException("Something went wrong while deleting all items from the basket"));
        }

        return Ok(hasDeleted);
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
            _logger.LogInformation("[BasketItemController: Delete_Id] ==> USER ID IS NULL\n");

            return BadRequest(new BusinessException("Invalid user"));
        }

        var hasDeleted = await _basketService.DeleteItemAsync(userId, id);

        if (!hasDeleted)
        {
            _logger.LogInformation("[BasketItemController: Delete_Id] ==> BASKET ITEM HAS NOT BEEN DELETED\n");

            return NotFound(new BusinessException("Item with provided id not found"));
        }

        return Ok(hasDeleted);
    }
}