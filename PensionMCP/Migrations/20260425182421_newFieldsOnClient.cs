using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PensionMCP.Migrations
{
    /// <inheritdoc />
    public partial class newFieldsOnClient : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "CurrentMonthlyEmployersContribution",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentMonthlyPensionContribution",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "CurrentPensionPotValue",
                table: "Clients",
                type: "TEXT",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "CurrentMonthlyEmployersContribution", "CurrentMonthlyPensionContribution", "CurrentPensionPotValue" },
                values: new object[] { 1000m, 1000m, 100000m });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "CurrentMonthlyEmployersContribution", "CurrentMonthlyPensionContribution", "CurrentPensionPotValue" },
                values: new object[] { -1m, -1m, -1m });

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "CurrentMonthlyEmployersContribution", "CurrentMonthlyPensionContribution", "CurrentPensionPotValue" },
                values: new object[] { -1m, -1m, -1m });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentMonthlyEmployersContribution",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CurrentMonthlyPensionContribution",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "CurrentPensionPotValue",
                table: "Clients");
        }
    }
}
