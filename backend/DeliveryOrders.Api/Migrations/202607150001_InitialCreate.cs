using System;
using DeliveryOrders.Api.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryOrders.Api.Migrations;

[DbContext(typeof(OrdersDbContext))]
[Migration("202607150001_InitialCreate")]
public partial class InitialCreate : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Orders",
            columns: table => new
            {
                Id = table.Column<Guid>(type: "TEXT", nullable: false),
                OrderNumber = table.Column<string>(type: "TEXT", maxLength: 32, nullable: false),
                SenderCity = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                SenderAddress = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                RecipientCity = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                RecipientAddress = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Weight = table.Column<decimal>(type: "TEXT", precision: 10, scale: 3, nullable: false),
                PickupDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Orders", x => x.Id));
        migrationBuilder.CreateIndex(name: "IX_Orders_OrderNumber", table: "Orders", column: "OrderNumber", unique: true);
    }

    protected override void Down(MigrationBuilder migrationBuilder) => migrationBuilder.DropTable(name: "Orders");
}
