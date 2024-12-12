using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class addshowupdatesandProdutionDatetoHarvest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
         

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Harvests",
                newName: "ProductionDate");

            migrationBuilder.AddColumn<bool>(
                name: "IsAlLowedToShowUpdatesToMerchant",
                table: "Harvests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAlLowedToShowUpdatesToMerchant",
                table: "Harvests");

            migrationBuilder.RenameColumn(
                name: "ProductionDate",
                table: "Harvests",
                newName: "Date");

         
        }
    }
}
