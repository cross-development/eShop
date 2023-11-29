using ClientApp.Models;
using ClientApp.DTOs.Requests;
using ClientApp.DTOs.Responses;

namespace ClientApp.Services.Interfaces;

public interface IOrderService
{
    Task<PaginatedResponseDto<OrderItem>> GetOrderItemsAsync(OrderRequestDto request);

    Task<OrderResponseDto> GetOrderItemByIdAsync(int id);

    Task<AddOrderResponseDto> AddOrderAsync(AddOrderRequestDto request);
}