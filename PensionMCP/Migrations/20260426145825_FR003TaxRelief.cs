using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PensionMCP.Migrations
{
    /// <inheritdoc />
    public partial class FR003TaxRelief : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsMarried",
                table: "Clients",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsQualifyingSingleParent",
                table: "Clients",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "SpouseIncome",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "IsMarried", "IsQualifyingSingleParent", "SpouseIncome" },
                values: new object[] { false, false, -1m });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "IsMarried", "IsQualifyingSingleParent", "SpouseIncome" },
                values: new object[] { true, false, 30000m });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "IsMarried", "IsQualifyingSingleParent", "SpouseIncome" },
                values: new object[] { false, false, -1m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsMarried",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "IsQualifyingSingleParent",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "SpouseIncome",
                table: "Clients");
        }
    }
}
