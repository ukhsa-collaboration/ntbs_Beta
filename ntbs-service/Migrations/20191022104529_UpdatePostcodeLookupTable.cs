using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    /*  
    Previous version of Postcodes file had dummy LA codes for postcodes which were missing LA codes.
    Current (up to date) version has LA codes as NULL/empty string for some postcodes.
    This migration updates only postcodes that should have NULL value for LA codes
    */  
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
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture)) {
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    var laCode = csvReader.GetField<string>("LA_Code");
                    
                    // Skip postcodes that have LA_Code defined in Postcode csv 
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
