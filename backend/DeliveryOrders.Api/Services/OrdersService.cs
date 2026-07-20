using DeliveryOrders.Api.Contracts;
using DeliveryOrders.Api.Data;
using DeliveryOrders.Api.Domain;
using Microsoft.EntityFrameworkCore;

namespace DeliveryOrders.Api.Services;

public sealed class OrdersService(OrdersDbContext db) : IOrdersService
{
    public async Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken)
    {
        var now = DateTimeOffset.UtcNow;
        var order = new Order
        {
            SenderCity = request.SenderCity!.Trim(),
            SenderAddress = request.SenderAddress!.Trim(),
            RecipientCity = request.RecipientCity!.Trim(),
            RecipientAddress = request.RecipientAddress!.Trim(),
            Weight = request.Weight,
            PickupDate = request.PickupDate,
            CreatedAt = now.UtcDateTime
        };

        db.Orders.Add(order);
        await db.SaveChangesAsync(cancellationToken);
        return ToResponse(order);
    }

    public async Task<IReadOnlyList<OrderResponse>> GetAllAsync(CancellationToken cancellationToken) =>
        await db.Orders.AsNoTracking()
            .OrderByDescending(order => order.CreatedAt)
            .Select(order => new OrderResponse(
                order.OrderNumber, order.SenderCity, order.SenderAddress,
                order.RecipientCity, order.RecipientAddress, order.Weight, order.PickupDate, new DateTimeOffset(order.CreatedAt)))
            .ToListAsync(cancellationToken);

    public async Task<OrderResponse?> GetByOrderNumberAsync(long orderNumber, CancellationToken cancellationToken) =>
        await db.Orders.AsNoTracking()
            .Where(order => order.OrderNumber == orderNumber)
            .Select(order => new OrderResponse(
                order.OrderNumber, order.SenderCity, order.SenderAddress,
                order.RecipientCity, order.RecipientAddress, order.Weight, order.PickupDate, new DateTimeOffset(order.CreatedAt)))
            .SingleOrDefaultAsync(cancellationToken);

    private static OrderResponse ToResponse(Order order) => new(
        order.OrderNumber, order.SenderCity, order.SenderAddress,
        order.RecipientCity, order.RecipientAddress, order.Weight, order.PickupDate, new DateTimeOffset(order.CreatedAt));
}
