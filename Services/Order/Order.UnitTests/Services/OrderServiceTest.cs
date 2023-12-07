namespace Order.UnitTests.Services;

public class OrderServiceTest
{
    private readonly Mock<IApplicationDbContextWrapper<ApplicationDbContext>> _dbContextWrapper;
    private readonly Mock<IOrderRepository> _orderRepository;
    private readonly Mock<ILogger<OrderService>> _logger;
    private readonly Mock<IMapper> _mapper;

    private readonly IOrderService _orderService;

    private readonly string _userId = "test-user-id";

    public OrderServiceTest()
    {
        _dbContextWrapper = new Mock<IApplicationDbContextWrapper<ApplicationDbContext>>();
        _orderRepository = new Mock<IOrderRepository>();
        _logger = new Mock<ILogger<OrderService>>();
        _mapper = new Mock<IMapper>();

        var dbContextTransaction = new Mock<IDbContextTransaction>();
        _dbContextWrapper
            .Setup(context => context.BeginTransactionAsync(CancellationToken.None))
            .ReturnsAsync(dbContextTransaction.Object);

        _orderService = new OrderService(_dbContextWrapper.Object, _logger.Object, _orderRepository.Object, _mapper.Object);
    }

    [Fact]
    public async Task GetOrderItemsAsync_Success()
    {
        // arrange
        var testTotalCount = 1;
        var date = DateTime.Now;

        var orderItem = new OrderItem
        {
            Id = 1,
            Date = date,
            Name = "Test Name",
            UserId = _userId,
            Quantity = 1,
            TotalPrice = 10,
            Products = "Test product"
        };

        var orderItemDto = new OrderItemDto
        {
            Id = 1,
            Date = date,
            Name = "Test Name",
            Quantity = 1,
            TotalPrice = 10,
            Products = "Test product"
        };

        var orderItemsList = new List<OrderItem> { orderItem };

        var paginatedRequest = new PaginatedRequest
        {
            Limit = 10,
            Page = 1,
        };

        _orderRepository.Setup(repository => repository.GetAllAsync(
            It.Is<PaginatedRequest>(request => request.Equals(paginatedRequest)),
            It.Is<string>(item => item.Equals(_userId)))
        ).ReturnsAsync(orderItemsList);

        _orderRepository.Setup(repository => repository.GetCountAsync()
        ).ReturnsAsync(testTotalCount);

        _mapper.Setup(mapper => mapper.Map<OrderItemDto>(
            It.Is<OrderItem>(order => order.Equals(orderItem)))
        ).Returns(orderItemDto);

        // act
        var result = await _orderService.GetOrderItemsAsync(paginatedRequest, _userId);

        // assert
        result.Should().NotBeNull();
        result?.Data.Should().NotBeNull();
        result?.Data.Count().Should().Be(testTotalCount);
        result?.Count.Should().Be(testTotalCount);
        result?.Page.Should().Be(paginatedRequest.Page);
        result?.Limit.Should().Be(paginatedRequest.Limit);
    }

    [Fact]
    public async Task GetOrderItemsAsync_Failed()
    {
        // arrange
        var paginatedRequest = new PaginatedRequest
        {
            Limit = 10,
            Page = 1,
        };

        _orderRepository.Setup(repository => repository.GetAllAsync(
            It.Is<PaginatedRequest>(request => request.Equals(paginatedRequest)),
            It.Is<string>(item => item.Equals(_userId)))
        ).Returns((Func<PaginatedResponse<OrderItemDto>>)null!);

        // act
        var result = await _orderService.GetOrderItemsAsync(paginatedRequest, _userId);

        // assert
        result.Should().BeNull();
    }

    [Fact]
    public async Task GetOrderItemByIdAsync_Success()
    {
        // arrange
        var date = DateTime.Now;

        var orderItem = new OrderItem
        {
            Id = 1,
            Date = date,
            Name = "Test Name",
            UserId = _userId,
            Quantity = 1,
            TotalPrice = 10,
            Products = "Test product"
        };

        var orderItemDto = new OrderItemDto
        {
            Id = 1,
            Date = date,
            Name = "Test Name",
            Quantity = 1,
            TotalPrice = 10,
            Products = "Test product"
        };

        _orderRepository.Setup(repository => repository.GetByIdAsync(
            It.Is<int>(itemId => itemId == orderItem.Id),
            It.Is<string>(item => item.Equals(_userId)))
        ).ReturnsAsync(orderItem);

        _mapper.Setup(mapper => mapper.Map<OrderItemDto>(
            It.Is<OrderItem>(item => item.Equals(orderItem)))
        ).Returns(orderItemDto);

        // act
        var result = await _orderService.GetOrderItemByIdAsync(orderItem.Id, _userId);

        // assert
        result.Should().NotBeNull();
        result?.Id.Should().Be(orderItem.Id);
        result?.Name.Should().Be(orderItem.Name);
        result?.Date.Should().Be(orderItem.Date);
        result?.Quantity.Should().Be(orderItem.Quantity);
        result?.TotalPrice.Should().Be(orderItem.TotalPrice);
        result?.Products.Should().Be(orderItem.Products);
    }

    [Fact]
    public async Task GetOrderItemByIdAsync_Failed()
    {
        // arrange
        var testItemId = 1;

        _orderRepository.Setup(repository => repository.GetByIdAsync(
            It.Is<int>(itemId => itemId == testItemId),
            It.Is<string>(item => item.Equals(_userId)))
        ).ReturnsAsync((Func<OrderItem>)null!);

        // act
        var result = await _orderService.GetOrderItemByIdAsync(testItemId, _userId);

        // result
        result.Should().BeNull();
    }

    [Fact]
    public async Task AddOrderAsync_Success()
    {
        // arrange
        var date = DateTime.Now;

        var addOrderRequest = new AddOrderRequest
        {
            Name = "Test Name",
            Products = "Test product",
            Date = date,
            Quantity = 1,
            TotalPrice = 10,
        };

        var orderItem = new OrderItem
        {
            Id = 1,
            Date = date,
            Name = "Test Name",
            UserId = _userId,
            Quantity = 1,
            TotalPrice = 10,
            Products = "Test product"
        };

        _orderRepository.Setup(repository => repository.AddAsync(
            It.Is<OrderItem>(item => item.Equals(orderItem)))
        ).ReturnsAsync(orderItem);

        _mapper.Setup(mapper => mapper.Map<OrderItem>(
            It.Is<AddOrderRequest>(request => request.Equals(addOrderRequest)))
        ).Returns(orderItem);

        // act
        var result = await _orderService.AddOrderAsync(addOrderRequest, _userId);

        // assert
        result.Should().Be(orderItem);
    }

    [Fact]
    public async Task AddOrderAsync_Failed()
    {
        // arrange
        OrderItem? testResult = null;

        var addOrderRequest = new AddOrderRequest
        {
            Name = "Test Name",
            Products = "Test product",
            Date = DateTime.Now,
            Quantity = 1,
            TotalPrice = 10,
        };

        _orderRepository.Setup(repository => repository.AddAsync(
            It.Is<OrderItem>(item => item.Equals(addOrderRequest)))
        ).ReturnsAsync(testResult);

        // act
        var result = await _orderService.AddOrderAsync(addOrderRequest, _userId);

        // assert
        result.Should().Be(testResult);
    }
}