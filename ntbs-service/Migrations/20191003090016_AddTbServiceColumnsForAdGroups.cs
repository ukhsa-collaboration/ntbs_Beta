using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddTbServiceColumnsForAdGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PHECAdGroup",
                table: "TBService",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceAdGroup",
                table: "TBService",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PHECAdGroup",
                table: "TBService");

            migrationBuilder.DropColumn(
                name: "ServiceAdGroup",
                table: "TBService");
        }
    }
}
