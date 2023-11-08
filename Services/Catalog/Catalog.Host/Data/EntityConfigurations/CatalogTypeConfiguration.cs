using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Catalog.Host.Data.Entities;

namespace Catalog.Host.Data.EntityConfigurations;

public sealed class CatalogTypeConfiguration : IEntityTypeConfiguration<CatalogType>
{
    public void Configure(EntityTypeBuilder<CatalogType> builder)
    {
        builder.ToTable("CatalogType");

        builder.HasKey(catalogType => catalogType.Id);

        builder.Property(catalogType => catalogType.Id)
            .UseHiLo("catalog_type_hi_lo")
            .IsRequired();

        builder.Property(catalogType => catalogType.Type)
            .HasMaxLength(100)
            .IsRequired();
    }
}