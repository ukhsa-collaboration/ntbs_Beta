using Microsoft.EntityFrameworkCore.Migrations;

namespace EFAuditer.Migrations
{
    public partial class AddAuditDetailsField : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuditDetails",
                table: "AuditLogs",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AuditDetails",
                table: "AuditLogs");
        }
    }
}
