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
        Assert.True(result.OrderNumber > 0);
        Assert.Equal(result.OrderNumber, await db.Orders.Select(order => order.OrderNumber).SingleAsync());
    }

    [Fact]
    public async Task GetPageAsync_ReturnsNewestOrdersFirst()
    {
        await using var db = CreateDbContext();
        var older = CreateOrder("Москва", new DateTime(2026, 7, 1, 10, 0, 0, DateTimeKind.Utc));
        var newer = CreateOrder("Казань", new DateTime(2026, 7, 2, 10, 0, 0, DateTimeKind.Utc));
        db.Orders.AddRange(older, newer);
        await db.SaveChangesAsync(CancellationToken.None);

        var result = await new OrdersService(db).GetPageAsync(1, CancellationToken.None);

        Assert.Collection(result.Items,
            order => Assert.Equal("Казань", order.SenderCity),
            order => Assert.Equal("Москва", order.SenderCity));
    }

    [Fact]
    public async Task GetByOrderNumberAsync_ReturnsNullWhenOrderDoesNotExist()
    {
        await using var db = CreateDbContext();

        var result = await new OrdersService(db).GetByOrderNumberAsync(999999, CancellationToken.None);

        Assert.Null(result);
    }

    private static OrdersDbContext CreateDbContext() => new(
        new DbContextOptionsBuilder<OrdersDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options);

    private static Order CreateOrder(string senderCity, DateTime createdAt) => new()
    {
        SenderCity = senderCity,
        SenderAddress = "Тверская, 1",
        RecipientCity = "Казань",
        RecipientAddress = "Баумана, 2",
        Weight = 1m,
        PickupDate = new DateOnly(2026, 8, 1),
        CreatedAt = createdAt
    };
}
