using Microsoft.EntityFrameworkCore;
using Infrastructure.Data.Interfaces;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Models.Requests;
using Order.Host.Repositories.Interfaces;

namespace Order.Host.Repositories;

public sealed class OrderRepository : IOrderRepository
{
    private readonly ILogger<OrderRepository> _logger;
    private readonly ApplicationDbContext _dbContext;

    public OrderRepository(
        ILogger<OrderRepository> logger,
        IApplicationDbContextWrapper<ApplicationDbContext> dbContextWrapper)
    {
        _logger = logger;
        _dbContext = dbContextWrapper.ApplicationDbContext;
    }

    public async Task<IEnumerable<OrderItem>> GetAllAsync(PaginatedRequest request, string userId)
    {
        _logger.LogInformation("[OrderRepository: GetAllAsync] ==> GETTING ORDER DATA FROM DATABASE...\n");

        return await _dbContext.OrderItems
            .Where(item => item.UserId == userId)
            .OrderBy(item => item.Date)
            .Skip((request.Page - 1) * request.Limit)
            .Take(request.Limit)
            .ToListAsync();
    }

    public async Task<OrderItem> GetByIdAsync(int id, string userId)
    {
        _logger.LogInformation("[OrderRepository: GetByIdAsync] ==> GETTING ORDER DATA BY ID FROM DATABASE...\n");

        return await _dbContext.OrderItems
            .FirstOrDefaultAsync(item => item.Id == id && item.UserId == userId);
    }

    public async Task<int> GetCountAsync()
    {
        _logger.LogInformation("[OrderRepository: GetCountAsync] ==> GETTING ORDER ITEMS COUNT FROM DATABASE...\n");

        return await _dbContext.OrderItems.CountAsync();
    }

    public async Task<OrderItem> AddAsync(OrderItem entity)
    {
        _logger.LogInformation("[OrderRepository: GetAllAsync] ==> ADDING ORDER DATA TO DATABASE...\n");

        var item = await _dbContext.AddAsync(entity);

        await _dbContext.SaveChangesAsync();

        return item.Entity;
    }
}