using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Infrastructure.Exceptions;
using Basket.Host.Models.Responses;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(ComponentDefaults.DefaultRouteV1)]
public sealed class BasketBffController : ControllerBase
{
    private readonly ILogger _logger;
    private readonly IBasketService _basketService;

    public BasketBffController(ILogger<BasketBffController> logger, IBasketService basketService)
    {
        _logger = logger;
        _basketService = basketService;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(GetBasketResponse), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    public async Task<IActionResult> Items()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

        if (userId == null)
        {
            _logger.LogInformation("[BasketBffController: Items] ==> USER ID IS NULL\n");

            return NotFound(new BusinessException("Invalid user"));
        }

        var response = await _basketService.GetBasketAsync(userId);

        return Ok(response);
    }
}