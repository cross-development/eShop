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
            .HasColumnType("timestamp")
            .IsRequired();

        builder.Property(orderItem => orderItem.Products)
            .IsRequired();

        builder.Property(orderItem => orderItem.Quantity)
            .IsRequired();

        builder.Property(orderItem => orderItem.TotalPrice)
            .IsRequired();

        builder.Property(orderItem => orderItem.UserId)
            .IsRequired();
    }
}