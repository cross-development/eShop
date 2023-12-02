using Microsoft.EntityFrameworkCore;

namespace Order.Host.Data;

public sealed class DbInitializer
{
    public static void Init(WebApplication application)
    {
        using var scope = application.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<DbInitializer>>();

        try
        {
            SeedData(context, logger).Wait();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[DbInitializer: Init] ==> An error occurred creating the DB.");
        }
    }

    private static async Task SeedData(ApplicationDbContext context, ILogger<DbInitializer> logger)
    {
        await context.Database.MigrateAsync();
    }
}