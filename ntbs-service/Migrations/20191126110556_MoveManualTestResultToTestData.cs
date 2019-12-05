using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MoveManualTestResultToTestData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManualTestResult_Notification_NotificationId",
                table: "ManualTestResult");

            migrationBuilder.AddForeignKey(
                name: "FK_ManualTestResult_TestData_NotificationId",
                table: "ManualTestResult",
                column: "NotificationId",
                principalTable: "TestData",
                principalColumn: "NotificationId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ManualTestResult_TestData_NotificationId",
                table: "ManualTestResult");

            migrationBuilder.AddForeignKey(
                name: "FK_ManualTestResult_Notification_NotificationId",
                table: "ManualTestResult",
                column: "NotificationId",
                principalTable: "Notification",
                principalColumn: "NotificationId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
