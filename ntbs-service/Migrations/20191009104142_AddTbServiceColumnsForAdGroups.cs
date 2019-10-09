using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddTbServiceColumnsForAdGroups : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PHECAdGroup",
                table: "TBService",
                maxLength: 64,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ServiceAdGroup",
                table: "TBService",
                maxLength: 64,
                nullable: true);
            
            migrationBuilder.CreateIndex(
                name: "IX_TBService_ServiceAdGroup",
                table: "TBService",
                column: "ServiceAdGroup",
                unique: true,
                filter: "[ServiceAdGroup] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_TBService_ServiceAdGroup",
                table: "TBService");

            migrationBuilder.DropColumn(
                name: "PHECAdGroup",
                table: "TBService");

            migrationBuilder.DropColumn(
                name: "ServiceAdGroup",
                table: "TBService");
        }
    }
}
