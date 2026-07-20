using DeliveryOrders.Api.Data;
using DeliveryOrders.Api.Endpoints;
using DeliveryOrders.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddProblemDetails();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<OrdersDbContext>(options =>
    options.UseSqlite(builder.Configuration.GetConnectionString("OrdersDatabase")));
builder.Services.AddScoped<IOrdersService, OrdersService>();
builder.Services.AddCors(options => options.AddPolicy("frontend", policy =>
    policy.WithOrigins("http://localhost:5173").AllowAnyHeader().AllowAnyMethod()));

var app = builder.Build();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandler();
app.UseSwagger();
app.UseSwaggerUI();
app.UseCors("frontend");

using (var scope = app.Services.CreateScope())
{
    var database = scope.ServiceProvider.GetRequiredService<OrdersDbContext>().Database;
    if (database.IsRelational())
        database.Migrate();
}

app.MapGet("/health", () => Results.Ok(new { status = "ok" }));
app.MapOrdersEndpoints();

app.Run();

public partial class Program;
