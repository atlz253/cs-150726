using DeliveryOrders.Api.Contracts;

namespace DeliveryOrders.Api.Services;

public static class OrderValidation
{
    public static Dictionary<string, string[]> Validate(CreateOrderRequest request)
    {
        var errors = new Dictionary<string, string[]>();
        Required(request.SenderCity, "senderCity", "Укажите город отправителя.", errors);
        Required(request.SenderAddress, "senderAddress", "Укажите адрес отправителя.", errors);
        Required(request.RecipientCity, "recipientCity", "Укажите город получателя.", errors);
        Required(request.RecipientAddress, "recipientAddress", "Укажите адрес получателя.", errors);

        if (request.Weight <= 0 || request.Weight > 1_000_000)
            errors["weight"] = ["Вес должен быть больше 0 и не превышать 1 000 000 кг."];
        if (request.PickupDate == default)
            errors["pickupDate"] = ["Укажите дату забора груза."];

        return errors;
    }

    private static void Required(string? value, string field, string message, IDictionary<string, string[]> errors)
    {
        if (string.IsNullOrWhiteSpace(value)) errors[field] = [message];
    }
}
