using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddTreatmentOutcomeTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TreatmentOutcome",
                columns: table => new
                {
                    TreatmentOutcomeId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    TreatmentOutcomeType = table.Column<string>(maxLength: 30, nullable: false),
                    TreatmentOutcomeSubType = table.Column<string>(maxLength: 30, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentOutcome", x => x.TreatmentOutcomeId);
                });

            migrationBuilder.CreateTable(
                name: "TreatmentEvent",
                columns: table => new
                {
                    TreatmentEventId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    EventDate = table.Column<DateTime>(nullable: false),
                    TreatmentEventType = table.Column<string>(maxLength: 30, nullable: false),
                    TreatmentOutcomeId = table.Column<int>(nullable: true),
                    Note = table.Column<string>(maxLength: 150, nullable: true),
                    NotificationId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TreatmentEvent", x => x.TreatmentEventId);
                    table.ForeignKey(
                        name: "FK_TreatmentEvent_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_TreatmentEvent_TreatmentOutcome_TreatmentOutcomeId",
                        column: x => x.TreatmentOutcomeId,
                        principalTable: "TreatmentOutcome",
                        principalColumn: "TreatmentOutcomeId",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentEvent_NotificationId",
                table: "TreatmentEvent",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_TreatmentEvent_TreatmentOutcomeId",
                table: "TreatmentEvent",
                column: "TreatmentOutcomeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TreatmentEvent");

            migrationBuilder.DropTable(
                name: "TreatmentOutcome");
        }
    }
}
