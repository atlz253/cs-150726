using System.Net;
using System.Net.Http.Json;
using DeliveryOrders.Api.Contracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace DeliveryOrders.Api.Tests;

public sealed class OrdersApiTests : IClassFixture<OrdersApiTests.TestApplication>
{
    private readonly HttpClient _client;

    public OrdersApiTests(TestApplication application) => _client = application.CreateClient();

    [Fact]
    public async Task CreateThenGet_ReturnsCreatedOrder()
    {
        var request = new CreateOrderRequest("Москва", "ул. Тверская, 1", "Казань", "ул. Баумана, 2", 12.5m, new DateOnly(2026, 8, 1));
        var create = await _client.PostAsJsonAsync("/api/orders", request);

        Assert.True(create.StatusCode == HttpStatusCode.Created, await create.Content.ReadAsStringAsync());
        var created = await create.Content.ReadFromJsonAsync<OrderResponse>();
        Assert.NotNull(created);
        Assert.True(created.OrderNumber > 0);

        var get = await _client.GetAsync($"/api/orders/{created.OrderNumber}");
        var order = await get.Content.ReadFromJsonAsync<OrderResponse>();
        Assert.Equal(HttpStatusCode.OK, get.StatusCode);
        Assert.Equal("Москва", order!.SenderCity);
    }

    [Fact]
    public async Task List_ReturnsOrdersNewestFirst()
    {
        var request = new CreateOrderRequest("Пермь", "Ленина, 1", "Уфа", "Мира, 2", 1m, new DateOnly(2026, 8, 1));
        await _client.PostAsJsonAsync("/api/orders", request);

        var response = await _client.GetAsync("/api/orders");
        Assert.True(response.StatusCode == HttpStatusCode.OK, await response.Content.ReadAsStringAsync());
        var orders = await response.Content.ReadFromJsonAsync<PagedOrdersResponse>();
        Assert.NotEmpty(orders!.Items);
        Assert.Equal(1, orders.Page);
        Assert.Equal(20, orders.PageSize);
    }

    [Fact]
    public async Task Create_AssignsSequentialOrderNumbers()
    {
        var first = await CreateOrderAsync("Москва");
        var second = await CreateOrderAsync("Казань");

        Assert.Equal(first.OrderNumber + 1, second.OrderNumber);
    }

    [Fact]
    public async Task List_ReturnsDistinctPagesOfTwentyOrders()
    {
        for (var index = 0; index < 21; index++)
            await CreateOrderAsync($"Город {index}");

        var firstPage = await _client.GetFromJsonAsync<PagedOrdersResponse>("/api/orders?page=1");
        var secondPage = await _client.GetFromJsonAsync<PagedOrdersResponse>("/api/orders?page=2");

        Assert.NotNull(firstPage);
        Assert.NotNull(secondPage);
        Assert.Equal(20, firstPage.Items.Count);
        Assert.True(secondPage.Items.Count > 0);
        Assert.Empty(firstPage.Items.Select(order => order.OrderNumber).Intersect(secondPage.Items.Select(order => order.OrderNumber)));
        Assert.True(firstPage.TotalCount >= 21);
        Assert.True(firstPage.TotalPages >= 2);
    }

    [Fact]
    public async Task List_ReturnsValidationProblemForPageBelowOne()
    {
        var response = await _client.GetAsync("/api/orders?page=0");

        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task List_ReturnsEmptyItemsForPageBeyondAvailableData()
    {
        var response = await _client.GetFromJsonAsync<PagedOrdersResponse>("/api/orders?page=999999");

        Assert.NotNull(response);
        Assert.Empty(response.Items);
    }

    [Fact]
    public async Task Create_ReturnsValidationProblem_ForInvalidPayload()
    {
        var response = await _client.PostAsJsonAsync("/api/orders", new CreateOrderRequest("", "", "", "", 0, default));
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Fact]
    public async Task Get_ReturnsNotFound_ForUnknownOrder()
    {
        var response = await _client.GetAsync("/api/orders/999999");
        Assert.True(response.StatusCode == HttpStatusCode.NotFound, await response.Content.ReadAsStringAsync());
    }

    private async Task<OrderResponse> CreateOrderAsync(string senderCity)
    {
        var response = await _client.PostAsJsonAsync(
            "/api/orders",
            new CreateOrderRequest(senderCity, "Ленина, 1", "Уфа", "Мира, 2", 1m, new DateOnly(2026, 8, 1)));
        Assert.True(response.StatusCode == HttpStatusCode.Created, await response.Content.ReadAsStringAsync());
        return (await response.Content.ReadFromJsonAsync<OrderResponse>())!;
    }

    public sealed class TestApplication : WebApplicationFactory<Program>
    {
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.UseEnvironment("Development");
            builder.ConfigureAppConfiguration((_, configuration) =>
                configuration.AddInMemoryCollection(new Dictionary<string, string?>
                {
                    ["ConnectionStrings:OrdersDatabase"] = $"Data Source=orders-tests-{Guid.NewGuid():N}.db"
                }));
        }
    }
}
