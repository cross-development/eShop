using System.Net.Mime;
using Infrastructure.Exceptions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Order.Host.Models.Requests;
using Order.Host.Models.Responses;
using Order.Host.Services.Interfaces;

namespace Order.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowClientPolicy)]
[Scope(AuthScope.OrderApi)]
[Route(ComponentDefaults.DefaultRouteV1)]
public sealed class OrderItemController : ControllerBase
{
    private readonly ILogger<OrderItemController> _logger;
    private readonly IOrderService _orderService;

    public OrderItemController(ILogger<OrderItemController> logger, IOrderService orderService)
    {
        _logger = logger;
        _orderService = orderService;
    }

    [HttpPost]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(AddOrderResponse), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    [ProducesResponseType(StatusCodes.Status503ServiceUnavailable)]
    public async Task<IActionResult> Add([FromBody] AddOrderRequest request)
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

        if (userId == null)
        {
            _logger.LogInformation("[OrderItemController: Add] ==> USER ID IS NULL\n");

            return BadRequest(new BusinessException("Invalid user"));
        }

        var result = await _orderService.AddOrderAsync(request, userId);

        if (result == null)
        {
            _logger.LogInformation("[OrderItemController: Add] ==> ORDER HAS NOT BEEN ADDED\n");

            return BadRequest(new BusinessException("Could not add the order item"));
        }

        return CreatedAtAction(nameof(Add), new AddOrderResponse { Id = result.Id });
    }
}
