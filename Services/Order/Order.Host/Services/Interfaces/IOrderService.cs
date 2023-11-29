using Order.Host.Data.Entities;
using Order.Host.Models.DTOs;
using Order.Host.Models.Requests;
using Order.Host.Models.Responses;

namespace Order.Host.Services.Interfaces;

public interface IOrderService
{
    Task<PaginatedResponse<OrderItemDto>> GetOrderItemsAsync(PaginatedRequest request, string userId);

    Task<OrderItemDto> GetOrderItemByIdAsync(int id, string userId);

    Task<OrderItem> AddOrderAsync(AddOrderRequest request);
}