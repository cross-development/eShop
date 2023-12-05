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
            logger.LogInformation("[DbInitializer: Init] ==> SEEDING DATABASE...\n");

            SeedData(context, logger).Wait();

            logger.LogInformation("[DbInitializer: Init] ==> DONE SEEDING DATABASE. EXITING.\n");
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "[DbInitializer: Init] ==> AN ERROR OCCURRED WHILE CREATING DATABASE\n");
        }
    }

    private static async Task SeedData(ApplicationDbContext context, ILogger<DbInitializer> logger)
    {
        await context.Database.MigrateAsync();
    }
}