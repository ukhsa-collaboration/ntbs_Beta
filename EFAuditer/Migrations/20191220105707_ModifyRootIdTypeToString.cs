using Microsoft.EntityFrameworkCore.Migrations;

namespace EFAuditer.Migrations
{
    public partial class ModifyRootIdTypeToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "RootId",
                table: "AuditLogs",
                nullable: true,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "RootId",
                table: "AuditLogs",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
