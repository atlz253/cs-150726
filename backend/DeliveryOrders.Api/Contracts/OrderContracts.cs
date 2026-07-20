namespace DeliveryOrders.Api.Contracts;

public sealed record CreateOrderRequest(
    string? SenderCity,
    string? SenderAddress,
    string? RecipientCity,
    string? RecipientAddress,
    decimal Weight,
    DateOnly PickupDate);

public sealed record OrderResponse(
    long OrderNumber,
    string SenderCity,
    string SenderAddress,
    string RecipientCity,
    string RecipientAddress,
    decimal Weight,
    DateOnly PickupDate,
    DateTimeOffset CreatedAt);
