using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddCropTypeandReturnImageUrlForHarvest : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CropType",
                table: "Crops");

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Harvests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<int>(
                name: "CropTypeId",
                table: "Crops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "CropTypes",
                columns: table => new
                {
                    CropTypeId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CropTypes", x => x.CropTypeId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Crops_CropTypeId",
                table: "Crops",
                column: "CropTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Crops_CropTypes_CropTypeId",
                table: "Crops",
                column: "CropTypeId",
                principalTable: "CropTypes",
                principalColumn: "CropTypeId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Crops_CropTypes_CropTypeId",
                table: "Crops");

            migrationBuilder.DropTable(
                name: "CropTypes");

            migrationBuilder.DropIndex(
                name: "IX_Crops_CropTypeId",
                table: "Crops");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "CropTypeId",
                table: "Crops");

            migrationBuilder.AddColumn<string>(
                name: "CropType",
                table: "Crops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }
    }
}
