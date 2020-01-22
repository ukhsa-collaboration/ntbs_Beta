using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class ConvertClusterIdToVarchar : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ClusterId",
                table: "Notification",
                nullable: true,
                maxLength: 20,
                oldClrType: typeof(int),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ClusterId",
                table: "Notification",
                nullable: true,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
