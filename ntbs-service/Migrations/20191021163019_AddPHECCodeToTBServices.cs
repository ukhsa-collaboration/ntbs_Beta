using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddPHECCodeToTBServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "PHECCode",
                table: "TBService",
                maxLength: 50,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0001",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0002",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0003",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0004",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0005",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0006",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0007",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0008",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0009",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0010",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0011",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0012",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0013",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0014",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0015",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0016",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0017",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0018",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0019",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0020",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0021",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0022",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0023",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0024",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0025",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0026",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0027",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0028",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0029",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0030",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0031",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0032",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0033",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0034",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0035",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0036",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0037",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0038",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0039",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0040",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0041",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0042",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0043",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0044",
                column: "PHECCode",
                value: "E45000016");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0045",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0046",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0047",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0048",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0049",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0050",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0051",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0052",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0053",
                column: "PHECCode",
                value: "E45000009");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0054",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0055",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0056",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0057",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0058",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0059",
                column: "PHECCode",
                value: "E45000016");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0060",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0061",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0062",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0063",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0064",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0065",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0066",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0067",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0068",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0069",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0070",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0071",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0072",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0073",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0074",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0075",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0076",
                column: "PHECCode",
                value: "E45000009");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0077",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0078",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0079",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0080",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0081",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0082",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0083",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0084",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0085",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0086",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0087",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0088",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0089",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0090",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0091",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0092",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0093",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0094",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0095",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0096",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0097",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0098",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0099",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0100",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0101",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0102",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0103",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0104",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0105",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0106",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0107",
                column: "PHECCode",
                value: "E45000016");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0108",
                column: "PHECCode",
                value: "E45000016");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0109",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0110",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0111",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0112",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0113",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0114",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0115",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0116",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0117",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0118",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0119",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0120",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0121",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0122",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0123",
                column: "PHECCode",
                value: "E45000016");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0124",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0125",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0126",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0127",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0128",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0129",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0130",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0131",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0132",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0133",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0134",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0135",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0136",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0137",
                column: "PHECCode",
                value: "E45000009");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0138",
                column: "PHECCode",
                value: "E45000009");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0139",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0140",
                column: "PHECCode",
                value: "E45000016");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0141",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0142",
                column: "PHECCode",
                value: "E45000016");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0143",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0144",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0145",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0146",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0147",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0148",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0149",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0150",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0151",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0152",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0153",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0154",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0155",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0156",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0157",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0158",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0159",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0160",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0161",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0162",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0163",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0164",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0165",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0166",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0167",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0168",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0169",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0170",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0171",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0172",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0173",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0174",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0175",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0176",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0177",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0178",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0179",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0180",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0181",
                column: "PHECCode",
                value: "E45000016");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0182",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0183",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0184",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0185",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0186",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0187",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0188",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0189",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0190",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0191",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0192",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0193",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0194",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0195",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0196",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0197",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0198",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0199",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0200",
                column: "PHECCode",
                value: "E45000009");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0201",
                column: "PHECCode",
                value: "E45000009");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0202",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0203",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0204",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0205",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0206",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0207",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0208",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0209",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0210",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0211",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0212",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0213",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0214",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0215",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0216",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0217",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0218",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0219",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0220",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0221",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0222",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0223",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0224",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0225",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0226",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0227",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0228",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0229",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0230",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0231",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0232",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0233",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0234",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0235",
                column: "PHECCode",
                value: "E45000009");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0236",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0237",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0238",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0239",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0240",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0241",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0242",
                column: "PHECCode",
                value: "E45000010");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0243",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0244",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0245",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0246",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0247",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0248",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0249",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0250",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0251",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0252",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0253",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0254",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0255",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0256",
                column: "PHECCode",
                value: "E45000017");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0257",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0258",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0259",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0260",
                column: "PHECCode",
                value: "E45000001");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0261",
                column: "PHECCode",
                value: "E45000018");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0262",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0263",
                column: "PHECCode",
                value: "E45000005");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0264",
                column: "PHECCode",
                value: "E45000019");

            migrationBuilder.UpdateData(
                table: "TBService",
                keyColumn: "Code",
                keyValue: "TBS0265",
                column: "PHECCode",
                value: "E45000020");

            migrationBuilder.CreateIndex(
                name: "IX_TBService_PHECCode",
                table: "TBService",
                column: "PHECCode");

            migrationBuilder.AddForeignKey(
                name: "FK_TBService_PHEC_PHECCode",
                table: "TBService",
                column: "PHECCode",
                principalTable: "PHEC",
                principalColumn: "Code",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TBService_PHEC_PHECCode",
                table: "TBService");

            migrationBuilder.DropIndex(
                name: "IX_TBService_PHECCode",
                table: "TBService");

            migrationBuilder.DropColumn(
                name: "PHECCode",
                table: "TBService");
        }
    }
}
