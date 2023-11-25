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
            logger.LogError(ex, "[DbInitializer: Init] ==> An error occurred creating the DB.");
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

            logger.LogInformation("[DbInitializer: SeedData] ==> Catalog brand data has been successfully seeded");
        }
        else
        {
            logger.LogInformation("[DbInitializer: SeedData] ==> Already have the catalog brands data - no need to seed");
        }

        if (!context.CatalogTypes.Any())
        {
            var seedCatalogTypes = GetPreconfiguredCatalogTypes();

            await context.CatalogTypes.AddRangeAsync(seedCatalogTypes);
            await context.SaveChangesAsync();

            logger.LogInformation("[DbInitializer: SeedData] ==> Catalog type data has been successfully seeded");
        }
        else
        {
            logger.LogInformation("[DbInitializer: SeedData] ==> Already have the catalog types data - no need to seed");
        }

        if (!context.CatalogItems.Any())
        {
            var seedCatalogItems = GetPreconfiguredItems();

            await context.CatalogItems.AddRangeAsync(seedCatalogItems);
            await context.SaveChangesAsync();

            logger.LogInformation("[DbInitializer: SeedData] ==> Catalog item data has been successfully seeded");
        }
        else
        {
            logger.LogInformation("[DbInitializer: SeedData] ==> Already have the catalog items data - no need to seed");
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
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 2,
                AvailableStock = 100,
                Name = ".NET Bot Black Hoodie",
                Description = "Elevate your coding style with the .NET Bot Black Hoodie. Blend comfort and sophistication as you embrace your love for coding in this sleek and trendy hoodie.",
                Price = 19.5M,
                PictureFileName = "1.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 1,
                CatalogBrandId = 2,
                AvailableStock = 100,
                Name = ".NET Black & White Mug",
                Description = "Sip your favorite brew in coding elegance with the .NET Black & White Mug. The perfect blend of design and function for every .NET enthusiast's coffee break.",
                Price = 8.50M,
                PictureFileName = "2.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 5,
                AvailableStock = 100, 
                Name = "Prism White T-Shirt", 
                Description = "Unleash your coding creativity in the Prism White T-Shirt. This tee embodies simplicity and style, making it the perfect canvas for your coding adventures.", 
                Price = 12,
                PictureFileName = "3.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 2,
                AvailableStock = 100,
                Name = ".NET Foundation T-shirt",
                Description = "Show your support for .NET innovation with the .NET Foundation T-shirt. A comfortable and stylish way to celebrate your commitment to advancing technology.",
                Price = 12,
                PictureFileName = "4.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 5,
                AvailableStock = 100,
                Name = "Roslyn Red Sheet",
                Description = "Dress your bed in coding flair with the Roslyn Red Sheet. Comfort meets passion, making it the ideal choice for every .NET developer's relaxation zone.",
                Price = 8.5M,
                PictureFileName = "5.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 2,
                AvailableStock = 100,
                Name = ".NET Blue Hoodie",
                Description = "Stay warm and on-trend with the .NET Blue Hoodie. A cozy essential that adds a splash of color to your coding journey, blending comfort and style seamlessly.",
                Price = 12,
                PictureFileName = "6.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 5,
                AvailableStock = 100,
                Name = "Roslyn Red T-Shirt",
                Description = "Make a bold statement with the Roslyn Red T-Shirt. This vibrant tee is a tribute to your passion for coding, offering comfort and style in one.",
                Price = 12,
                PictureFileName = "7.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 5,
                AvailableStock = 100,
                Name = "Kudu Purple Hoodie",
                Description = "Code in comfort with the Kudu Purple Hoodie. A cozy essential with a touch of personality, reflecting your commitment to innovation and style.",
                Price = 8.5M,
                PictureFileName = "8.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 1,
                CatalogBrandId = 5,
                AvailableStock = 100,
                Name = "Cup<T> White Mug",
                Description = "Sip with a touch of humor using the Cup<T> White Mug. A playful nod to coding conventions, this mug adds a tech-inspired twist to your coffee routine.", 
                Price = 12, 
                PictureFileName = "9.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 2,
                AvailableStock = 100,
                Name = ".NET Foundation Sheet", 
                Description = "Dress your bed in .NET pride with the .NET Foundation Sheet. Comfort meets dedication in this sheet that celebrates the heart of coding innovation.", 
                Price = 12,
                PictureFileName = "10.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 3,
                CatalogBrandId = 2,
                AvailableStock = 100,
                Name = "Cup<T> Sheet", 
                Description = "Bring coding humor to your bedtime routine with the Cup<T> Sheet. Soft and quirky, this sheet is a playful addition to any coder's sleep haven.", 
                Price = 8.5M, 
                PictureFileName = "11.png"
            },
            new CatalogItem
            {
                CatalogTypeId = 2,
                CatalogBrandId = 5,
                AvailableStock = 100,
                Name = "Prism White TShirt", 
                Description = "Elevate your coding wardrobe with the Prism White T-Shirt. A symbol of simplicity and sophistication, perfect for expressing your coding passion.", 
                Price = 12, 
                PictureFileName = "12.png"
            },
        };
    }
}