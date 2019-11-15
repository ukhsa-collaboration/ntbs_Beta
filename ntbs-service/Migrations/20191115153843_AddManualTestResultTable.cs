using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddManualTestResultTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ManualTestResult",
                columns: table => new
                {
                    ManualTestResultId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    NotificationId = table.Column<int>(nullable: false),
                    ManualTestTypeId = table.Column<int>(nullable: false),
                    SampleTypeId = table.Column<int>(nullable: false),
                    Result = table.Column<string>(maxLength: 30, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ManualTestResult", x => x.ManualTestResultId);
                    table.ForeignKey(
                        name: "FK_ManualTestResult_ManualTestType_ManualTestTypeId",
                        column: x => x.ManualTestTypeId,
                        principalTable: "ManualTestType",
                        principalColumn: "ManualTestTypeId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManualTestResult_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ManualTestResult_SampleType_SampleTypeId",
                        column: x => x.SampleTypeId,
                        principalTable: "SampleType",
                        principalColumn: "SampleTypeId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ManualTestResult_ManualTestTypeId",
                table: "ManualTestResult",
                column: "ManualTestTypeId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualTestResult_NotificationId",
                table: "ManualTestResult",
                column: "NotificationId");

            migrationBuilder.CreateIndex(
                name: "IX_ManualTestResult_SampleTypeId",
                table: "ManualTestResult",
                column: "SampleTypeId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ManualTestResult");
        }
    }
}
