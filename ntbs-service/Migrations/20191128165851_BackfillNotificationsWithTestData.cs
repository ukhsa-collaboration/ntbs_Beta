using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class BackfillNotificationsWithTestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                insert into TestData (NotificationId)
                select n.NotificationId 
                from Notification n left join TestData t on n.NotificationId = t.NotificationId 
                where t.NotificationId is null;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
