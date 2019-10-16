using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddDenotificationDetailsEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "DenotificationDetails",
                columns: table => new
                {
                    NotificationId = table.Column<int>(nullable: false),
                    DateOfDenotification = table.Column<DateTime>(nullable: false),
                    Reason = table.Column<string>(maxLength: 30, nullable: false),
                    OtherDescription = table.Column<string>(maxLength: 150, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DenotificationDetails", x => x.NotificationId);
                    table.ForeignKey(
                        name: "FK_DenotificationDetails_Notification_NotificationId",
                        column: x => x.NotificationId,
                        principalTable: "Notification",
                        principalColumn: "NotificationId",
                        onDelete: ReferentialAction.Cascade);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DenotificationDetails");
        }
    }
}
