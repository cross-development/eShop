using AutoMapper;
using Infrastructure.Data.Interfaces;
using Infrastructure.Services;
using Order.Host.Data;
using Order.Host.Data.Entities;
using Order.Host.Models.DTOs;
using Order.Host.Models.Requests;
using Order.Host.Models.Responses;
using Order.Host.Repositories.Interfaces;
using Order.Host.Services.Interfaces;

namespace Order.Host.Services;

public class OrderService : BaseDataService<ApplicationDbContext>, IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IMapper _mapper;

    public OrderService(
        IApplicationDbContextWrapper<ApplicationDbContext> dbContextWrapper,
        ILogger<BaseDataService<ApplicationDbContext>> logger,
        IOrderRepository orderRepository,
        IMapper mapper)
        : base(dbContextWrapper, logger)
    {
        _orderRepository = orderRepository;
        _mapper = mapper;
    }

    public async Task<PaginatedResponse<OrderItemDto>> GetOrderItemsAsync(PaginatedRequest request, string userId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var result = await _orderRepository.GetAllAsync(request, userId);

            return new PaginatedResponse<OrderItemDto>
            {
                Data = result.Select(item => _mapper.Map<OrderItemDto>(item))
            };
        });
    }

    public async Task<OrderItemDto> GetOrderItemByIdAsync(int id, string userId)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var item = await _orderRepository.GetByIdAsync(id, userId);

            return _mapper.Map<OrderItemDto>(item);
        });
    }

    public async Task<OrderItem> AddOrderAsync(AddOrderRequest request)
    {
        return await ExecuteSafeAsync(async () =>
        {
            var catalogBrand = _mapper.Map<OrderItem>(request);

            return await _orderRepository.AddAsync(catalogBrand);
        });
    }
}
