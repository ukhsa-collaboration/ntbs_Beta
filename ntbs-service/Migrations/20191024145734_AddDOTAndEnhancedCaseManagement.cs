using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddDOTAndEnhancedCaseManagement : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDOT",
                table: "ClinicalDetails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnhancedCaseManagement",
                table: "ClinicalDetails",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDOT",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "IsEnhancedCaseManagement",
                table: "ClinicalDetails");
        }
    }
}
