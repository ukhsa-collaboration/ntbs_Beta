using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class AddPCTtoPostcodesTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>("PCT", table: "PostcodeLookup", type: "varchar(10)", schema: "ReferenceData", nullable: true);

            var pathToFile = Path.Combine(Environment.CurrentDirectory, "Models/SeedData/Postcodes.csv");
            const int itemsPerUpdate = 1000;

            var postcodeArray = new string[itemsPerUpdate];
            var pctArray = new string[itemsPerUpdate];

            int index = 0;

            using (StreamReader reader = new StreamReader(pathToFile)) 
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    postcodeArray[index] = csvReader.GetField<string>("Pcode");
                    pctArray[index] = csvReader.GetField<string>("PCT");
                    index++;

                    if (index == itemsPerUpdate) 
                    {
                        migrationBuilder.UpdateData(
                            table: "PostcodeLookup",
                            schema: "ReferenceData",
                            keyColumn: "Postcode",
                            keyValues: postcodeArray,
                            column: "PCT",
                            values: pctArray
                        );
                        postcodeArray = new string[itemsPerUpdate];
                        pctArray = new string[itemsPerUpdate];
                        index = 0;
                    }
                }
            }

            if (index > 0) 
            {
                migrationBuilder.UpdateData(
                    table: "PostcodeLookup",
                    schema: "ReferenceData",
                    keyColumn: "Postcode",
                    keyValues: postcodeArray,
                    column: "PCT",
                    values: pctArray
                );
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn("PCT", "PostcodeLookup", schema: "ReferenceData");
        }
    }
}
