using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class updateFTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_AspNetUsers_ApplicationUserId1",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_ApplicationUserId1",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId1",
                table: "Farmers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId1",
                table: "Farmers",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_ApplicationUserId1",
                table: "Farmers",
                column: "ApplicationUserId1");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_AspNetUsers_ApplicationUserId1",
                table: "Farmers",
                column: "ApplicationUserId1",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
