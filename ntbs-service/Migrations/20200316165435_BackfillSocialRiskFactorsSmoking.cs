using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class BackfillSocialRiskFactorsSmoking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                INSERT INTO RiskFactorSmoking (SocialRiskFactorsNotificationId)
                SELECT n.NotificationId
                FROM Notification n");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM RiskFactorSmoking");
        }
    }
}
