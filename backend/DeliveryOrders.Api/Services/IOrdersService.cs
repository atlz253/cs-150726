using DeliveryOrders.Api.Contracts;

namespace DeliveryOrders.Api.Services;

public interface IOrdersService
{
    Task<OrderResponse> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken);
    Task<IReadOnlyList<OrderResponse>> GetAllAsync(CancellationToken cancellationToken);
    Task<OrderResponse?> GetByIdAsync(Guid id, CancellationToken cancellationToken);
}
