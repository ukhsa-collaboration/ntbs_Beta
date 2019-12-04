using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddMultiIndexOnAlertsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Alert_AlertStatus_AlertType_TbServiceCode",
                table: "Alert",
                columns: new[] { "AlertStatus", "AlertType", "TbServiceCode" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Alert_AlertStatus_AlertType_TbServiceCode",
                table: "Alert");
        }
    }
}
