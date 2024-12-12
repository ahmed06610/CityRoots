using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddTaskNameandTaskDescriptionToSchdeuleTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TaskType",
                table: "Schedules",
                newName: "TaskName");

            migrationBuilder.AddColumn<string>(
                name: "TaskDescription",
                table: "Schedules",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TaskDescription",
                table: "Schedules");

            migrationBuilder.RenameColumn(
                name: "TaskName",
                table: "Schedules",
                newName: "TaskType");
        }
    }
}
