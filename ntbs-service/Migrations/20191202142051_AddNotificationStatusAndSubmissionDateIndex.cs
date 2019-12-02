using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddNotificationStatusAndSubmissionDateIndex : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationStatus_SubmissionDate",
                table: "Notification",
                columns: new[] { "NotificationStatus", "SubmissionDate" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_NotificationStatus_SubmissionDate",
                table: "Notification");
        }
    }
}
