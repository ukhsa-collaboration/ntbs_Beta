using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class UpdatePreviousHistoryTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PreviousTbHistory",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    PreviouslyHadTb = table.Column<string>(maxLength: 30, nullable: true),
                    PreviousTbDiagnosisYear = table.Column<int>(nullable: true),
                    PreviouslyTreated = table.Column<string>(maxLength: 30, nullable: true),
                    PreviousTreatmentCountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PreviousTbHistory", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_PreviousTbHistory_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.Sql(@"
            INSERT INTO PreviousTbHistory (NotificationId, PreviouslyHadTb, PreviousTbDiagnosisYear)
            SELECT NotificationId
                ,(Case WHEN PreviouslyHadTB = 0 THEN 'No' WHEN PreviouslyHadTB = 1 THEN 'Yes' END)
                ,PreviousTBDiagnosisYear
            FROM PatientTBHistories
            ");
            
            migrationBuilder.CreateIndex(
                name: "IX_PreviousTbHistory_PreviousTreatmentCountryId",
                table: "PreviousTbHistory",
                column: "PreviousTreatmentCountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_PreviousTbHistory_Country_PreviousTreatmentCountryId",
                table: "PreviousTbHistory",
                column: "PreviousTreatmentCountryId",
                principalSchema: "ReferenceData",
                principalTable: "Country",
                principalColumn: "CountryId",
                onDelete: ReferentialAction.Restrict);
            
            migrationBuilder.DropTable(
                name: "PatientTBHistories");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "PatientTBHistories",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    PreviousTBDiagnosisYear = table.Column<int>(nullable: true),
                    PreviouslyHadTB = table.Column<bool>(nullable: true)
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
            
            migrationBuilder.Sql(@"
            INSERT INTO PatientTBHistories (NotificationId, PreviouslyHadTB, PreviousTBDiagnosisYear)
            SELECT NotificationId
                ,(Case WHEN PreviouslyHadTb = 'No' THEN 0 WHEN PreviouslyHadTb = 'Yes' THEN 1 END)
                ,PreviousTBDiagnosisYear
            FROM PreviousTbHistory
            ");
            
            migrationBuilder.DropTable(
                name: "PreviousTbHistory");
        }
    }
}
