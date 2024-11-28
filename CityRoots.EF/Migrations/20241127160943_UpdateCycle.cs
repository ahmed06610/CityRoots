using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCycle : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableProfitTypes",
                table: "Cycles");

            migrationBuilder.AddColumn<string>(
                name: "AvailableProfitTypes",
                table: "OpenInvestmentCycles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableProfitTypes",
                table: "OpenInvestmentCycles");

            migrationBuilder.AddColumn<string>(
                name: "AvailableProfitTypes",
                table: "Cycles",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
