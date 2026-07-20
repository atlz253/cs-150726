using DeliveryOrders.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeliveryOrders.Api.Data;

public sealed class OrdersDbContext(DbContextOptions<OrdersDbContext> options) : DbContext(options)
{
    public DbSet<Order> Orders => Set<Order>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        var order = modelBuilder.Entity<Order>();
        order.HasKey(x => x.OrderNumber);
        order.Property(x => x.OrderNumber).ValueGeneratedOnAdd();
        order.Property(x => x.SenderCity).HasMaxLength(100).IsRequired();
        order.Property(x => x.SenderAddress).HasMaxLength(250).IsRequired();
        order.Property(x => x.RecipientCity).HasMaxLength(100).IsRequired();
        order.Property(x => x.RecipientAddress).HasMaxLength(250).IsRequired();
        order.Property(x => x.Weight).HasPrecision(10, 3);
    }
}
