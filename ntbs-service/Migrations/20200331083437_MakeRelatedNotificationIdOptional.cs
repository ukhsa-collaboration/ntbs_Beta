using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MakeRelatedNotificationIdOptional : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExposureNotificationId",
                table: "MBovisExposureToKnownCase",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ExposureNotificationId",
                table: "MBovisExposureToKnownCase",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
