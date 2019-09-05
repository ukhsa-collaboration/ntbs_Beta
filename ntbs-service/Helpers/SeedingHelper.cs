using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Linq;

namespace ntbs_service.Helpers
{
    public static class SeedingHelper
    {
        public static List<Object> GetHospitalsList(string pathToFile)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, pathToFile);
            var anonymousTypeDefinition = new 
            {
                HospitalId = default(Guid),
                Name = string.Empty,
                CountryCode = string.Empty,
                TBServiceCode = string.Empty
            };

            using (StreamReader reader = new StreamReader(filePath)) 
            using (CsvReader csvReader = new CsvReader(reader)) {
                return csvReader.GetRecords(anonymousTypeDefinition).Cast<Object>().ToList();
            }
        }

        public static List<T> GetRecordsFromCSV<T>(string pathToFile)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, pathToFile);
            
            using (StreamReader reader = new StreamReader(filePath)) 
            using (CsvReader csvReader = GetInitialisedCsvReader(new CsvReader(reader))) {
                var a = csvReader.GetRecords<T>().ToList();
                Console.WriteLine("Done");
                Console.WriteLine(a[0]);

                return a;
            }           
        }

        private static CsvReader GetInitialisedCsvReader(CsvReader csvReader) {
            csvReader.Configuration.Delimiter = ",";
            csvReader.Configuration.HasHeaderRecord = true;
            csvReader.Configuration.HeaderValidated = null;
            csvReader.Configuration.MissingFieldFound = null;

            return csvReader;
        }
    }
}