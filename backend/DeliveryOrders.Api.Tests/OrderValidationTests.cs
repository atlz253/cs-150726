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

    [Fact]
    public void Create_BuildsReadableStableNumber()
    {
        var id = Guid.Parse("a0b1c2d3-1111-2222-3333-444444444444");
        var number = OrderNumberGenerator.Create(new DateTimeOffset(2026, 7, 15, 10, 0, 0, TimeSpan.Zero), id);
        Assert.Equal("ORD-20260715-A0B1C2D3", number);
    }
}
