using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class ChangeRiskFactorMentalHealthToStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskFactorMentalHealth");

            migrationBuilder.AddColumn<string>(
                name: "MentalHealthStatus",
                table: "SocialRiskFactors",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MentalHealthStatus",
                table: "SocialRiskFactors");

            migrationBuilder.CreateTable(
                name: "RiskFactorMentalHealth",
                columns: table => new
                {
                    SocialRiskFactorsNotificationId = table.Column<int>(nullable: false),
                    InPastFiveYears = table.Column<bool>(nullable: false),
                    IsCurrent = table.Column<bool>(nullable: false),
                    MoreThanFiveYearsAgo = table.Column<bool>(nullable: false),
                    Status = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskFactorMentalHealth", x => x.SocialRiskFactorsNotificationId);
                    table.ForeignKey(
                        name: "FK_RiskFactorMentalHealth_SocialRiskFactors_SocialRiskFactorsNotificationId",
                        column: x => x.SocialRiskFactorsNotificationId,
                        principalTable: "SocialRiskFactors",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
