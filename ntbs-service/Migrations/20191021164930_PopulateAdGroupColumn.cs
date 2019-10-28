using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class PopulateAdGroupColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000001",
                column: "AdGroup",
                value: "Global.NIS.NTBS.LON");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000005",
                column: "AdGroup",
                value: "Global.NIS.NTBS.WMS");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000009",
                column: "AdGroup",
                value: "Global.NIS.NTBS.NoE");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000010",
                column: "AdGroup",
                value: "Global.NIS.NTBS.YHR");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000016",
                column: "AdGroup",
                value: "Global.NIS.NTBS.EMS");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000017",
                column: "AdGroup",
                value: "Global.NIS.NTBS.EoE");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000018",
                column: "AdGroup",
                value: "Global.NIS.NTBS.NoW");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000019",
                column: "AdGroup",
                value: "Global.NIS.NTBS.SoE");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000020",
                column: "AdGroup",
                value: "Global.NIS.NTBS.SoW");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "PHECNI",
                column: "AdGroup",
                value: "");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "PHECSCOT",
                column: "AdGroup",
                value: "");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "PHECWAL",
                column: "AdGroup",
                value: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000001",
                column: "AdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000005",
                column: "AdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000009",
                column: "AdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000010",
                column: "AdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000016",
                column: "AdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000017",
                column: "AdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000018",
                column: "AdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000019",
                column: "AdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "E45000020",
                column: "AdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "PHECNI",
                column: "AdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "PHECSCOT",
                column: "AdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "PHECWAL",
                column: "AdGroup",
                value: null);
        }
    }
}
