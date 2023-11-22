namespace Infrastructure.UnitTests.Services;

public class BaseDataServiceTest
{
    private readonly Mock<IDbContextTransaction> _dbContextTransaction;
    private readonly Mock<ILogger<MockService>> _logger;
    private readonly MockService _mockService;

    public BaseDataServiceTest()
    {
        var dbContextWrapper = new Mock<IApplicationDbContextWrapper<MockApplicationDbContext>>();
        _dbContextTransaction = new Mock<IDbContextTransaction>();
        _logger = new Mock<ILogger<MockService>>();

        dbContextWrapper.Setup(wrapper => wrapper.BeginTransactionAsync(CancellationToken.None))
            .ReturnsAsync(_dbContextTransaction.Object);

        _mockService = new MockService(dbContextWrapper.Object, _logger.Object);
    }

    [Fact]
    public async Task ExecuteSafe_Success()
    {
        // act
        await _mockService.RunWithoutException();

        // assert
        _dbContextTransaction.Verify(transaction => transaction.CommitAsync(CancellationToken.None), Times.Once);
        _dbContextTransaction.Verify(transaction => transaction.RollbackAsync(CancellationToken.None), Times.Never);
    }

    [Fact]
    public async Task ExecuteSafe_Failed()
    {
        // act
        await _mockService.RunWithException();

        // assert
        _dbContextTransaction.Verify(transaction => transaction.CommitAsync(CancellationToken.None), Times.Never);
        _dbContextTransaction.Verify(transaction => transaction.RollbackAsync(CancellationToken.None), Times.Once);
    }
}