using System;
using System.IO;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    public partial class UpdatePostcodeLookupTable : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var pathToFile = Path.Combine(Environment.CurrentDirectory, "Models/SeedData/Postcodes.csv");
            const int itemsPerUpdate = 1000;

            var postcodeArray = new string[itemsPerUpdate];
            var laCodeArray = new string[itemsPerUpdate];

            int index = 0;

            using (StreamReader reader = new StreamReader(pathToFile)) 
            using (CsvReader csvReader = new CsvReader(reader)) {
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    var laCode = csvReader.GetField<string>("LA_Code");
                    if (!string.IsNullOrEmpty(laCode)) 
                    {
                        continue;
                    }
                    laCode = null;

                    postcodeArray[index] = csvReader.GetField("Pcode");
                    laCodeArray[index] = laCode;
                    index++;

                    if (index == itemsPerUpdate) 
                    {
                        migrationBuilder.UpdateData(
                            table: "PostcodeLookup",
                            keyColumn: "Postcode",
                            keyValues: postcodeArray,
                            column: "LocalAuthorityCode",
                            values: laCodeArray
                        );
                        postcodeArray = new string[itemsPerUpdate];
                        laCodeArray = new string[itemsPerUpdate];
                        index = 0;
                    }
                }
            }

            if (index > 0) 
            {
                migrationBuilder.UpdateData(
                    table: "PostcodeLookup",
                    keyColumn: "Postcode",
                    keyValues: postcodeArray,
                    column: "LocalAuthorityCode",
                    values: laCodeArray
                );             
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
        }
    }
}
