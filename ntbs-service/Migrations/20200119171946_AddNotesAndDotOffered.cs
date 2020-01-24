using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddNotesAndDotOffered : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDotOffered",
                table: "ClinicalDetails",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ClinicalDetails",
                maxLength: 500,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDotOffered",
                table: "ClinicalDetails");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ClinicalDetails");
        }
    }
}
