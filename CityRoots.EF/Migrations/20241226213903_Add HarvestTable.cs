using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddHarvestTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HarvestRequests",
                columns: table => new
                {
                    HarvestRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    Date = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    HarvestId = table.Column<int>(type: "int", nullable: false),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    notificationsend = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HarvestRequests", x => x.HarvestRequestId);
                    table.ForeignKey(
                        name: "FK_HarvestRequests_Harvests_HarvestId",
                        column: x => x.HarvestId,
                        principalTable: "Harvests",
                        principalColumn: "HarvestId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HarvestRequests_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HarvestRequests_HarvestId",
                table: "HarvestRequests",
                column: "HarvestId");

            migrationBuilder.CreateIndex(
                name: "IX_HarvestRequests_MerchantId",
                table: "HarvestRequests",
                column: "MerchantId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HarvestRequests");
        }
    }
}
