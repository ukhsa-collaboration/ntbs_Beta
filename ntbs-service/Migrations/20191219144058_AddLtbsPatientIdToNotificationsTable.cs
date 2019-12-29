using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddLtbsPatientIdToNotificationsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "LTBRPatientId",
                table: "Notification",
                maxLength: 50,
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_LTBRPatientId",
                table: "Notification",
                column: "LTBRPatientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Notification_LTBRPatientId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "LTBRPatientId",
                table: "Notification");
        }
    }
}
