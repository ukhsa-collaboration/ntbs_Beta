using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddUnknownToSitesOfDisease : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Site",
                columns: new[] { "SiteId", "Description" },
                values: new object[] { 18, "Unknown" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 18);
        }
    }
}
