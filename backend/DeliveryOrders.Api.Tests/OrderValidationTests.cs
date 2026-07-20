using DeliveryOrders.Api.Contracts;
using DeliveryOrders.Api.Services;
using Xunit;

namespace DeliveryOrders.Api.Tests;

public sealed class OrderValidationTests
{
    [Fact]
    public void Validate_ReturnsErrors_ForMissingFieldsAndInvalidWeight()
    {
        var errors = OrderValidation.Validate(new CreateOrderRequest("", null, "", null, 0, default));
        Assert.Equal(6, errors.Count);
    }
}
