using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddVisitAndTravelDetailsEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "TravelDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    HasTravel = table.Column<bool>(nullable: true),
                    CountryId1 = table.Column<int>(nullable: true),
                    Country1CountryId = table.Column<int>(nullable: true),
                    StayLengthInMonths1 = table.Column<int>(nullable: true),
                    CountryId2 = table.Column<int>(nullable: true),
                    Country2CountryId = table.Column<int>(nullable: true),
                    StayLengthInMonths2 = table.Column<int>(nullable: true),
                    CountryId3 = table.Column<int>(nullable: true),
                    Country3CountryId = table.Column<int>(nullable: true),
                    StayLengthInMonths3 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TravelDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_TravelDetails_Country_Country1CountryId",
                        column: x => x.Country1CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelDetails_Country_Country2CountryId",
                        column: x => x.Country2CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelDetails_Country_Country3CountryId",
                        column: x => x.Country3CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_TravelDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VisitorDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    HasVisitor = table.Column<bool>(nullable: true),
                    CountryId1 = table.Column<int>(nullable: true),
                    Country1CountryId = table.Column<int>(nullable: true),
                    StayLengthInMonths1 = table.Column<int>(nullable: true),
                    CountryId2 = table.Column<int>(nullable: true),
                    Country2CountryId = table.Column<int>(nullable: true),
                    StayLengthInMonths2 = table.Column<int>(nullable: true),
                    CountryId3 = table.Column<int>(nullable: true),
                    Country3CountryId = table.Column<int>(nullable: true),
                    StayLengthInMonths3 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VisitorDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_VisitorDetails_Country_Country1CountryId",
                        column: x => x.Country1CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitorDetails_Country_Country2CountryId",
                        column: x => x.Country2CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitorDetails_Country_Country3CountryId",
                        column: x => x.Country3CountryId,
                        principalTable: "Country",
                        principalColumn: "CountryId",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_VisitorDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TravelDetails_Country1CountryId",
                table: "TravelDetails",
                column: "Country1CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelDetails_Country2CountryId",
                table: "TravelDetails",
                column: "Country2CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_TravelDetails_Country3CountryId",
                table: "TravelDetails",
                column: "Country3CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorDetails_Country1CountryId",
                table: "VisitorDetails",
                column: "Country1CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorDetails_Country2CountryId",
                table: "VisitorDetails",
                column: "Country2CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_VisitorDetails_Country3CountryId",
                table: "VisitorDetails",
                column: "Country3CountryId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TravelDetails");

            migrationBuilder.DropTable(
                name: "VisitorDetails");
        }
    }
}
