using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddMDRDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MDRDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    ExposureToKnownCaseStatus = table.Column<string>(maxLength: 30, nullable: true),
                    RelationshipToCase = table.Column<string>(maxLength: 40, nullable: true),
                    CaseInUKStatus = table.Column<string>(maxLength: 30, nullable: true),
                    RelatedNotificationId = table.Column<int>(nullable: true),
                    CountryId = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MDRDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_MDRDetails_Country_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MDRDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_MDRDetails_CountryId",
                table: "MDRDetails",
                column: "CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MDRDetails");
        }
    }
}
