using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Order.Host.Data.Entities;

namespace Order.Host.Data.EntityConfigurations;

public sealed class OrderItemConfiguration : IEntityTypeConfiguration<OrderItem>
{
    public void Configure(EntityTypeBuilder<OrderItem> builder)
    {
        builder.ToTable("Order");

        builder.Property(orderItem => orderItem.Id)
            .UseHiLo("order_hi_lo")
            .IsRequired();

        builder.Property(orderItem => orderItem.Name)
            .HasMaxLength(100)
            .IsRequired();

        builder.Property(orderItem => orderItem.Date)
            // Do we need to add any extra constraints?
            .IsRequired();

        builder.Property(orderItem => orderItem.Products)
            // Do we need to add any other constraints?
            .IsRequired();

        builder.Property(orderItem => orderItem.Quantity)
            // Do we need to add any extra constraints?
            .IsRequired();

        builder.Property(orderItem => orderItem.TotalPrice)
            // Do we need to add any other constraints?
            .IsRequired();
    }
}