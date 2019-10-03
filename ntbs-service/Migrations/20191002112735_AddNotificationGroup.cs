using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddNotificationGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "GroupId",
                table: "Notification",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "NotificationGroup",
                columns: table => new
                {
                    NotificationGroupId = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationGroup", x => x.NotificationGroupId);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Notification_GroupId",
                table: "Notification",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_NotificationGroup_GroupId",
                table: "Notification",
                column: "GroupId",
                principalTable: "NotificationGroup",
                principalColumn: "NotificationGroupId",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_NotificationGroup_GroupId",
                table: "Notification");

            migrationBuilder.DropTable(
                name: "NotificationGroup");

            migrationBuilder.DropIndex(
                name: "IX_Notification_GroupId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Notification");
        }
    }
}
