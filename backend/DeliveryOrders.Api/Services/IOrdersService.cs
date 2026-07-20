using DeliveryOrders.Api.Contracts;

namespace DeliveryOrders.Api.Services;

public interface IOrdersService
{
    Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    Task<PagedOrdersResponse> GetPageAsync(int page, CancellationToken cancellationToken);
    Task<OrderResponse?> GetByOrderNumberAsync(long orderNumber, CancellationToken cancellationToken);
}
