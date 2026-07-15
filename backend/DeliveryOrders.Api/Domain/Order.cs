namespace DeliveryOrders.Api.Domain;

public sealed class Order
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public string OrderNumber { get; init; } = string.Empty;
    public string SenderCity { get; init; } = string.Empty;
    public string SenderAddress { get; init; } = string.Empty;
    public string RecipientCity { get; init; } = string.Empty;
    public string RecipientAddress { get; init; } = string.Empty;
    public decimal Weight { get; init; }
    public DateOnly PickupDate { get; init; }
    public DateTime CreatedAt { get; init; }
}
