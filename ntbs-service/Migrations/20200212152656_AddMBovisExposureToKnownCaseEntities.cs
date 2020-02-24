using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddMBovisExposureToKnownCaseEntities : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MBovisDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    HasExposureToKnownCases = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBovisDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_MBovisDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "MBovisExposureToKnownCase",
                columns: table => new
                {
                    MBovisExposureToKnownCaseId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy",
                            SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationId = table.Column<int>(nullable: false),
                    YearOfExposure = table.Column<int>(nullable: false),
                    ExposureSetting = table.Column<string>(maxLength: 30, nullable: false),
                    ExposureNotificationId = table.Column<int>(nullable: false),
                    OtherDetails = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MBovisExposureToKnownCase", x => x.MBovisExposureToKnownCaseId);
                    table.ForeignKey(
                        name: "FK_MBovisExposureToKnownCase_MBovisDetails_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "MBovisDetails",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TbService_IsLegacy_Name",
                table: "TbService",
                columns: new[] {"IsLegacy", "Name"});

            migrationBuilder.CreateIndex(
                name: "IX_MBovisExposureToKnownCase_NotificationId",
                table: "MBovisExposureToKnownCase",
                column: "NotificationId");

            migrationBuilder.Sql(@"
                INSERT INTO MBovisDetails (NotificationId)
                    SELECT n.NotificationId 
                    FROM Notification n
                    LEFT JOIN MBovisDetails mbovis on n.NotificationId = mbovis.NotificationId 
                    WHERE mbovis.NotificationId IS NULL;
            ");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MBovisExposureToKnownCase");

            migrationBuilder.DropTable(
                name: "MBovisDetails");

            migrationBuilder.DropIndex(
                name: "IX_TbService_IsLegacy_Name",
                table: "TbService");
        }
    }
}
