using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddUniquenessIndeciesOnLegacyIds : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notification_ETSID",
                table: "Notification",
                column: "ETSID",
                unique: true,
                filter: "[ETSID] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Notification_LTBRID",
                table: "Notification",
                column: "LTBRID",
                unique: true,
                filter: "[LTBRID] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_ETSID",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_LTBRID",
                table: "Notification");
        }
    }
}
