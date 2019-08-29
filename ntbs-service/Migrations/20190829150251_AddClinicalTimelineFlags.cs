using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddClinicalTimelineFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ClinicalTimelines",
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
                    table.PrimaryKey("PK_ClinicalTimelines", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_ClinicalTimelines_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ClinicalTimelines");
        }
    }
}
