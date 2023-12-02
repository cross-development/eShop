using Order.Host.Data.Entities;
using Order.Host.Models.Requests;

namespace Order.Host.Repositories.Interfaces;

public interface IOrderRepository
{
    Task<IEnumerable<OrderItem>> GetAllAsync(PaginatedRequest request, string userId);

    Task<OrderItem> GetByIdAsync(int id, string userId);

    Task<int> GetCountAsync();

    Task<OrderItem> AddAsync(OrderItem entity);
}