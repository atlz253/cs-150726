using DeliveryOrders.Api.Contracts;
using DeliveryOrders.Api.Data;
using DeliveryOrders.Api.Domain;
using DeliveryOrders.Api.Services;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace DeliveryOrders.Api.Tests;

public sealed class OrdersServiceTests
{
    [Fact]
    public async Task CreateAsync_SavesAndReturnsTrimmedOrder()
    {
        await using var db = CreateDbContext();
        var service = new OrdersService(db);

        var result = await service.CreateAsync(
            new CreateOrderRequest(" Москва ", " Тверская, 1 ", " Казань ", " Баумана, 2 ", 12.5m, new DateOnly(2026, 8, 1)),
            CancellationToken.None);

        Assert.Equal("Москва", result.SenderCity);
        Assert.Equal("Баумана, 2", result.RecipientAddress);
        Assert.StartsWith("ORD-", result.OrderNumber);
        Assert.Equal(result.Id, await db.Orders.Select(order => order.Id).SingleAsync());
    }

    [Fact]
    public async Task GetAllAsync_ReturnsNewestOrdersFirst()
    {
        await using var db = CreateDbContext();
        var older = CreateOrder("ORD-OLDER", new DateTime(2026, 7, 1, 10, 0, 0, DateTimeKind.Utc));
        var newer = CreateOrder("ORD-NEWER", new DateTime(2026, 7, 2, 10, 0, 0, DateTimeKind.Utc));
        db.Orders.AddRange(older, newer);
        await db.SaveChangesAsync(CancellationToken.None);

        var result = await new OrdersService(db).GetAllAsync(CancellationToken.None);

        Assert.Collection(result,
            order => Assert.Equal("ORD-NEWER", order.OrderNumber),
            order => Assert.Equal("ORD-OLDER", order.OrderNumber));
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNullWhenOrderDoesNotExist()
    {
        await using var db = CreateDbContext();

        var result = await new OrdersService(db).GetByIdAsync(Guid.NewGuid(), CancellationToken.None);

        Assert.Null(result);
    }

    private static OrdersDbContext CreateDbContext() => new(
        new DbContextOptionsBuilder<OrdersDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static Order CreateOrder(string orderNumber, DateTime createdAt) => new()
    {
        Id = Guid.NewGuid(),
        OrderNumber = orderNumber,
        SenderCity = "Москва",
        SenderAddress = "Тверская, 1",
        RecipientCity = "Казань",
        RecipientAddress = "Баумана, 2",
        Weight = 1m,
        PickupDate = new DateOnly(2026, 8, 1),
        CreatedAt = createdAt
    };
}
