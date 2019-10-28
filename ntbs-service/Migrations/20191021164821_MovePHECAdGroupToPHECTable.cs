using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MovePHECAdGroupToPHECTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PHECAdGroup",
                table: "TBService");

            migrationBuilder.AddColumn<string>(
                name: "AdGroup",
                table: "PHEC",
                maxLength: 64,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdGroup",
                table: "PHEC");

            migrationBuilder.AddColumn<string>(
                name: "PHECAdGroup",
                table: "TBService",
                maxLength: 64,
                nullable: true);
        }
    }
}
