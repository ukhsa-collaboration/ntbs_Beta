using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddOrderIndexToSites : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "OrderIndex",
                schema: "ReferenceData",
                table: "Site",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 1,
                column: "OrderIndex",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 2,
                column: "OrderIndex",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 3,
                column: "OrderIndex",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 4,
                column: "OrderIndex",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 5,
                column: "OrderIndex",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 6,
                column: "OrderIndex",
                value: 13);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 7,
                column: "OrderIndex",
                value: 10);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 8,
                column: "OrderIndex",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 9,
                column: "OrderIndex",
                value: 12);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 10,
                column: "OrderIndex",
                value: 8);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 11,
                column: "OrderIndex",
                value: 9);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 12,
                column: "OrderIndex",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 13,
                column: "OrderIndex",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 14,
                column: "OrderIndex",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 15,
                column: "OrderIndex",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 16,
                column: "OrderIndex",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Site",
                keyColumn: "SiteId",
                keyValue: 17,
                column: "OrderIndex",
                value: 17);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrderIndex",
                schema: "ReferenceData",
                table: "Site");
        }
    }
}
