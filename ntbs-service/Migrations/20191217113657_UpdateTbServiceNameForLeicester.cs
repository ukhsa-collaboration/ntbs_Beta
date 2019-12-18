using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class UpdateTbServiceNameForLeicester : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0107",
                column: "Name",
                value: "Leicester, Leicestershire & Rutland TB Service");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0107",
                column: "Name",
                value: "Leicester Royal Infirmary");
        }
    }
}
