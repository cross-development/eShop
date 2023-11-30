using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Infrastructure.Exceptions;
using Order.Host.Models.DTOs;
using Order.Host.Models.Requests;
using Order.Host.Models.Responses;
using Order.Host.Services.Interfaces;

namespace Order.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(ComponentDefaults.DefaultRouteV1)]
public sealed class OrderBffController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrderBffController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(PaginatedResponse<OrderItemDto>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<IActionResult> Items([FromQuery] PaginatedRequest request)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

        if (userId == null)
        {
            return BadRequest(new BusinessException("Invalid user"));
        }

        var result = await _orderService.GetOrderItemsAsync(request, userId);

        return Ok(result);
    }

    [HttpGet("{id:int}")]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(OrderItemDto), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Items([FromRoute] int id)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

        if (userId == null)
        {
            return BadRequest(new BusinessException("Invalid user"));
        }

        var result = await _orderService.GetOrderItemByIdAsync(id, userId);

        if (result == null)
        {
            return NotFound("Item with provided id not found");
        }

        return Ok(result);
    }
}
