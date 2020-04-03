using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class ChangeCaseInUkStatusToNotifiedToPHEStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotifiedToPheStatus",
                table: "MDRDetails",
                maxLength: 30,
                nullable: true);

            // Set CountryId to UK country ID 235 for Cases in UK
            migrationBuilder.Sql(@"UPDATE MDRDetails SET CountryId = 235 WHERE CaseInUKStatus = 'Yes'");
            
            migrationBuilder.DropColumn(
                name: "CaseInUKStatus",
                table: "MDRDetails");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifiedToPheStatus",
                table: "MDRDetails");

            migrationBuilder.AddColumn<string>(
                name: "CaseInUKStatus",
                table: "MDRDetails",
                maxLength: 30,
                nullable: true);
        }
    }
}
