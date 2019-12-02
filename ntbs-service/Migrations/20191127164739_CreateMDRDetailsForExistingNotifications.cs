using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class CreateMDRDetailsForExistingNotifications : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var insertQuery =  @"
            INSERT INTO MDRDetails (NotificationId)
            SELECT NotificationId FROM Notification;
            ";
            migrationBuilder.Sql(insertQuery);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
