using Microsoft.EntityFrameworkCore;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data;

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
            logger.LogError(ex, "An error occurred creating the DB.");
        }
    }

    private static async Task SeedData(ApplicationDbContext context, ILogger<DbInitializer> logger)
    {
        await context.Database.MigrateAsync();

        if (!context.CatalogBrands.Any())
        {
            var seedCatalogBrands = GetPreconfiguredCatalogBrands();

            await context.CatalogBrands.AddRangeAsync(seedCatalogBrands);
            await context.SaveChangesAsync();
        }
        else
        {
            logger.LogInformation("Already have the catalog brands data - no need to seed");
        }

        if (!context.CatalogTypes.Any())
        {
            var seedCatalogTypes = GetPreconfiguredCatalogTypes();

            await context.CatalogTypes.AddRangeAsync(seedCatalogTypes);
            await context.SaveChangesAsync();
        }
        else
        {
            logger.LogInformation("Already have the catalog types data - no need to seed");
        }

        if (!context.CatalogItems.Any())
        {
            var seedCatalogItems = GetPreconfiguredItems();

            await context.CatalogItems.AddRangeAsync(seedCatalogItems);
            await context.SaveChangesAsync();
        }
        else
        {
            logger.LogInformation("Already have the catalog items data - no need to seed");
        }
    }

    private static IEnumerable<CatalogBrand> GetPreconfiguredCatalogBrands()
    {
        return new List<CatalogBrand>
        {
            new CatalogBrand { Brand = "Azure" },
            new CatalogBrand { Brand = ".NET" },
            new CatalogBrand { Brand = "Visual Studio" },
            new CatalogBrand { Brand = "SQL Server" },
            new CatalogBrand { Brand = "Other" }
        };
    }

    private static IEnumerable<CatalogType> GetPreconfiguredCatalogTypes()
    {
        return new List<CatalogType>
        {
            new CatalogType { Type = "Mug" },
            new CatalogType { Type = "T-Shirt" },
            new CatalogType { Type = "Sheet" },
            new CatalogType { Type = "USB Memory Stick" }
        };
    }

    private static IEnumerable<CatalogItem> GetPreconfiguredItems()
    {
        return new List<CatalogItem>
        {
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Bot Black Hoodie", Name = ".NET Bot Black Hoodie", Price = 19.5M, PictureFileName = "1.png" },
            new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Black & White Mug", Name = ".NET Black & White Mug", Price = 8.50M, PictureFileName = "2.png" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Prism White T-Shirt", Name = "Prism White T-Shirt", Price = 12, PictureFileName = "3.png" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Foundation T-shirt", Name = ".NET Foundation T-shirt", Price = 12, PictureFileName = "4.png" },
            new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 5, AvailableStock = 100, Description = "Roslyn Red Sheet", Name = "Roslyn Red Sheet", Price = 8.5M, PictureFileName = "5.png" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Blue Hoodie", Name = ".NET Blue Hoodie", Price = 12, PictureFileName = "6.png" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Roslyn Red T-Shirt", Name = "Roslyn Red T-Shirt", Price = 12, PictureFileName = "7.png" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Kudu Purple Hoodie", Name = "Kudu Purple Hoodie", Price = 8.5M, PictureFileName = "8.png" },
            new CatalogItem { CatalogTypeId = 1, CatalogBrandId = 5, AvailableStock = 100, Description = "Cup<T> White Mug", Name = "Cup<T> White Mug", Price = 12, PictureFileName = "9.png" },
            new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 100, Description = ".NET Foundation Sheet", Name = ".NET Foundation Sheet", Price = 12, PictureFileName = "10.png" },
            new CatalogItem { CatalogTypeId = 3, CatalogBrandId = 2, AvailableStock = 100, Description = "Cup<T> Sheet", Name = "Cup<T> Sheet", Price = 8.5M, PictureFileName = "11.png" },
            new CatalogItem { CatalogTypeId = 2, CatalogBrandId = 5, AvailableStock = 100, Description = "Prism White TShirt", Name = "Prism White TShirt", Price = 12, PictureFileName = "12.png" },
        };
    }
}