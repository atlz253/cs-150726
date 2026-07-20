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
            return Results.Created($"/api/orders/{order.OrderNumber}", order);
        }).WithName("CreateOrder");

        group.MapGet("", async (IOrdersService ordersService, CancellationToken cancellationToken, int page = 1) =>
        {
            if (page < 1)
                return Results.ValidationProblem(new Dictionary<string, string[]> { ["page"] = ["Номер страницы должен быть не меньше 1."] });

            var orders = await ordersService.GetPageAsync(page, cancellationToken);
            return Results.Ok(orders);
        }).WithName("GetOrders");

        group.MapGet("/{orderNumber:long}", async (long orderNumber, IOrdersService ordersService, CancellationToken cancellationToken) =>
        {
            var order = await ordersService.GetByOrderNumberAsync(orderNumber, cancellationToken);
            return order is null ? Results.NotFound() : Results.Ok(order);
        }).WithName("GetOrder");

        return app;
    }
}
