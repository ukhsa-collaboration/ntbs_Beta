using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class ChangeDotAndEnhancedCaseManagementToStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDOT",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "IsEnhancedCaseManagement",
                table: "ClinicalDetails");

            migrationBuilder.AddColumn<string>(
                name: "DotStatus",
                table: "ClinicalDetails",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "EnhancedCaseManagementStatus",
                table: "ClinicalDetails",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DotStatus",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "EnhancedCaseManagementStatus",
                table: "ClinicalDetails");

            migrationBuilder.AddColumn<bool>(
                name: "IsDOT",
                table: "ClinicalDetails",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsEnhancedCaseManagement",
                table: "ClinicalDetails",
                nullable: true);
        }
    }
}
