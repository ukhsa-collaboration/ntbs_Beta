using Microsoft.EntityFrameworkCore.Migrations;

namespace EFAuditer.Migrations
{
    public partial class ChangeOriginalIdFromIntToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "OriginalId",
                table: "AuditLogs",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "OriginalId",
                table: "AuditLogs",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
