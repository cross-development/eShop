using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations;

public sealed class CatalogItemConfiguration : IEntityTypeConfiguration<CatalogItem>
{
    public void Configure(EntityTypeBuilder<CatalogItem> builder)
    {
        builder.ToTable("Catalog");

        builder.Property(catalogItem => catalogItem.Id)
            .UseHiLo("catalog_hi_lo")
            .IsRequired();

        builder.Property(catalogItem => catalogItem.Name)
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(catalogItem => catalogItem.Description)
            .HasMaxLength(200)
            .IsRequired();
        
        builder.Property(catalogItem => catalogItem.Price)
            .IsRequired();

        builder.Property(catalogItem => catalogItem.PictureFileName)
            .IsRequired(false);

        builder.HasOne(catalogItem => catalogItem.CatalogBrand)
            .WithMany(catalogBrand => catalogBrand.CatalogItems)
            .HasForeignKey(catalogItem => catalogItem.CatalogBrandId);

        builder.HasOne(catalogItem => catalogItem.CatalogType)
            .WithMany(catalogType => catalogType.CatalogItems)
            .HasForeignKey(catalogItem => catalogItem.CatalogTypeId);
    }
}