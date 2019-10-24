using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddDOTAndEnhancedCaseManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isDOT",
                table: "ClinicalDetails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isEnhancedCaseManagement",
                table: "ClinicalDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isDOT",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "isEnhancedCaseManagement",
                table: "ClinicalDetails");
        }
    }
}
