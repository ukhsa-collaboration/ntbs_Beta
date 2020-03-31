using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class MakeNotifiedToPheStatusRequired : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                UPDATE MBovisExposureToKnownCase
                SET NotifiedToPheStatus = 'Unknown'
                WHERE NotifiedToPheStatus IS NULL");
            
            migrationBuilder.AlterColumn<string>(
                name: "NotifiedToPheStatus",
                table: "MBovisExposureToKnownCase",
                maxLength: 30,
                nullable: false,
                oldClrType: typeof(string),
                oldMaxLength: 30,
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "NotifiedToPheStatus",
                table: "MBovisExposureToKnownCase",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldMaxLength: 30);
        }
    }
}
