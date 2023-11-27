using System.Net.Mime;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Infrastructure.Helpers;
using Infrastructure.Identity;
using Basket.Host.Models.Responses;
using Basket.Host.Services.Interfaces;

namespace Basket.Host.Controllers;

[ApiController]
[Authorize(Policy = AuthPolicy.AllowEndUserPolicy)]
[Route(ComponentDefaults.DefaultRouteV1)]
public class BasketBffController : ControllerBase
{
    private readonly IBasketService _basketService;
    private readonly ILogger<BasketBffController> _logger;

    public BasketBffController(IBasketService basketService, ILogger<BasketBffController> logger)
    {
        _basketService = basketService;
        _logger = logger;
    }

    [HttpGet]
    [Produces(MediaTypeNames.Application.Json)]
    [ProducesResponseType(typeof(GetBasketResponse), StatusCodes.Status200OK)]
    public async Task<IActionResult> Items()
    {
        var userId = User.Claims.FirstOrDefault(claim => claim.Type == "sub")?.Value;

        _logger.LogInformation($"[BasketBffController: Items] ==> USER ID: {userId}");

        var response = await _basketService.GetBasketAsync(userId);

        return Ok(response);
    }
}