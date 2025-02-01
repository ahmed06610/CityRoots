using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddforeignkeytofavriteFarmerTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_favoriteFarmers_FarmerId",
                table: "favoriteFarmers",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_favoriteFarmers_AspNetUsers_FarmerId",
                table: "favoriteFarmers",
                column: "FarmerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_favoriteFarmers_AspNetUsers_FarmerId",
                table: "favoriteFarmers");

            migrationBuilder.DropIndex(
                name: "IX_favoriteFarmers_FarmerId",
                table: "favoriteFarmers");
        }
    }
}
