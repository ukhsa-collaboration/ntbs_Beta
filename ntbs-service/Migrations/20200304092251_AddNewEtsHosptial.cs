using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddNewEtsHosptial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Hospital",
                columns: new[] { "HospitalId", "CountryCode", "IsLegacy", "Name", "TBServiceCode" },
                values: new object[] { new Guid("b93a5546-6c26-4a4b-a2fa-427b2711b861"), "E92000001", false, "SOUTH WEST ACUTE HOSPITAL", "TBS0123" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b93a5546-6c26-4a4b-a2fa-427b2711b861"));
        }
    }
}
