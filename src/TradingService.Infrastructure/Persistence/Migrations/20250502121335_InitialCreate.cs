using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TradingService.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "trades",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    side = table.Column<string>(type: "character varying(4)", maxLength: 4, nullable: false),
                    quantity = table.Column<int>(type: "integer", nullable: false),
                    price = table.Column<decimal>(type: "numeric(9,2)", precision: 9, scale: 2, nullable: false),
                    total_amount = table.Column<decimal>(type: "numeric(9,2)", precision: 9, scale: 2, nullable: false),
                    executed_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_trades", x => x.id);
                    table.CheckConstraint("CK_Trade_Price_GreaterThanZero", "price > 0");
                    table.CheckConstraint("CK_Trade_Quantity_GreaterThanZero", "quantity > 0");
                });

            migrationBuilder.InsertData(
                table: "trades",
                columns: new[] { "id", "executed_at", "price", "quantity", "side", "total_amount" },
                values: new object[,]
                {
                    { new Guid("1570b546-7456-4358-bcc3-38b08b428bde"), new DateTime(2025, 3, 3, 9, 0, 0, 0, DateTimeKind.Utc), 25.0m, 200, "Buy", 5000.0m },
                    { new Guid("866bee10-bb20-4d6b-aab2-0df998d52b4f"), new DateTime(2025, 3, 1, 12, 0, 0, 0, DateTimeKind.Utc), 50.0m, 100, "Buy", 5000.0m },
                    { new Guid("c4ae962b-1cf5-498e-92ed-2db397b883c0"), new DateTime(2025, 3, 2, 14, 30, 0, 0, DateTimeKind.Utc), 75.0m, 50, "Sell", 3750.0m }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "trades");
        }
    }
}
