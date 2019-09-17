using System;
using Microsoft.EntityFrameworkCore.Migrations;
using ntbs_service.Models.Enums;

namespace ntbs_service.Migrations
{
    public partial class AddDateAndStatusToNotification : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "NotificationStatus",
                table: "Notification",
                nullable: false,
                defaultValue: NotificationStatus.Draft.ToString());

            migrationBuilder.AddColumn<DateTime>(
                name: "SubmissionDate",
                table: "Notification",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotificationStatus",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "SubmissionDate",
                table: "Notification");
        }
    }
}
