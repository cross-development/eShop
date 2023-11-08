using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Infrastructure.Data.Interfaces;

namespace Infrastructure.Data;

public class ApplicationDbContextWrapper<T> : IApplicationDbContextWrapper<T>
    where T : DbContext
{
    private readonly T _applicationDbContext;

    public ApplicationDbContextWrapper(IDbContextFactory<T> dbContextFactory)
    {
        _applicationDbContext = dbContextFactory.CreateDbContext();
    }

    public T ApplicationDbContext => _applicationDbContext;

    public Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return _applicationDbContext.Database.BeginTransactionAsync(cancellationToken);
    }
}