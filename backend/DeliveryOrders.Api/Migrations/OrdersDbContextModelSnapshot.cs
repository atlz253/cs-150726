using System;
using DeliveryOrders.Api.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace DeliveryOrders.Api.Migrations;

[DbContext(typeof(OrdersDbContext))]
partial class OrdersDbContextModelSnapshot : ModelSnapshot
{
    protected override void BuildModel(ModelBuilder modelBuilder)
    {
        modelBuilder.HasAnnotation("ProductVersion", "9.0.0");
        modelBuilder.Entity("DeliveryOrders.Api.Domain.Order", b =>
        {
            b.Property<Guid>("Id").HasColumnType("TEXT");
            b.Property<DateTime>("CreatedAt").HasColumnType("TEXT");
            b.Property<string>("OrderNumber").IsRequired().HasMaxLength(32).HasColumnType("TEXT");
            b.Property<DateOnly>("PickupDate").HasColumnType("TEXT");
            b.Property<string>("RecipientAddress").IsRequired().HasMaxLength(250).HasColumnType("TEXT");
            b.Property<string>("RecipientCity").IsRequired().HasMaxLength(100).HasColumnType("TEXT");
            b.Property<string>("SenderAddress").IsRequired().HasMaxLength(250).HasColumnType("TEXT");
            b.Property<string>("SenderCity").IsRequired().HasMaxLength(100).HasColumnType("TEXT");
            b.Property<decimal>("Weight").HasPrecision(10, 3).HasColumnType("TEXT");
            b.HasKey("Id");
            b.HasIndex("OrderNumber").IsUnique();
            b.ToTable("Orders");
        });
    }
}
