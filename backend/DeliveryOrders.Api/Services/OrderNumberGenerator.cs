namespace DeliveryOrders.Api.Services;

public static class OrderNumberGenerator
{
    public static string Create(DateTimeOffset now, Guid id) =>
        $"ORD-{now:yyyyMMdd}-{id.ToString("N")[..8].ToUpperInvariant()}";
}
