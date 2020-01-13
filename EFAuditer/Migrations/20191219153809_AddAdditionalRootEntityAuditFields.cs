using Microsoft.EntityFrameworkCore.Migrations;

namespace EFAuditer.Migrations
{
    public partial class AddAdditionalRootEntityAuditFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "RootEntity",
                table: "AuditLogs",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RootId",
                table: "AuditLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RootEntity",
                table: "AuditLogs");

            migrationBuilder.DropColumn(
                name: "RootId",
                table: "AuditLogs");
        }
    }
}
