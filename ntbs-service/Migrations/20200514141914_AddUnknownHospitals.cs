using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddUnknownHospitals : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Hospital",
                columns: new[] { "HospitalId", "CountryCode", "IsLegacy", "Name", "TBServiceCode" },
                values: new object[,]
                {
                    { new Guid("41751ac7-1b68-4664-a59f-0e81855256d5"), "E92000001", true, "UNKNOWN", "TBS1001" },
                    { new Guid("f0a1cf54-a92a-4264-9811-f9d6c6f4e8b3"), "E92000001", true, "UNKNOWN", "TBS1002" },
                    { new Guid("8089c3c9-b6a6-4ada-a1e8-25cd232c535a"), "E92000001", true, "UNKNOWN", "TBS1003" },
                    { new Guid("594f3942-ea11-4dfb-a5b5-4c3c644fed5e"), "E92000001", true, "UNKNOWN", "TBS1004" },
                    { new Guid("1468f45e-5808-44e1-bdcc-fa4133e31f71"), "E92000001", true, "UNKNOWN", "TBS1005" },
                    { new Guid("bee2702b-602a-48e0-8e86-d251ac3748db"), "E92000001", true, "UNKNOWN", "TBS1006" },
                    { new Guid("d7fc290f-90e0-4251-b68d-3d22ba4f500c"), "E92000001", true, "UNKNOWN", "TBS1007" },
                    { new Guid("7f554a3f-daec-4ca8-85b8-4ae968b7ad22"), "E92000001", true, "UNKNOWN", "TBS1008" },
                    { new Guid("8d22e664-cfdd-48a1-861c-7cd15710282c"), "E92000001", true, "UNKNOWN", "TBS1009" },
                    { new Guid("7d6a40cf-97b6-4ee4-9e37-4735fdda5def"), "N92000002", true, "UNKNOWN", "TBS1010" },
                    { new Guid("c9fe3b5b-11f9-40ac-908b-2c8a43d908bd"), "S92000003", true, "UNKNOWN", "TBS1011" },
                    { new Guid("48fc350f-a167-42e2-8260-280c29a7553a"), "W92000004", true, "UNKNOWN", "TBS1012" },
                    { new Guid("d6adb6de-6f78-4998-a5d8-672c7ebfcd7c"), "", true, "UNKNOWN", "TBS1013" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("1468f45e-5808-44e1-bdcc-fa4133e31f71"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("41751ac7-1b68-4664-a59f-0e81855256d5"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("48fc350f-a167-42e2-8260-280c29a7553a"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("594f3942-ea11-4dfb-a5b5-4c3c644fed5e"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7d6a40cf-97b6-4ee4-9e37-4735fdda5def"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7f554a3f-daec-4ca8-85b8-4ae968b7ad22"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8089c3c9-b6a6-4ada-a1e8-25cd232c535a"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8d22e664-cfdd-48a1-861c-7cd15710282c"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("bee2702b-602a-48e0-8e86-d251ac3748db"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("c9fe3b5b-11f9-40ac-908b-2c8a43d908bd"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d6adb6de-6f78-4998-a5d8-672c7ebfcd7c"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d7fc290f-90e0-4251-b68d-3d22ba4f500c"));

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("f0a1cf54-a92a-4264-9811-f9d6c6f4e8b3"));
        }
    }
}
