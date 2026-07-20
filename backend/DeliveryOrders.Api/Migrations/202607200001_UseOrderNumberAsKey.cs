using System;
using DeliveryOrders.Api.Data;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DeliveryOrders.Api.Migrations;

[DbContext(typeof(OrdersDbContext))]
[Migration("202607200001_UseOrderNumberAsKey")]
public partial class UseOrderNumberAsKey : Migration
{
    protected override void Up(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Orders_New",
            columns: table => new
            {
                OrderNumber = table.Column<long>(type: "INTEGER", nullable: false)
                    .Annotation("Sqlite:Autoincrement", true),
                SenderCity = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                SenderAddress = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                RecipientCity = table.Column<string>(type: "TEXT", maxLength: 100, nullable: false),
                RecipientAddress = table.Column<string>(type: "TEXT", maxLength: 250, nullable: false),
                Weight = table.Column<decimal>(type: "TEXT", precision: 10, scale: 3, nullable: false),
                PickupDate = table.Column<DateOnly>(type: "TEXT", nullable: false),
                CreatedAt = table.Column<DateTime>(type: "TEXT", nullable: false)
            },
            constraints: table => table.PrimaryKey("PK_Orders_New", x => x.OrderNumber));

        migrationBuilder.Sql("""
            INSERT INTO "Orders_New" ("SenderCity", "SenderAddress", "RecipientCity", "RecipientAddress", "Weight", "PickupDate", "CreatedAt")
            SELECT "SenderCity", "SenderAddress", "RecipientCity", "RecipientAddress", "Weight", "PickupDate", "CreatedAt"
            FROM "Orders"
            ORDER BY "CreatedAt", "Id";
            """);

        migrationBuilder.DropTable(name: "Orders");
        migrationBuilder.RenameTable(name: "Orders_New", newName: "Orders");
    }

    protected override void Down(MigrationBuilder migrationBuilder)
    {
        migrationBuilder.CreateTable(
            name: "Orders_Old",
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
            constraints: table => table.PrimaryKey("PK_Orders_Old", x => x.Id));

        migrationBuilder.Sql("""
            INSERT INTO "Orders_Old" ("Id", "OrderNumber", "SenderCity", "SenderAddress", "RecipientCity", "RecipientAddress", "Weight", "PickupDate", "CreatedAt")
            SELECT lower(hex(randomblob(16))), CAST("OrderNumber" AS TEXT), "SenderCity", "SenderAddress", "RecipientCity", "RecipientAddress", "Weight", "PickupDate", "CreatedAt"
            FROM "Orders";
            """);

        migrationBuilder.DropTable(name: "Orders");
        migrationBuilder.RenameTable(name: "Orders_Old", newName: "Orders");
        migrationBuilder.CreateIndex(name: "IX_Orders_OrderNumber", table: "Orders", column: "OrderNumber", unique: true);
    }
}
