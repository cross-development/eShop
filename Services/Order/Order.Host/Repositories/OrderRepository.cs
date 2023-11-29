using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Interfaces;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Models.Requests;
using Order.Host.Repositories.Interfaces;

namespace Order.Host.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(IApplicationDbContextWrapper<ApplicationDbContext> dbContextWrapper)
    {
        _dbContext = dbContextWrapper.ApplicationDbContext;
    }

    public async Task<IEnumerable<OrderItem>> GetAllAsync(PaginatedRequest request, string userId)
    {
        return await _dbContext.OrderItems
            .Where(item => item.UserId == userId)
            .OrderBy(item => item.Date)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .ToListAsync();
    }

    public async Task<OrderItem> GetByIdAsync(int id, string userId)
    {
        return await _dbContext.OrderItems
            .FirstOrDefaultAsync(item => item.Id == id && item.UserId == userId);
    }

    public async Task<OrderItem> AddAsync(OrderItem entity)
    {
        var item = await _dbContext.AddAsync(entity);

        await _dbContext.SaveChangesAsync();

        return item.Entity;
    }
}