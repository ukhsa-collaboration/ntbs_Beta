using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddSocialRiskFactors : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "SocialRiskFactors",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    AlcoholMisuseStatus = table.Column<string>(nullable: true),
                    SmokingStatus = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialRiskFactors", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_SocialRiskFactors_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });


            migrationBuilder.CreateTable(
                name: "RiskFactorDrugs",
                columns: table => new
                {
                    SocialRiskFactorsNotificationId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    IsCurrent = table.Column<bool>(nullable: false),
                    InPastFiveYears = table.Column<bool>(nullable: false),
                    MoreThanFiveYearsAgo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskFactorDrugs", x => x.SocialRiskFactorsNotificationId);
                    table.ForeignKey(
                        name: "FK_RiskFactorDrugs_SocialRiskFactors_SocialRiskFactorsNotificationId",
                        column: x => x.SocialRiskFactorsNotificationId,
                        principalTable: "SocialRiskFactors",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskFactorHomelessness",
                columns: table => new
                {
                    SocialRiskFactorsNotificationId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    IsCurrent = table.Column<bool>(nullable: false),
                    InPastFiveYears = table.Column<bool>(nullable: false),
                    MoreThanFiveYearsAgo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskFactorHomelessness", x => x.SocialRiskFactorsNotificationId);
                    table.ForeignKey(
                        name: "FK_RiskFactorHomelessness_SocialRiskFactors_SocialRiskFactorsNotificationId",
                        column: x => x.SocialRiskFactorsNotificationId,
                        principalTable: "SocialRiskFactors",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskFactorImprisonment",
                columns: table => new
                {
                    SocialRiskFactorsNotificationId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    IsCurrent = table.Column<bool>(nullable: false),
                    InPastFiveYears = table.Column<bool>(nullable: false),
                    MoreThanFiveYearsAgo = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RiskFactorImprisonment", x => x.SocialRiskFactorsNotificationId);
                    table.ForeignKey(
                        name: "FK_RiskFactorImprisonment_SocialRiskFactors_SocialRiskFactorsNotificationId",
                        column: x => x.SocialRiskFactorsNotificationId,
                        principalTable: "SocialRiskFactors",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RiskFactorMentalHealth",
                columns: table => new
                {
                    SocialRiskFactorsNotificationId = table.Column<int>(nullable: false),
                    Status = table.Column<string>(nullable: true),
                    IsCurrent = table.Column<bool>(nullable: false),
                    InPastFiveYears = table.Column<bool>(nullable: false),
                    MoreThanFiveYearsAgo = table.Column<bool>(nullable: false)
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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RiskFactorDrugs");

            migrationBuilder.DropTable(
                name: "RiskFactorHomelessness");

            migrationBuilder.DropTable(
                name: "RiskFactorImprisonment");

            migrationBuilder.DropTable(
                name: "RiskFactorMentalHealth");

            migrationBuilder.DropTable(
                name: "SocialRiskFactors");
        }
    }
}
