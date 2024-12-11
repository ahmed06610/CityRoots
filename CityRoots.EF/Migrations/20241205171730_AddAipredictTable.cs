using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CityRoots.EF.Migrations
{
    /// <inheritdoc />
    public partial class AddAipredictTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
           

            migrationBuilder.CreateTable(
                name: "AiPredicts",
                columns: table => new
                {
                    AiPredictId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ArabicName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    EnglishName = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Diagnosis = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    recommendation = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AiPredicts", x => x.AiPredictId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AiPredicts");

            migrationBuilder.DropColumn(
                name: "AvailableProfitTypes",
                table: "OpenInvestmentCycles");

            migrationBuilder.RenameColumn(
                name: "CycleName",
                table: "Cycles",
                newName: "AvailableProfitTypes");
        }
    }
}
