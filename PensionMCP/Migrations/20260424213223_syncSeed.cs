using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PensionMCP.Migrations
{
    /// <inheritdoc />
    public partial class syncSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "NetRelevantIncome",
                value: 44000m);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                column: "NetRelevantIncome",
                value: 45000m);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                column: "NetRelevantIncome",
                value: 120000m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "NetRelevantIncome",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                column: "NetRelevantIncome",
                value: 0m);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                column: "NetRelevantIncome",
                value: 0m);
        }
    }
}
