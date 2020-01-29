using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class UpdateReferenceDataForAdGroupsAndNewServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6fd71037-5957-4a18-97e7-65efdd524cf7"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("fefd7cdd-bdaa-4be8-b839-780a7bb0d7ff"));

            migrationBuilder.UpdateData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2d19c818-ba9d-4c38-baf4-42cf14844619"),
                column: "Name",
                value: "LIVERPOOL WOMENS HOSPITAL");

            migrationBuilder.UpdateData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d473c823-ec99-43f8-ae8f-23eb332b1b29"),
                column: "Name",
                value: "SPIRE LONGLANDS CONSULTING ROOMS");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "PHECNI",
                column: "AdGroup",
                value: "Global.NIS.NTBS.NI");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "PHECSCOT",
                column: "AdGroup",
                value: "Global.NIS.NTBS.SCOT");

            migrationBuilder.UpdateData(
                table: "PHEC",
                keyColumn: "Code",
                keyValue: "PHECWAL",
                column: "AdGroup",
                value: "Global.NIS.NTBS.WALES");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0001",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Abingdon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0002",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Addenbrooke");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0003",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Airedale");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0004",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Amersham");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0005",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Andover");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0006",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Arrowe");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0007",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_AshfordStPeter");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0008",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Ashford");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0009",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Barnet");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0010",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Basildon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0011",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Baskingstoke");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0012",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bath");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0013",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Battle");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0014",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bedford");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0015",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Benenden");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0016",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Berkshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0017",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Beverley");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0018",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_BHRUniversity");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0019",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Birmingham");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0020",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Blackheath");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0021",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bognor");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0022",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_BradfordRoyal");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0023",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_BradfordTeaching");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0024",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Brentwood");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0025",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bridgewater");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0026",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bridlington");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0027",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Brighton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0028",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bristol");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0029",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bromley");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0030",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Broomfield");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0031",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Buckingham");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0032",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Buckland");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0033",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Burton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0034",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Calderdale");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0035",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Cavell");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0036",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Lancashire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0037",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Charter");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0038",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Chase");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0039",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Chaucer");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0040",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Hull");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0041",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Chelsea");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0042",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Chelsfield");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0043",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Cheshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0044",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Chesterfield");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0045",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Chiltern");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0046",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_ChurchHill");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0047",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Clacton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0048",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Clementine");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0049",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Colchester");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0050",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Conquest");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0051",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Cornwall");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0052",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_CountessOfChester");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0053",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Durham");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0054",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Coventry");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0055",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Crawley");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0056",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_MorecambeBay");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0057",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NorthCumbria");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0058",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_DarentValley");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0059",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Doncaster");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0060",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Dorset");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0061",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_DuchessOfKent");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0062",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Dudley");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0063",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_ENHerts");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0064",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_EastDorset");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0065",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_EastLancs");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0066",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_EastSurrey");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0067",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Eastbourne");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0068",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Epsom");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0069",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Erith");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0070",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Esperance");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0071",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Essex");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0072",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Fawkham");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0073",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Fishermead");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0074",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Fitzwilliam");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0075",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Frimley");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0076",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Gateshead");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0077",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Gloucestershire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0078",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Goole");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0079",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Gravesend");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0080",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_GreatOrmond");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0081",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Guys");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0082",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Halton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0083",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Hampshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0084",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Harefield");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0085",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_HaroldWood");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0086",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Harrogate");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0087",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Heart");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0088",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Heatherwood");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0089",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Herefordshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0090",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Hillingdon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0091",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_HollyHouse");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0092",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Homerton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0093",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Horsham");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0094",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Tropical");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0095",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_ImperialCollege");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0096",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Ipswich");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0097",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_JamesPaget");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0098",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_JoyceGreen");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0099",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_KentSussex");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0100",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_KentCommunity");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0101",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_KingsCollege");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0102",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_KingsCollegeDulwich");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0103",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_KingsOak");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0104",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_KingsPark");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0105",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Kingston");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0106",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Leeds");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0107",
                columns: new[] { "Name", "ServiceAdGroup" },
                values: new object[] { "Leicester Royal Infirmary", "Global.NIS.NTBS.Service_Leicester" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0108",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Lincolnshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0109",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Liverpool");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0110",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Huddersfield");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0111",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Kirklees");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0112",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_LondonIndependent");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0113",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Middelsex");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0114",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Ealing");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0115",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NorthwickPark");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0116",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Luton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0117",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Lymington");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0118",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Maidstone");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0119",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Manchester");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0120",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Mayday");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0121",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Medway");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0122",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_MemorialHospital");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0123",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_MidNotts");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0124",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_MileEnd");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0125",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_MiltonKeynes");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0126",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_MountAlvernia");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0127",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_MountVernon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0128",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Neurology");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0129",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Nelson");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0130",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Newham");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0131",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Croydon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0132",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Norfolk");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0133",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NorthDevon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0134",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NorthDowns");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0135",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NorthHampshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0136",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NorthMiddlesex");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0137",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NorthOfTyne");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0138",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NorthTees");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0139",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NorthWestAnglia");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0140",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Northamptonshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0141",
                columns: new[] { "Name", "ServiceAdGroup" },
                values: new object[] { "Northern Lincolnshire & Goole NHS Foundation Trust ", "Global.NIS.NTBS.Service_NorthLincolnshire" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0142",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Nottingham");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0143",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NuffieldBrentwood");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0144",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NuffieldCambridge");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0145",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NuffieldChichester");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0146",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NuffieldGuildford");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0147",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NuffieldHaywards");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0148",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NuffieldLeeds");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0149",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NuffieldTunbridge");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0150",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NuffieldWoking");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0151",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NuffieldYork");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0152",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Oldchurch");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0153",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_OpenDoor");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0154",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Ormskirk");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0155",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Orpington");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0156",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Orsett");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0157",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Oxford");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0158",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Papworth");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0159",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Parkside");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0160",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Pennine");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0161",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Pinehill");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0162",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Pontefract");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0163",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Portland");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0164",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Preston");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0165",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_PrincessAlexandra");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0166",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_PrincessRoyalSussex");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0167",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Priory");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0168",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_QueenAlexandra");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0169",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_QueenElizabeth");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0170",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_QueenElizabethKingsLynn");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0171",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_QueenMaryLondon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0172",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_QueenMarySidcup");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0173",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_QueenVictoriaGrinstead");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0174",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Radcliffe");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0175",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Devon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0176",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RDASH");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0177",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalBerkshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0178",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bolton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0179",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Brompton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0180",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Buckinghamshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0181",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Derby");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0182",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalFree");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0183",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalHampshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0184",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalHaslar");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0185",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalLondon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0186",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalMarsden");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0187",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Orthopaedic");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0188",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalSouthHants");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0189",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalSurreyCounty");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0190",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalSussexCounty");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0191",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalVictoriaFolkestone");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0192",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Runneymede");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0193",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Salford");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0194",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Sandwell");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0195",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Sheffield");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0196",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Shropshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0197",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Sloane");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0198",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Somerfield");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0199",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Somerset");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0200",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SouthTees");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0201",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SouthTyneside");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0202",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Barnsley");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0203",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Wakefield");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0204",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Southampton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0205",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Southend");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0206",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Southport");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0207",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SpireAlexandra");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0208",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SpireHarpenden");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0209",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SpireHartswood");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0210",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SpireLea");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0211",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SpireLonglands");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0212",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SpireSaviours");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0213",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SpireTunbridge");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0214",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SpireWellesley");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0215",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StAlbans");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0216",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StAnnsLondon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0217",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StAnthonys");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0218",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_BartsLondon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0219",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_BartsRochester");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0220",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StGeorge");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0221",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StGeorgeStafford");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0222",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StHelenMerseyside");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0223",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StJamesSouthsea");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0224",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StJohnElizabeth");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0225",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StMaryWight");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0226",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StMaryPortsmouth");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0227",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StMichael");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0228",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StPancras");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0229",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StRichard");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0230",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StThomas");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0231",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StMargaret");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0232",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Staffordshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0233",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Stepping");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0234",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StokeMandeville");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0235",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Sunderland");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0236",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Swindon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0237",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Wiltshire");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0238",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Tameside");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0239",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SouthHub");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0240",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000010", "Global.NIS.NTBS.Service_NULL" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0241",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Rotherham");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0242",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000020", "Global.NIS.NTBS.Service_TickhillRoad" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0243",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", "Global.NIS.NTBS.Service_Torbay" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0244",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Townlands");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0245",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000001", "Global.NIS.NTBS.Service_TunbridgePembury" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0246",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", "Global.NIS.NTBS.Service_Lewisham" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0247",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000001", "Global.NIS.NTBS.Service_Slough" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0248",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000005", "Global.NIS.NTBS.Service_Romford" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0249",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", "Global.NIS.NTBS.Service_Walsall" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0250",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000018", "Global.NIS.NTBS.Service_Warders" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0251",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000017", "Global.NIS.NTBS.Service_Warrington" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0252",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_WaterEaton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0253",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000001", "Global.NIS.NTBS.Service_WestHerts" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0254",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", "Global.NIS.NTBS.Service_WestMiddlesex" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0255",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000017", "Global.NIS.NTBS.Service_WestPark" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0256",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", "Global.NIS.NTBS.Service_WestSuffolk" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0257",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000020", "Global.NIS.NTBS.Service_Western" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0258",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", "Global.NIS.NTBS.Service_Weston" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0259",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000001", "Global.NIS.NTBS.Service_Wexham" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0260",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000018", "Global.NIS.NTBS.Service_WhippsCross" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0261",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000005", "Global.NIS.NTBS.Service_Whiston" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0262",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Wolverhampton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0263",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", "Global.NIS.NTBS.Service_Worcestershire" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0264",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000020", "Global.NIS.NTBS.Service_Worthing" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0265",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Yeovil");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0266",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "PHECNI", "Global.NIS.NTBS.Service_Plymouth" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0267",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_DaisyHill");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0268",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_MusgravePark");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0269",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Whiteabbey");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0270",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Ards");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0271",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_MaterInfirmorum");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0272",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalVictoriaBelfast");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0273",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Craigavon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0274",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Causeway");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0275",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SouthTyrone");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0276",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Antrim");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0277",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Belfast");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0278",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Lagan");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0279",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Erne");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0280",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Coleraine");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0281",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Ulster");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0282",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Tyrone");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0283",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Downe");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0284",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalBelfast");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0285",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bangor");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0286",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Altnagelvin");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0287",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "PHECWAL", "Global.NIS.NTBS.Service_MidUlster" });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0288",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Werndale");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0289",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Havenway");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0290",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Lluesty");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0291",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Ystradgynlais");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0292",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Tredegar");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0293",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Groeswen");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0294",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Chirk");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0295",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bronllys");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0296",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Oakdale");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0297",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Newport");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0298",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Gellinudd");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0299",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Clydach");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0300",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Pontypridd");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0301",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Cefni");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0302",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Caernarfon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0303",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_PortTalbot");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0304",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_GlanClwyd");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0305",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Stanley");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0306",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalGlamorgan");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0307",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Llwyneryr");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0308",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NantYGlyn");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0309",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Morriston");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0310",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SpireYale");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0311",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Llandudno");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0312",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SwnYGwynt");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0313",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_ColwynBay");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0314",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StTydfils");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0315",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Dolgellau");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0316",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Aberdare");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0317",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_YsbytyGeorge");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0318",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_BronYGarth");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0319",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StDavidCardiff");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0320",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Gwynedd");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0321",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Llandovery");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0322",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Builth");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0323",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Ffestiniog");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0324",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Forglen");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0325",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_PrincessWales");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0326",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StWoolos");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0327",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalAlexRhyl");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0328",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Blaina");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0329",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Whitchurch");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0330",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SpireCardiff");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0331",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StCadocs");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0332",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Llanrwst");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0333",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Garngoch");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0334",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Llanrhaedr");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0335",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Redwood");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0336",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_ParkSquare");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0337",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Wrexham");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0338",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Tywyn");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0339",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_VictoriaPowys");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0340",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_AmyEvans");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0341",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_PrincePhilip");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0342",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StDavidCarmarthen");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0343",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Deeside");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0344",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Cardiff");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0345",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Brynmawr");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0346",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Maesgwyn");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0347",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Mold");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0348",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Mynydd");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0349",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Monmouth");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0350",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Aberystwyth");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0351",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Westfa");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0352",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_BroDdyfi");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0353",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Pwllheli");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0354",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Tenby");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0355",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_HillHouse");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0356",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_CefnCoed");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0357",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Aberaeron");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0358",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Tonna");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0359",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Ystrad");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0360",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Amman");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0361",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Fairwood");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0362",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Caerphilly");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0363",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Holywell");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0364",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Llwynypia");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0365",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Brynseiont");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0366",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Llanfrechfa");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0367",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Hafen");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0368",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Aberbargoed");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0369",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Tregaron");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0370",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Rookwood");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0371",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Llandough");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0372",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Penrhos");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0373",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Llangollen");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0374",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Montgomery");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0375",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Gorseinon");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0376",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Ruthin");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0377",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Chepstow");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0378",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bryn");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0379",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_StJoseph");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0380",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_DewiSant");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0381",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_TriChwm");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0382",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Prestatyn");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0383",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_SouthPembroke");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0384",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Penley");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0385",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Pontypool");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0386",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Minfordd");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0387",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_NevillHall");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0388",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Cymla");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0389",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Withybush");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0390",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_MountainAsh");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0391",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Maindiff");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0392",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Eryri");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0393",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Maesteg");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0394",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Abertillery");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0395",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_RoyalGwent");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0396",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Glanrhyd");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0397",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_BrynBeryl");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0398",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_WestWales");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0399",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Denbigh");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0400",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Velindre");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0401",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_UniversityWales");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0402",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Overmonnow");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0403",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Abergele");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0404",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Singleton");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0405",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Bronglais");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0406",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Cardigan");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0407",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Flint");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0408",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_Barry");

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0409",
                column: "ServiceAdGroup",
                value: "Global.NIS.NTBS.Service_PrinceCharles");

            migrationBuilder.InsertData(
                table: "TbService",
                columns: new[] { "Code", "Name", "PHECCode", "ServiceAdGroup" },
                values: new object[,]
                {
                    { "TBS0421", "NHS Shetland", "PHECSCOT", null },
                    { "TBS0420", "NHS Orkney", "PHECSCOT", null },
                    { "TBS0419", "NHS Lothian", "PHECSCOT", null },
                    { "TBS0418", "NHS Lanarkshire", "PHECSCOT", null },
                    { "TBS0417", "NHS Highland", "PHECSCOT", null },
                    { "TBS0416", "NHS Greater Glasgow and Clyde", "PHECSCOT", null },
                    { "TBS0410", "NHS Ayrshire and Arran", "PHECSCOT", null },
                    { "TBS0414", "NHS Forth Valley", "PHECSCOT", null },
                    { "TBS0413", "NHS Fife", "PHECSCOT", null },
                    { "TBS0412", "NHS Dumfries and Galloway", "PHECSCOT", null },
                    { "TBS0411", "NHS Borders", "PHECSCOT", null },
                    { "TBS0422", "NHS Tayside", "PHECSCOT", null },
                    { "TBS0415", "NHS Grampian", "PHECSCOT", null },
                    { "TBS0423", "NHS Western Isles", "PHECSCOT", null }
                });

            migrationBuilder.InsertData(
                table: "Hospital",
                columns: new[] { "HospitalId", "CountryCode", "Name", "TBServiceCode" },
                values: new object[,]
                {
                    { new Guid("b140f9a6-c080-4fed-a992-ac7dd71e20dc"), "S92000003", "NHS AYRSHIRE AND ARRAN", "TBS0410" },
                    { new Guid("af410a50-e52c-4fa3-8a07-c0eb63f5272f"), "S92000003", "NHS BORDERS", "TBS0411" },
                    { new Guid("b7e758b3-2cdd-436c-9834-24263f901b8a"), "S92000003", "NHS DUMFRIES AND GALLOWAY", "TBS0412" },
                    { new Guid("8ae267ca-0751-4b82-b37f-2bc95ac06672"), "S92000003", "NHS FIFE", "TBS0413" },
                    { new Guid("d4042531-c128-4b28-be30-394a487f3d2f"), "S92000003", "NHS FORTH VALLEY", "TBS0414" },
                    { new Guid("8d22fbaf-33a5-4060-a61a-abec1153390d"), "S92000003", "NHS GRAMPIAN", "TBS0415" },
                    { new Guid("83ab54b0-8fd8-46d9-9007-d641d575a4ac"), "S92000003", "NHS GREATER GLASGOW AND CLYDE", "TBS0416" },
                    { new Guid("d6dd3782-f4c3-4192-a99a-bbfd351d1a66"), "S92000003", "NHS HIGHLAND", "TBS0417" },
                    { new Guid("6414552c-64a4-4309-aec5-d6c6d55b2144"), "S92000003", "NHS LANARKSHIRE", "TBS0418" },
                    { new Guid("55cd6665-0b69-4a2d-b8a0-26db6398248b"), "S92000003", "NHS LOTHIAN", "TBS0419" },
                    { new Guid("44ead93f-6b3e-4873-9c4b-b590dd2287cf"), "S92000003", "NHS ORKNEY", "TBS0420" },
                    { new Guid("7d77c176-c394-4f85-880e-1b76dca15c19"), "S92000003", "NHS SHETLAND", "TBS0421" },
                    { new Guid("2d0e397a-0e17-46fc-8fd5-16101c1cc978"), "S92000003", "NHS TAYSIDE", "TBS0422" },
                    { new Guid("8e0bca35-8ae9-49d3-b86b-0d5eda72037f"), "S92000003", "NHS WESTERN ISLES", "TBS0423" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2d0e397a-0e17-46fc-8fd5-16101c1cc978"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("44ead93f-6b3e-4873-9c4b-b590dd2287cf"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("55cd6665-0b69-4a2d-b8a0-26db6398248b"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("6414552c-64a4-4309-aec5-d6c6d55b2144"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("7d77c176-c394-4f85-880e-1b76dca15c19"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("83ab54b0-8fd8-46d9-9007-d641d575a4ac"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8ae267ca-0751-4b82-b37f-2bc95ac06672"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8d22fbaf-33a5-4060-a61a-abec1153390d"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("8e0bca35-8ae9-49d3-b86b-0d5eda72037f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("af410a50-e52c-4fa3-8a07-c0eb63f5272f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b140f9a6-c080-4fed-a992-ac7dd71e20dc"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("b7e758b3-2cdd-436c-9834-24263f901b8a"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d4042531-c128-4b28-be30-394a487f3d2f"));

            migrationBuilder.DeleteData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d6dd3782-f4c3-4192-a99a-bbfd351d1a66"));

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0410");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0411");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0412");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0413");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0414");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0415");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0416");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0417");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0418");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0419");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0420");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0421");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0422");

            migrationBuilder.DeleteData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0423");

            migrationBuilder.UpdateData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("2d19c818-ba9d-4c38-baf4-42cf14844619"),
                column: "Name",
                value: "LIVERPOOL WOMANS HOSPITAL");

            migrationBuilder.UpdateData(
                table: "Hospital",
                keyColumn: "HospitalId",
                keyValue: new Guid("d473c823-ec99-43f8-ae8f-23eb332b1b29"),
                column: "Name",
                value: "SPIRE LONGLANDS CONSULTING ROOMS?");

            migrationBuilder.InsertData(
                table: "Hospital",
                columns: new[] { "HospitalId", "CountryCode", "Name", "TBServiceCode" },
                values: new object[,]
                {
                    { new Guid("fefd7cdd-bdaa-4be8-b839-780a7bb0d7ff"), "E92000001", "BROMLEY HOSPITAL", "TBS0029" },
                    { new Guid("6fd71037-5957-4a18-97e7-65efdd524cf7"), "E92000001", "TB SERVICE NCL - SOUTH HUB", "TBS0239" }
                });

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

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0001",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0002",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0003",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0004",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0005",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0006",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0007",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0008",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0009",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0010",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0011",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0012",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0013",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0014",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0015",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0016",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0017",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0018",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0019",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0020",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0021",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0022",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0023",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0024",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0025",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0026",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0027",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0028",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0029",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0030",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0031",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0032",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0033",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0034",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0035",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0036",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0037",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0038",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0039",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0040",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0041",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0042",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0043",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0044",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0045",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0046",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0047",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0048",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0049",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0050",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0051",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0052",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0053",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0054",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0055",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0056",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0057",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0058",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0059",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0060",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0061",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0062",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0063",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0064",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0065",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0066",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0067",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0068",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0069",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0070",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0071",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0072",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0073",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0074",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0075",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0076",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0077",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0078",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0079",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0080",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0081",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0082",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0083",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0084",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0085",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0086",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0087",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0088",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0089",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0090",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0091",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0092",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0093",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0094",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0095",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0096",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0097",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0098",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0099",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0100",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0101",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0102",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0103",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0104",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0105",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0106",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0107",
                columns: new[] { "Name", "ServiceAdGroup" },
                values: new object[] { "Leicester, Leicestershire & Rutland TB Service", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0108",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0109",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0110",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0111",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0112",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0113",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0114",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0115",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0116",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0117",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0118",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0119",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0120",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0121",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0122",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0123",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0124",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0125",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0126",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0127",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0128",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0129",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0130",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0131",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0132",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0133",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0134",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0135",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0136",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0137",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0138",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0139",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0140",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0141",
                columns: new[] { "Name", "ServiceAdGroup" },
                values: new object[] { "Northern Lincolnshire & Goole NHS Foundation Trust", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0142",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0143",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0144",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0145",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0146",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0147",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0148",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0149",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0150",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0151",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0152",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0153",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0154",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0155",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0156",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0157",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0158",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0159",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0160",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0161",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0162",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0163",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0164",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0165",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0166",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0167",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0168",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0169",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0170",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0171",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0172",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0173",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0174",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0175",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0176",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0177",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0178",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0179",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0180",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0181",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0182",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0183",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0184",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0185",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0186",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0187",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0188",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0189",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0190",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0191",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0192",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0193",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0194",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0195",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0196",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0197",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0198",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0199",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0200",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0201",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0202",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0203",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0204",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0205",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0206",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0207",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0208",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0209",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0210",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0211",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0212",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0213",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0214",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0215",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0216",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0217",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0218",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0219",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0220",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0221",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0222",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0223",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0224",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0225",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0226",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0227",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0228",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0229",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0230",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0231",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0232",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0233",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0234",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0235",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0236",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0237",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0238",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0239",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0240",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { null, null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0241",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0242",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000010", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0243",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000020", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0244",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0245",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0246",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000001", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0247",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0248",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000001", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0249",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000005", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0250",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0251",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000018", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0252",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0253",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000017", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0254",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000001", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0255",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0256",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000017", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0257",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0258",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000020", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0259",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0260",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000001", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0261",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000018", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0262",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0263",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000005", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0264",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000019", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0265",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0266",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "E45000020", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0267",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0268",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0269",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0270",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0271",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0272",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0273",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0274",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0275",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0276",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0277",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0278",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0279",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0280",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0281",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0282",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0283",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0284",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0285",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0286",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0287",
                columns: new[] { "PHECCode", "ServiceAdGroup" },
                values: new object[] { "PHECNI", null });

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0288",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0289",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0290",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0291",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0292",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0293",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0294",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0295",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0296",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0297",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0298",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0299",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0300",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0301",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0302",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0303",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0304",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0305",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0306",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0307",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0308",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0309",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0310",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0311",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0312",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0313",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0314",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0315",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0316",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0317",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0318",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0319",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0320",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0321",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0322",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0323",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0324",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0325",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0326",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0327",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0328",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0329",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0330",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0331",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0332",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0333",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0334",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0335",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0336",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0337",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0338",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0339",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0340",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0341",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0342",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0343",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0344",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0345",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0346",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0347",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0348",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0349",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0350",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0351",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0352",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0353",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0354",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0355",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0356",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0357",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0358",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0359",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0360",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0361",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0362",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0363",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0364",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0365",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0366",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0367",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0368",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0369",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0370",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0371",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0372",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0373",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0374",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0375",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0376",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0377",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0378",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0379",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0380",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0381",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0382",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0383",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0384",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0385",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0386",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0387",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0388",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0389",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0390",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0391",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0392",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0393",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0394",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0395",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0396",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0397",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0398",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0399",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0400",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0401",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0402",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0403",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0404",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0405",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0406",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0407",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0408",
                column: "ServiceAdGroup",
                value: null);

            migrationBuilder.UpdateData(
                table: "TbService",
                keyColumn: "Code",
                keyValue: "TBS0409",
                column: "ServiceAdGroup",
                value: null);
        }
    }
}
