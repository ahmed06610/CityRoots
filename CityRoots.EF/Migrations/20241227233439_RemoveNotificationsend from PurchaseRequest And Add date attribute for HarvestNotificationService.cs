using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class RemoveNotificationsendfromPurchaseRequestAndAdddateattributeforHarvestNotificationService : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "notificationsend",
                table: "PurchaseRequests");

            migrationBuilder.RenameColumn(
                name: "HarvestType",
                table: "HarvestNotificationLogs",
                newName: "HarvestNotificationType");

            migrationBuilder.AddColumn<DateTime>(
                name: "NotificationDate",
                table: "HarvestNotificationLogs",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationDate",
                table: "HarvestNotificationLogs");

            migrationBuilder.RenameColumn(
                name: "HarvestNotificationType",
                table: "HarvestNotificationLogs",
                newName: "HarvestType");

            migrationBuilder.AddColumn<bool>(
                name: "notificationsend",
                table: "PurchaseRequests",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
