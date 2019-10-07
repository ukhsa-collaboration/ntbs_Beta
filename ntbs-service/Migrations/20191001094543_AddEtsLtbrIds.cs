using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddEtsLtbrIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ETSID",
                table: "Notification",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LTBRID",
                table: "Notification",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ETSID",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "LTBRID",
                table: "Notification");
        }
    }
}
