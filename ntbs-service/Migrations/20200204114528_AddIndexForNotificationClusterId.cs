using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddIndexForNotificationClusterId : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClusterId",
                table: "Notification",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_ClusterId",
                table: "Notification",
                column: "ClusterId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_ClusterId",
                table: "Notification");

            migrationBuilder.AlterColumn<string>(
                name: "ClusterId",
                table: "Notification",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
