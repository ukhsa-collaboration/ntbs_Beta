using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddNotifiedToPHEStatusToMbovis : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "YearOfExposure",
                table: "MBovisExposureToKnownCase",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<string>(
                name: "NotifiedToPheStatus",
                table: "MBovisExposureToKnownCase",
                maxLength: 30,
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NotifiedToPheStatus",
                table: "MBovisExposureToKnownCase");

            migrationBuilder.AlterColumn<int>(
                name: "YearOfExposure",
                table: "MBovisExposureToKnownCase",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);
        }
    }
}
