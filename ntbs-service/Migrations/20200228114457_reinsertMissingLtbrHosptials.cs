using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class reinsertMissingLtbrHosptials : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hospital",
                columns: new[] { "HospitalId", "CountryCode", "IsLegacy", "Name", "TBServiceCode" },
                values: new object[] { new Guid("fefd7cdd-bdaa-4be8-b839-780a7bb0d7ff"), "E92000001", true, "BROMLEY HOSPITAL", "TBS0029" });

            migrationBuilder.InsertData(
                table: "Hospital",
                columns: new[] { "HospitalId", "CountryCode", "IsLegacy", "Name", "TBServiceCode" },
                values: new object[] { new Guid("6fd71037-5957-4a18-97e7-65efdd524cf7"), "E92000001", true, "TB SERVICE NCL - SOUTH HUB", "TBS0239" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6fd71037-5957-4a18-97e7-65efdd524cf7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fefd7cdd-bdaa-4be8-b839-780a7bb0d7ff"));
        }
    }
}
