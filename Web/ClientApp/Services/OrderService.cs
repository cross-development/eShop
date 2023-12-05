using Microsoft.Extensions.Options;
using ClientApp.Models;
using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;
using ClientApp.Configurations;
using ClientApp.Services.Interfaces;

namespace ClientApp.Services;

public sealed class OrderService : IOrderService
{
    private readonly ILogger<OrderService> _logger;
    private readonly IHttpClientService _httpClientService;
    private readonly ApiConfiguration _apiOptions;

    public OrderService(
        ILogger<OrderService> logger,
        IHttpClientService httpClientService,
        IOptions<ApiConfiguration> apiOptions)
    {
        _logger = logger;
        _httpClientService = httpClientService;
        _apiOptions = apiOptions.Value;
    }

    public async Task<PaginatedResponseDto<OrderItem>> GetOrderItemsAsync(OrderRequestDto request)
    {
        _logger.LogInformation("[OrderService: GetOrderItemsAsync] ==> FETCHING ORDER ITEMS DATA...\n");

        var result = await _httpClientService.SendAsync<PaginatedResponseDto<OrderItem>, OrderRequestDto>(
            $"{_apiOptions.OrderUrl}/order-bff/items",
            HttpMethod.Get,
            request);

        return result;
    }

    public async Task<OrderResponseDto> GetOrderItemByIdAsync(int id)
    {
        _logger.LogInformation("[OrderService: GetOrderItemByIdAsync] ==> FETCHING ORDER ITEM DATA BY ID...\n");

        var result = await _httpClientService.SendAsync<OrderResponseDto>(
            $"{_apiOptions.OrderUrl}/order-bff/items/{id}",
            HttpMethod.Get);

        return result;
    }

    public async Task<AddOrderResponseDto> AddOrderAsync(AddOrderRequestDto request)
    {
        _logger.LogInformation("[OrderService: AddOrderAsync] ==> ADDING ORDER ITEM DATA...\n");

        var result = await _httpClientService.SendAsync<AddOrderResponseDto, object, AddOrderRequestDto>(
            $"{_apiOptions.OrderUrl}/order-item/add",
            HttpMethod.Post,
            null,
            request);

        return result;
    }
}