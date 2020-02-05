using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class BackfillNotificationsWithDrugResistanceProfile : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql($@"
                INSERT INTO DrugResistanceProfile (NotificationId)
                SELECT n.NotificationId 
                FROM Notification n
                LEFT JOIN DrugResistanceProfile drp on n.NotificationId = drp.NotificationId 
                WHERE drp.NotificationId IS NULL;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
