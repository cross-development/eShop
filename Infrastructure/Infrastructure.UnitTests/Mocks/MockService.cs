namespace Infrastructure.UnitTests.Mocks;

public class MockService : BaseDataService<MockApplicationDbContext>
{
    public MockService(
        IApplicationDbContextWrapper<MockApplicationDbContext> dbContextWrapper,
        ILogger<MockService> logger)
        : base(dbContextWrapper, logger)
    {
    }

    public async Task RunWithException()
    {
        await ExecuteSafeAsync(() => throw new Exception());
    }

    public async Task RunWithoutException()
    {
        await ExecuteSafeAsync(() => Task.CompletedTask);
    }
}