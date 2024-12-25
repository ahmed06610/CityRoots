using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class cyclenotificationlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "cycleNotificationLogs",
                columns: table => new
                {
                    NotificationLogId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CycleId = table.Column<int>(type: "int", nullable: false),
                    CycleNotificationType = table.Column<int>(type: "int", nullable: false),
                    NotificationDate = table.Column<DateTime>(type: "datetime2", nullable: false),
                    InvestmentRequestId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_cycleNotificationLogs", x => x.NotificationLogId);
                    table.ForeignKey(
                        name: "FK_cycleNotificationLogs_Cycles_CycleId",
                        column: x => x.CycleId,
                        principalTable: "Cycles",
                        principalColumn: "CycleId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_cycleNotificationLogs_CycleId",
                table: "cycleNotificationLogs",
                column: "CycleId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "cycleNotificationLogs");
        }
    }
}
