using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddUnknownTbServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "TbService",
                columns: new[] { "Code", "IsLegacy", "Name", "PHECCode", "ServiceAdGroup" },
                values: new object[,]
                {
                    { "TBS1001", true, "UNKNOWN", "E45000001", null },
                    { "TBS1002", true, "UNKNOWN", "E45000005", null },
                    { "TBS1003", true, "UNKNOWN", "E45000009", null },
                    { "TBS1004", true, "UNKNOWN", "E45000010", null },
                    { "TBS1005", true, "UNKNOWN", "E45000016", null },
                    { "TBS1006", true, "UNKNOWN", "E45000017", null },
                    { "TBS1007", true, "UNKNOWN", "E45000018", null },
                    { "TBS1008", true, "UNKNOWN", "E45000019", null },
                    { "TBS1009", true, "UNKNOWN", "E45000020", null },
                    { "TBS1010", true, "UNKNOWN", "PHECNI", null },
                    { "TBS1011", true, "UNKNOWN", "PHECSCOT", null },
                    { "TBS1012", true, "UNKNOWN", "PHECWAL", null },
                    { "TBS1013", true, "UNKNOWN", null, null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1001");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1002");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1003");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1004");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1005");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1006");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1007");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1008");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1009");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1010");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1011");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1012");

            migrationBuilder.DeleteData(
                schema: "ReferenceData",
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS1013");
        }
    }
}
