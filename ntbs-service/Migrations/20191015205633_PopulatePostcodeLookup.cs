using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using CsvHelper;
using Microsoft.EntityFrameworkCore.Migrations;
using ntbs_service.Helpers;
using ntbs_service.Models;

namespace ntbs_service.Migrations
{
    public partial class PopulatePostcodeLookup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            var pathToFile = Path.Combine(Environment.CurrentDirectory, "Models/SeedData/ReducedPostcode.csv");
            const int itemsPerUpdate = 1000;

            var records = new string[itemsPerUpdate,3];
            int countPerUpdate = 0;

            using (StreamReader reader = new StreamReader(pathToFile)) 
            using (CsvReader csvReader = new CsvReader(reader)) {
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    records[countPerUpdate, 0] = csvReader.GetField("Pcode");
                    records[countPerUpdate, 1] = csvReader.GetField("Country");
                    records[countPerUpdate, 2] = csvReader.GetField("LA_Code");
                    countPerUpdate++;

                    if (countPerUpdate == itemsPerUpdate)
                    {
                        migrationBuilder.InsertData(
                            table: "PostcodeLookup",
                            columns: new[] { "Postcode", "CountryCode", "LocalAuthorityCode" },
                            values: records
                        );
                        records = new string[itemsPerUpdate, 3];
                        countPerUpdate = 0;                    
                    }
                }
            }

            if (countPerUpdate > 0) 
            {
                var finalBatch = new string[countPerUpdate,3]; 
                for (int index=0; index<countPerUpdate; index++)
                {
                    finalBatch[index,0] = records[index,0];
                    finalBatch[index,1] = records[index,1];
                    finalBatch[index,2] = records[index,2];
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

        }
    }
}
