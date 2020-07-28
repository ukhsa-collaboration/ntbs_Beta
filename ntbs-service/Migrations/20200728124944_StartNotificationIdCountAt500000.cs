using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class StartNotificationIdCountAt500000 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // This is to stay wall clear of potential clashes with ETS ids which are also incrementing
            // integers (last ones seen at the time of writing are 244851)
            migrationBuilder.Sql("DBCC CHECKIDENT ('Notification', RESEED, 400000)");

        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
