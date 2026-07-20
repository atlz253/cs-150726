using DeliveryOrders.Api.Contracts;
using DeliveryOrders.Api.Services;

namespace DeliveryOrders.Api.Endpoints;

public static class OrdersEndpoints
{
    public static IEndpointRouteBuilder MapOrdersEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/api/orders").WithTags("Orders");

        group.MapPost("", async (CreateOrderRequest request, IOrdersService ordersService, CancellationToken cancellationToken) =>
        {
            var errors = OrderValidation.Validate(request);
            if (errors.Count > 0) return Results.ValidationProblem(errors);

            var order = await ordersService.CreateAsync(request, cancellationToken);
            return Results.Created($"/api/orders/{order.Id}", order);
        }).WithName("CreateOrder");

        group.MapGet("", async (IOrdersService ordersService, CancellationToken cancellationToken) =>
        {
            var orders = await ordersService.GetAllAsync(cancellationToken);
            return Results.Ok(orders);
        }).WithName("GetOrders");

        group.MapGet("/{id:guid}", async (Guid id, IOrdersService ordersService, CancellationToken cancellationToken) =>
        {
            var order = await ordersService.GetByIdAsync(id, cancellationToken);
            return order is null ? Results.NotFound() : Results.Ok(order);
        }).WithName("GetOrder");

        return app;
    }
}
