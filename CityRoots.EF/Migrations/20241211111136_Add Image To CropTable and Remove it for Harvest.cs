using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddImageToCropTableandRemoveitforHarvest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Harvests");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Crops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Crops");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Harvests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
