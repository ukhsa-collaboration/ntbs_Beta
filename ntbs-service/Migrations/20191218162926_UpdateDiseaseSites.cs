using Microsoft.EntityFrameworkCore.Migrations;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Migrations
{
    public partial class UpdateDiseaseSites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Remove any 'Unknown' disease sites as we are removing this as an option
            migrationBuilder.Sql("DELETE FROM [NotificationSite] WHERE SiteId = 18");

            migrationBuilder.DeleteData(
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 18);

            migrationBuilder.UpdateData(
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 2,
                column: "Description",
                value: "Spine");

            migrationBuilder.UpdateData(
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 3,
                column: "Description",
                value: "Bone/joint: Other");

            migrationBuilder.UpdateData(
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 4,
                column: "Description",
                value: "Meningitis");

            migrationBuilder.UpdateData(
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 5,
                column: "Description",
                value: "CNS: Other");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 2,
                column: "Description",
                value: "Bone/joint: spine");

            migrationBuilder.UpdateData(
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 3,
                column: "Description",
                value: "Bone/joint: other");

            migrationBuilder.UpdateData(
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 4,
                column: "Description",
                value: "meningitis");

            migrationBuilder.UpdateData(
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 5,
                column: "Description",
                value: "other");

            migrationBuilder.InsertData(
                table: "Site",
                columns: new[] { "SiteId", "Description" },
                values: new object[] { 18, "Unknown" });
        }
    }
}
