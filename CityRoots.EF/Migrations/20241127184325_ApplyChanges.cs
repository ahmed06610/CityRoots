using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class ApplyChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Purchases");

            migrationBuilder.AddColumn<int>(
                name: "CycleId",
                table: "Harvests",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FarmerId",
                table: "Harvests",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ImageUrl",
                table: "Harvests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<decimal>(
                name: "Price",
                table: "Harvests",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "status",
                table: "Harvests",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "CropType",
                table: "Crops",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "PurchaseRequests",
                columns: table => new
                {
                    PurchaseRequestId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HarvestId = table.Column<int>(type: "int", nullable: false),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    RequestedAmount = table.Column<double>(type: "float", nullable: false),
                    RequestedPrice = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RequestStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    RequestDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Notes = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PurchaseRequests", x => x.PurchaseRequestId);
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Harvests_HarvestId",
                        column: x => x.HarvestId,
                        principalTable: "Harvests",
                        principalColumn: "HarvestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_PurchaseRequests_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_CycleId",
                table: "Harvests",
                column: "CycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Harvests_FarmerId",
                table: "Harvests",
                column: "FarmerId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_HarvestId",
                table: "PurchaseRequests",
                column: "HarvestId");

            migrationBuilder.CreateIndex(
                name: "IX_PurchaseRequests_MerchantId",
                table: "PurchaseRequests",
                column: "MerchantId");

            migrationBuilder.AddForeignKey(
                name: "FK_Harvests_Cycles_CycleId",
                table: "Harvests",
                column: "CycleId",
                principalTable: "Cycles",
                principalColumn: "CycleId");

            migrationBuilder.AddForeignKey(
                name: "FK_Harvests_Farmers_FarmerId",
                table: "Harvests",
                column: "FarmerId",
                principalTable: "Farmers",
                principalColumn: "FarmerId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Harvests_Cycles_CycleId",
                table: "Harvests");

            migrationBuilder.DropForeignKey(
                name: "FK_Harvests_Farmers_FarmerId",
                table: "Harvests");

            migrationBuilder.DropTable(
                name: "PurchaseRequests");

            migrationBuilder.DropIndex(
                name: "IX_Harvests_CycleId",
                table: "Harvests");

            migrationBuilder.DropIndex(
                name: "IX_Harvests_FarmerId",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "CycleId",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "FarmerId",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "ImageUrl",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "Price",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "status",
                table: "Harvests");

            migrationBuilder.DropColumn(
                name: "CropType",
                table: "Crops");

            migrationBuilder.CreateTable(
                name: "Purchases",
                columns: table => new
                {
                    PurchaseId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    HarvestId = table.Column<int>(type: "int", nullable: false),
                    MerchantId = table.Column<int>(type: "int", nullable: false),
                    Amount = table.Column<double>(type: "float", nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    Status = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Purchases", x => x.PurchaseId);
                    table.ForeignKey(
                        name: "FK_Purchases_Harvests_HarvestId",
                        column: x => x.HarvestId,
                        principalTable: "Harvests",
                        principalColumn: "HarvestId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Purchases_Merchants_MerchantId",
                        column: x => x.MerchantId,
                        principalTable: "Merchants",
                        principalColumn: "MerchantId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_HarvestId",
                table: "Purchases",
                column: "HarvestId");

            migrationBuilder.CreateIndex(
                name: "IX_Purchases_MerchantId",
                table: "Purchases",
                column: "MerchantId");
        }
    }
}
