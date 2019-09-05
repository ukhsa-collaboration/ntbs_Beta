using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddPatientTBHistoryFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientTBHistories",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    NotPreviouslyHadTB = table.Column<bool>(nullable: false),
                    PreviousTBDiagnosisYear = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PatientTBHistories", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_PatientTBHistories_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PatientTBHistories");
        }
    }
}
