using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class RenameClinicalTimelineToClinicalDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicalTimelines");

            migrationBuilder.CreateTable(
                name: "ClinicalDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    SymptomStartDate = table.Column<DateTime>(nullable: true),
                    PresentationDate = table.Column<DateTime>(nullable: true),
                    DiagnosisDate = table.Column<DateTime>(nullable: true),
                    TreatmentStartDate = table.Column<DateTime>(nullable: true),
                    DeathDate = table.Column<DateTime>(nullable: true),
                    DidNotStartTreatment = table.Column<bool>(nullable: false),
                    IsPostMortem = table.Column<bool>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_ClinicalDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicalDetails");

            migrationBuilder.CreateTable(
                name: "ClinicalTimelines",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    DeathDate = table.Column<DateTime>(nullable: true),
                    DiagnosisDate = table.Column<DateTime>(nullable: true),
                    DidNotStartTreatment = table.Column<bool>(nullable: false),
                    IsPostMortem = table.Column<bool>(nullable: false),
                    PresentationDate = table.Column<DateTime>(nullable: true),
                    SymptomStartDate = table.Column<DateTime>(nullable: true),
                    TreatmentStartDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ClinicalTimelines", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_ClinicalTimelines_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }
    }
}
