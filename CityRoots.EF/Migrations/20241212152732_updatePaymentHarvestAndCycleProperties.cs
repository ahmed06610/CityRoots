using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class updatePaymentHarvestAndCycleProperties : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CycleId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "HarvestId",
                table: "Payments",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Payments_CycleId",
                table: "Payments",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Payments_HarvestId",
                table: "Payments",
                column: "HarvestId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Cycles_CycleId",
                table: "Payments",
                column: "CycleId",
                principalTable: "Cycles",
                principalColumn: "CycleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Payments_Harvests_HarvestId",
                table: "Payments",
                column: "HarvestId",
                principalTable: "Harvests",
                principalColumn: "HarvestId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Cycles_CycleId",
                table: "Payments");

            migrationBuilder.DropForeignKey(
                name: "FK_Payments_Harvests_HarvestId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_CycleId",
                table: "Payments");

            migrationBuilder.DropIndex(
                name: "IX_Payments_HarvestId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "CycleId",
                table: "Payments");

            migrationBuilder.DropColumn(
                name: "HarvestId",
                table: "Payments");
        }
    }
}
