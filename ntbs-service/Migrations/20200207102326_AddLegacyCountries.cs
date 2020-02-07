using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddLegacyCountries : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLegacy",
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
                columns: new[] { "CountryId", "HasHighTbOccurence", "IsLegacy", "IsoCode", "Name" },
                values: new object[,]
                {
                    { 251, false, true, "CS", "Serbia & Montenegro" },
                    { 252, false, true, "YU", "Yugoslavia" },
                    { 253, false, true, "ZR", "Zaire" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Country_IsLegacy_Name",
                table: "Country",
                columns: new[] { "IsLegacy", "Name" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Country_IsLegacy_Name",
                table: "Country");

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
                name: "IsLegacy",
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
