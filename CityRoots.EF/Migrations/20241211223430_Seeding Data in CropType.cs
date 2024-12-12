using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class SeedingDatainCropType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "CropTypes",
                columns: new[] { "CropTypeId", "Name" },
                values: new object[,]
                {
                    { 1, "حبوب" },
                    { 2, "فاكهه" },
                    { 3, "خضار" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "CropTypes",
                keyColumn: "CropTypeId",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "CropTypes",
                keyColumn: "CropTypeId",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "CropTypes",
                keyColumn: "CropTypeId",
                keyValue: 3);
        }
    }
}
