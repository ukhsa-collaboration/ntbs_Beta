using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class UpdateSouthWestAcuteHospitalData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "TbService",
                columns: new[] { "Code", "IsLegacy", "Name", "PHECCode", "ServiceAdGroup" },
                values: new object[] { "TBS1000", false, "SOUTH WEST ACUTE HOSPITAL", "PHECNI", null });

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b93a5546-6c26-4a4b-a2fa-427b2711b861"),
                columns: new[] { "CountryCode", "TBServiceCode" },
                values: new object[] { "N92000002", "TBS1000" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1000");

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b93a5546-6c26-4a4b-a2fa-427b2711b861"),
                columns: new[] { "CountryCode", "TBServiceCode" },
                values: new object[] { "E92000001", "TBS0123" });
        }
    }
}
