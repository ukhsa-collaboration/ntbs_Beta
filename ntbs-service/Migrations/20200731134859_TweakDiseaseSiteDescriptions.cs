using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class TweakDiseaseSiteDescriptions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 10,
                column: "Description",
                value: "Lymph nodes: Intra-thoracic");

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 11,
                column: "Description",
                value: "Lymph nodes: Extra-thoracic");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 10,
                column: "Description",
                value: "Intra-thoracic");

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 11,
                column: "Description",
                value: "Extra-thoracic");
        }
    }
}
