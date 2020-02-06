using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddLegacyCountries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "LegacyCountry",
                table: "Country",
                nullable: false,
                defaultValue: false);

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 51,
                column: "Name",
                value: "Congo, Democratic Republic of the");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 131,
                column: "Name",
                value: "North Macedonia");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 171,
                column: "Name",
                value: "Palestine, State of");

            migrationBuilder.InsertData(
                table: "Country",
                columns: new[] { "CountryId", "HasHighTbOccurence", "IsoCode", "LegacyCountry", "Name" },
                values: new object[,]
                {
                    { 251, false, "CS", true, "Serbia & Montenegro" },
                    { 252, false, "YU", true, "Yugoslavia" },
                    { 253, false, "ZR", true, "Zaire" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 251);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 252);

            migrationBuilder.DeleteData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 253);

            migrationBuilder.DropColumn(
                name: "LegacyCountry",
                table: "Country");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 51,
                column: "Name",
                value: "Congo, The Democratic Republic of the");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 131,
                column: "Name",
                value: "Macedonia, The Former Yugoslav Republic of");

            migrationBuilder.UpdateData(
                table: "Country",
                keyColumn: "CountryId",
                keyValue: 171,
                column: "Name",
                value: "Palestinian Territory, Occupied");
        }
    }
}
