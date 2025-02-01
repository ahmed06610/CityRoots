using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class MakeforfavouriteFarmerTablecompositeKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_favoriteFarmers",
                table: "favoriteFarmers");

            migrationBuilder.DropColumn(
                name: "Id",
                table: "favoriteFarmers");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "favoriteFarmers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AlterColumn<string>(
                name: "FarmerId",
                table: "favoriteFarmers",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_favoriteFarmers",
                table: "favoriteFarmers",
                columns: new[] { "userId", "FarmerId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_favoriteFarmers",
                table: "favoriteFarmers");

            migrationBuilder.AlterColumn<string>(
                name: "FarmerId",
                table: "favoriteFarmers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "userId",
                table: "favoriteFarmers",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "favoriteFarmers",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_favoriteFarmers",
                table: "favoriteFarmers",
                column: "Id");
        }
    }
}
