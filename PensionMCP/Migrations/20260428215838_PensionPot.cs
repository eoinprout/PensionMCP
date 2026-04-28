using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PensionMCP.Migrations
{
    /// <inheritdoc />
    public partial class PensionPot : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PlannedRetirementAge",
                table: "Clients",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 1,
                column: "PlannedRetirementAge",
                value: 66);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 2,
                column: "PlannedRetirementAge",
                value: 66);

            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: 3,
                column: "PlannedRetirementAge",
                value: 66);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PlannedRetirementAge",
                table: "Clients");
        }
    }
}
