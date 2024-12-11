using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class RenameAipredictColumn : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "recommendation",
                table: "AiPredicts",
                newName: "Recommendation");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Recommendation",
                table: "AiPredicts",
                newName: "recommendation");
        }
    }
}
