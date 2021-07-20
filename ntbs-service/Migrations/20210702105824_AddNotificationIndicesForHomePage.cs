using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddNotificationIndicesForHomePage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(
                "CREATE NONCLUSTERED INDEX [IX_Notification_RecentNotifications] " +
                "    ON [dbo].[Notification] ([NotificationDate] DESC)" +
                "    INCLUDE ([CreationDate])" +
                "    WHERE ([NotificationStatus] IN ('Notified', 'Closed'));");

            migrationBuilder.Sql(
                "CREATE NONCLUSTERED INDEX [IX_Notification_RecentDrafts] " +
                "    ON [dbo].[Notification] ([CreationDate] DESC)" +
                "    INCLUDE ([NotificationDate])" +
                "    WHERE ([NotificationStatus]='Draft');");

            migrationBuilder.DropIndex(
                name: "IX_Notification_NotificationStatus_SubmissionDate",
                table: "Notification");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_RecentNotifications",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_RecentDrafts",
                table: "Notification");


            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationStatus_SubmissionDate",
                table: "Notification",
                columns: new[] { "NotificationStatus", "SubmissionDate" });
        }
    }
}
