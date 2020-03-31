using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class ChangeCaseInUkStatusToNotifiedToPHEStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CaseInUKStatus",
                table: "MDRDetails");

            migrationBuilder.AddColumn<string>(
                name: "NotifiedToPheStatus",
                table: "MDRDetails",
                maxLength: 30,
                nullable: true);
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
