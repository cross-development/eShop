using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations;

public sealed class CatalogBrandConfiguration : IEntityTypeConfiguration<CatalogBrand>
{
    public void Configure(EntityTypeBuilder<CatalogBrand> builder)
    {
        builder.ToTable("CatalogBrand");

        builder.HasKey(catalogBrand => catalogBrand.Id);

        builder.Property(catalogBrand => catalogBrand.Id)
            .UseHiLo("catalog_brand_hi_lo")
            .IsRequired();

        builder.Property(catalogBrand => catalogBrand.Brand)
            .HasMaxLength(100)
            .IsRequired();
    }
}