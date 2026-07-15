using DeliveryOrders.Api.Contracts;
using DeliveryOrders.Api.Data;
using DeliveryOrders.Api.Domain;
using DeliveryOrders.Api.Services;
using Microsoft.EntityFrameworkCore;

namespace DeliveryOrders.Api.Endpoints;

public static class OrdersEndpoints
{
    public static IEndpointRouteBuilder MapOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");

        group.MapPost("", async (CreateOrderRequest request, OrdersDbContext db, CancellationToken cancellationToken) =>
        {
            var errors = OrderValidation.Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);

            var id = Guid.NewGuid();
            var now = DateTimeOffset.UtcNow;
            var order = new Order
            {
                Id = id,
                OrderNumber = OrderNumberGenerator.Create(now, id),
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
            return Results.Created($"/api/orders/{order.Id}", ToResponse(order));
        }).WithName("CreateOrder");

        group.MapGet("", async (OrdersDbContext db, CancellationToken cancellationToken) =>
        {
            var orders = await db.Orders.AsNoTracking()
                .OrderByDescending(order => order.CreatedAt)
                .ToListAsync(cancellationToken);
            return Results.Ok(orders.Select(ToResponse));
        }).WithName("GetOrders");

        group.MapGet("/{id:guid}", async (Guid id, OrdersDbContext db, CancellationToken cancellationToken) =>
        {
            var order = await db.Orders.AsNoTracking().SingleOrDefaultAsync(order => order.Id == id, cancellationToken);
            return order is null ? Results.NotFound() : Results.Ok(ToResponse(order));
        }).WithName("GetOrder");

        return app;
    }

    private static OrderResponse ToResponse(Order order) => new(
        order.Id, order.OrderNumber, order.SenderCity, order.SenderAddress,
        order.RecipientCity, order.RecipientAddress, order.Weight, order.PickupDate, new DateTimeOffset(order.CreatedAt));
}
