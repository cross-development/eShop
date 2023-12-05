using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Infrastructure.Data.Interfaces;

namespace Infrastructure.Services;

public abstract class BaseDataService<T>
    where T : DbContext
{
    private readonly IApplicationDbContextWrapper<T> _dbContextWrapper;
    private readonly ILogger<BaseDataService<T>> _logger;

    protected BaseDataService(IApplicationDbContextWrapper<T> dbContextWrapper, ILogger<BaseDataService<T>> logger)
    {
        _dbContextWrapper = dbContextWrapper;
        _logger = logger;
    }

    protected Task ExecuteSafeAsync(Func<Task> action, CancellationToken cancellationToken = default) =>
        ExecuteSafeAsync(token => action(), cancellationToken);

    protected Task<TResult> ExecuteSafeAsync<TResult>(Func<Task<TResult>> action,
        CancellationToken cancellationToken = default) => ExecuteSafeAsync(token => action(), cancellationToken);

    private async Task ExecuteSafeAsync(Func<CancellationToken, Task> action,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[BaseDataService: ExecuteSafeAsync_WithoutResult] ==> TRANSACTION STARTING...\n");

        await using var transaction = await _dbContextWrapper.BeginTransactionAsync(cancellationToken);

        try
        {
            _logger.LogInformation("[BaseDataService: ExecuteSafeAsync_WithoutResult] ==> PERFORMING ACTION...\n");

            await action(cancellationToken);

            _logger.LogInformation("[BaseDataService: ExecuteSafeAsync_WithoutResult] ==> ACTION PERFORMED\n");

            _logger.LogInformation("[BaseDataService: ExecuteSafeAsync_WithoutResult] ==> COMMITING TRANSACTION...\n");

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("[BaseDataService: ExecuteSafeAsync_WithoutResult] ==> TRANSACTION COMMITTED\n");
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError(e, "[BaseDataService: ExecuteSafeAsync_WithoutResult] ==> " +
                                $"TRANSACTION WITH ID {transaction.TransactionId} HAS BEEN ROLLBACKED\n");
        }
    }

    private async Task<TResult> ExecuteSafeAsync<TResult>(Func<CancellationToken, Task<TResult>> action,
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation("[BaseDataService: ExecuteSafeAsync_WithResult] ==> TRANSACTION STARTING...\n");

        await using var transaction = await _dbContextWrapper.BeginTransactionAsync(cancellationToken);

        try
        {
            _logger.LogInformation("[BaseDataService: ExecuteSafeAsync_WithResult] ==> PERFORMING ACTION...\n");

            var result = await action(cancellationToken);

            _logger.LogInformation("[BaseDataService: ExecuteSafeAsync_WithResult] ==> ACTION PERFORMED\n");

            _logger.LogInformation("[BaseDataService: ExecuteSafeAsync_WithResult] ==> COMMITING TRANSACTION...\n");

            await transaction.CommitAsync(cancellationToken);

            _logger.LogInformation("[BaseDataService: ExecuteSafeAsync_WithResult] ==> TRANSACTION COMMITTED\n");

            return result;
        }
        catch (Exception e)
        {
            await transaction.RollbackAsync(cancellationToken);

            _logger.LogError(e, "[BaseDataService: ExecuteSafeAsync_WithResult] ==> " +
                                $"TRANSACTION WITH ID {transaction.TransactionId} HAS BEEN ROLLBACKED\n");
        }

        return default!;
    }
}