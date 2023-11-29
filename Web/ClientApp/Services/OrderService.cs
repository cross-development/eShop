using Microsoft.Extensions.Options;
using ClientApp.Models;
using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;
using ClientApp.Configurations;
using ClientApp.Services.Interfaces;

namespace ClientApp.Services;

public sealed class OrderService : IOrderService
{
    private readonly IHttpClientService _httpClientService;
    private readonly ApiConfiguration _apiOptions;

    public OrderService(IHttpClientService httpClientService, IOptions<ApiConfiguration> apiOptions)
    {
        _httpClientService = httpClientService;
        _apiOptions = apiOptions.Value;
    }

    public async Task<PaginatedResponseDto<OrderItem>> GetOrderItemsAsync(OrderRequestDto request)
    {
        var result = await _httpClientService.SendAsync<PaginatedResponseDto<OrderItem>, OrderRequestDto>(
            $"{_apiOptions.OrderUrl}/order-bff/items",
            HttpMethod.Get,
            request);

        return result;
    }

    public async Task<OrderResponseDto> GetOrderItemByIdAsync(int id)
    {
        var result = await _httpClientService.SendAsync<OrderResponseDto>(
            $"{_apiOptions.OrderUrl}/order-bff/items/{id}",
            HttpMethod.Get);

        return result;
    }

    public async Task<AddOrderResponseDto> AddOrderAsync(AddOrderRequestDto request)
    {
        var result = await _httpClientService.SendAsync<AddOrderResponseDto, object, AddOrderRequestDto>(
            $"{_apiOptions.OrderUrl}/order-item/add",
            HttpMethod.Post,
            null,
            request);

        return result;
    }
}