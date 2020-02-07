using System;
using System.Globalization;
using System.IO;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ntbs_service.Migrations
{
    // Postcode lookup has around 3 million records, seeding with FluentApi is very time consuming with large data.
    // Therefore, this migration is solely responsible for adding data into postcode lookup
    public partial class PopulatePostcodeLookup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var pathToFile = Path.Combine(Environment.CurrentDirectory, "Models/SeedData/Postcodes.csv");
            const int itemsPerUpdate = 1000;

            var records = new string[itemsPerUpdate, 3];
            int index = 0;

            using (StreamReader reader = new StreamReader(pathToFile))
            using (CsvReader csvReader = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    records[index, 0] = csvReader.GetField("Pcode");
                    records[index, 1] = csvReader.GetField("Country");
                    var laCode = csvReader.GetField("LA_Code");
                    records[index, 2] = string.IsNullOrWhiteSpace(laCode) ? null : laCode;
                    index++;

                    if (index == itemsPerUpdate)
                    {
                        migrationBuilder.InsertData(
                            table: "PostcodeLookup",
                            columns: new[] { "Postcode", "CountryCode", "LocalAuthorityCode" },
                            values: records
                        );
                        records = new string[itemsPerUpdate, 3];
                        index = 0;
                    }
                }
            }

            if (index > 0)
            {
                var finalBatch = new string[index, 3];
                for (int i = 0; i < index; i++)
                {
                    finalBatch[i, 0] = records[i, 0];
                    finalBatch[i, 1] = records[i, 1];
                    finalBatch[i, 2] = records[i, 2];
                }

                migrationBuilder.InsertData(
                    table: "PostcodeLookup",
                    columns: new[] { "Postcode", "CountryCode", "LocalAuthorityCode" },
                    values: finalBatch
                );
            }
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("DELETE FROM [PostcodeLookup]", true);
        }
    }
}
