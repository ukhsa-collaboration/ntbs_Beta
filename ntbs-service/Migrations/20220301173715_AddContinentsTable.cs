using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ntbs_service.Migrations
{
    public partial class AddContinentsTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ContinentId",
                schema: "ReferenceData",
                table: "Country",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "Continent",
                schema: "ReferenceData",
                columns: table => new
                {
                    ContinentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Continent", x => x.ContinentId);
                });

            migrationBuilder.InsertData(
                schema: "ReferenceData",
                table: "Continent",
                columns: new[] { "ContinentId", "Code", "Name" },
                values: new object[,]
                {
                    { 1, "AF", "Africa" },
                    { 2, "AN", "Antarctica" },
                    { 3, "AS", "Asia" },
                    { 4, "CEU", "Central Europe" },
                    { 5, "EAS", "East Asia" },
                    { 6, "EEU", "East Europe" },
                    { 7, "EMD", "East Mediterranean" },
                    { 8, "EU", "Europe" },
                    { 9, "NAF", "North Africa" },
                    { 10, "NA", "North America" },
                    { 11, "NAO", "North America and Oceania" },
                    { 12, "OC", "Oceania" },
                    { 13, "SA", "South America" },
                    { 14, "SCA", "South and Central America, and the Caribbean" },
                    { 15, "SAS", "South Asia" },
                    { 16, "SEA", "South East Asia" },
                    { 17, "SSA", "Sub-Saharan Africa" },
                    { 18, "UU", "Unknown" },
                    { 19, "WEU", "West Europe" }
                });

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 1,
                column: "ContinentId",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 2,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 3,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 4,
                column: "ContinentId",
                value: 9);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 5,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 6,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 7,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 8,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 9,
                column: "ContinentId",
                value: 2);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 10,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 11,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 12,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 13,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 14,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 15,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 16,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 17,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 18,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 19,
                column: "ContinentId",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 20,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 21,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 22,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 23,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 24,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 25,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 26,
                column: "ContinentId",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 27,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 28,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 29,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 30,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 31,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 32,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 33,
                column: "ContinentId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 34,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 35,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 36,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 37,
                column: "ContinentId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 38,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 39,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 40,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 41,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 42,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 43,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 44,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 45,
                column: "ContinentId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 46,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 47,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 48,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 49,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 50,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 51,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 52,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 53,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 54,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 55,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 56,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 57,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 58,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 59,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 60,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 61,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 62,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 63,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 64,
                column: "ContinentId",
                value: 9);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 65,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 66,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 67,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 68,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 69,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 70,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 71,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 72,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 73,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 74,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 75,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 76,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 77,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 78,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 79,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 80,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 81,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 82,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 83,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 84,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 85,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 86,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 87,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 88,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 89,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 90,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 91,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 92,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 93,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 94,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 95,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 96,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 97,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 98,
                column: "ContinentId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 99,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 100,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 101,
                column: "ContinentId",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 102,
                column: "ContinentId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 103,
                column: "ContinentId",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 104,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 105,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 106,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 107,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 108,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 109,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 110,
                column: "ContinentId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 111,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 112,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 113,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 114,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 115,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 116,
                column: "ContinentId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 117,
                column: "ContinentId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 118,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 119,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 120,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 121,
                column: "ContinentId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 122,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 123,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 124,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 125,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 126,
                column: "ContinentId",
                value: 9);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 127,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 128,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 129,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 130,
                column: "ContinentId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 131,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 132,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 133,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 134,
                column: "ContinentId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 135,
                column: "ContinentId",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 136,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 137,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 138,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 139,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 140,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 141,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 142,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 143,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 144,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 145,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 146,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 147,
                column: "ContinentId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 148,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 149,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 150,
                column: "ContinentId",
                value: 9);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 151,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 152,
                column: "ContinentId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 153,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 154,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 155,
                column: "ContinentId",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 156,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 157,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 158,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 159,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 160,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 161,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 162,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 163,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 164,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 165,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 166,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 167,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 168,
                column: "ContinentId",
                value: 18);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 169,
                column: "ContinentId",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 170,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 171,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 172,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 173,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 174,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 175,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 176,
                column: "ContinentId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 177,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 178,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 179,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 180,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 181,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 182,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 183,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 184,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 185,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 186,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 187,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 188,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 189,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 190,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 191,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 192,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 193,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 194,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 195,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 196,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 197,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 198,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 199,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 200,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 201,
                column: "ContinentId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 202,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 203,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 204,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 205,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 206,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 207,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 208,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 209,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 210,
                column: "ContinentId",
                value: 15);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 211,
                column: "ContinentId",
                value: 9);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 212,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 213,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 214,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 215,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 216,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 217,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 218,
                column: "ContinentId",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 219,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 220,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 221,
                column: "ContinentId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 222,
                column: "ContinentId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 223,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 224,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 225,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 226,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 227,
                column: "ContinentId",
                value: 9);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 228,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 229,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 230,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 231,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 232,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 233,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 234,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 235,
                column: "ContinentId",
                value: 19);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 236,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 237,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 238,
                column: "ContinentId",
                value: 18);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 239,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 240,
                column: "ContinentId",
                value: 6);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 241,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 242,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 243,
                column: "ContinentId",
                value: 16);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 244,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 245,
                column: "ContinentId",
                value: 14);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 246,
                column: "ContinentId",
                value: 11);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 247,
                column: "ContinentId",
                value: 9);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 248,
                column: "ContinentId",
                value: 7);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 249,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 250,
                column: "ContinentId",
                value: 17);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 251,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 252,
                column: "ContinentId",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "ReferenceData",
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 253,
                column: "ContinentId",
                value: 17);

            migrationBuilder.CreateIndex(
                name: "IX_Country_ContinentId",
                schema: "ReferenceData",
                table: "Country",
                column: "ContinentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Country_Continent_ContinentId",
                schema: "ReferenceData",
                table: "Country",
                column: "ContinentId",
                principalSchema: "ReferenceData",
                principalTable: "Continent",
                principalColumn: "ContinentId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Country_Continent_ContinentId",
                schema: "ReferenceData",
                table: "Country");

            migrationBuilder.DropTable(
                name: "Continent",
                schema: "ReferenceData");

            migrationBuilder.DropIndex(
                name: "IX_Country_ContinentId",
                schema: "ReferenceData",
                table: "Country");

            migrationBuilder.DropColumn(
                name: "ContinentId",
                schema: "ReferenceData",
                table: "Country");
        }
    }
}
