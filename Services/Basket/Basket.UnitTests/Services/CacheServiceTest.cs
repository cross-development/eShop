namespace Basket.UnitTests.Services;

public class CacheServiceTest
{
    private readonly Mock<IOptions<RedisConfiguration>> _config;
    private readonly Mock<ILogger<CacheService>> _logger;
    private readonly Mock<IRedisCacheConnectionService> _redisCacheConnectionService;
    private readonly Mock<IConnectionMultiplexer> _connectionMultiplexer;
    private readonly Mock<IDatabase> _redisDataBase;

    private readonly ICacheService _cacheService;

    public CacheServiceTest()
    {
        _config = new Mock<IOptions<RedisConfiguration>>();
        _logger = new Mock<ILogger<CacheService>>();

        _config.Setup(x => x.Value).Returns(new RedisConfiguration() { CacheTimeout = TimeSpan.Zero });

        _redisCacheConnectionService = new Mock<IRedisCacheConnectionService>();
        _connectionMultiplexer = new Mock<IConnectionMultiplexer>();
        _redisDataBase = new Mock<IDatabase>();

        _connectionMultiplexer
            .Setup(connection => connection.GetDatabase(
                It.IsAny<int>(), It.IsAny<object>())).Returns(_redisDataBase.Object);

        _redisCacheConnectionService
            .Setup(cache => cache.Connection)
            .Returns(_connectionMultiplexer.Object);

        _cacheService = new CacheService(_logger.Object, _config.Object, _redisCacheConnectionService.Object);
    }

    [Fact]
    public async Task AddOrUpdateAsync_Add_Success()
    {
        // arrange
        var testEntity = new
        {
            UserId = "TestUserId",
            Data = "data"
        };

        _redisDataBase.Setup(expression: database => database.StringSetAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<TimeSpan?>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>())).ReturnsAsync(true);

        // act
        await _cacheService.AddOrUpdateAsync(testEntity.UserId, testEntity.Data);
        await _cacheService.AddOrUpdateAsync(testEntity.UserId, testEntity.Data);

        // assert
        _logger.Verify(
            logger => logger.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) =>
                o.ToString().Contains($"The data was cached for the key: {testEntity.UserId}")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
    }

    [Fact]
    public async Task AddOrUpdateAsync_Update_Success()
    {
        // arrange
        var testEntity = new
        {
            UserId = "TestUserId",
            Data = "data"
        };

        _redisDataBase.Setup(expression: database => database.StringSetAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<TimeSpan?>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>())).ReturnsAsync(true);

        // act
        await _cacheService.AddOrUpdateAsync(testEntity.UserId, testEntity.Data);

        // assert
        _logger.Verify(
            logger => logger.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) =>
                o.ToString().Contains($"DATA WAS NOT CACHED WITH KEY: {testEntity.UserId}")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Once);
    }

    [Fact]
    public async Task AddOrUpdateAsync_Failed()
    {
        // arrange
        var testEntity = new
        {
            UserId = "TestUserId",
            Data = "data"
        };

        _redisDataBase.Setup(expression: database => database.StringSetAsync(
            It.IsAny<RedisKey>(),
            It.IsAny<RedisValue>(),
            It.IsAny<TimeSpan?>(),
            It.IsAny<When>(),
            It.IsAny<CommandFlags>())).ReturnsAsync(false);

        // act
        await _cacheService.AddOrUpdateAsync(testEntity.UserId, testEntity.Data);

        // assert
        _logger.Verify(
            logger => logger.Log(
            LogLevel.Information,
            It.IsAny<EventId>(),
            It.Is<It.IsAnyType>((o, t) =>
                o.ToString().Contains($"DATA WAS CACHED WITH KEY: {testEntity.UserId}")),
            It.IsAny<Exception>(),
            It.IsAny<Func<It.IsAnyType, Exception, string>>()), Times.Never);
    }

    [Fact]
    public async Task GetAsync_Failed()
    {
        // arrange
        var testName = "testName";

        // act
        var result = await _cacheService.GetAsync<string>(testName);

        // assert
        result.Should().BeNull();
    }
}