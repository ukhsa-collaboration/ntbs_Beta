using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using System.Linq;
using ntbs_service.Models;

namespace ntbs_service.Helpers
{
    public static class SeedingHelper
    {

        private static string GetFullFilePath(string relativePathToFile) 
            => Path.Combine(Environment.CurrentDirectory, relativePathToFile);

        private static CsvReader GetInitialisedCsvReader(CsvReader csvReader) {
            csvReader.Configuration.Delimiter = ",";
            csvReader.Configuration.HasHeaderRecord = true;
            csvReader.Configuration.HeaderValidated = null;
            csvReader.Configuration.MissingFieldFound = null;

            return csvReader;
        }

        public static List<T> GetRecordsFromCSV<T>(string relativePathToFile)
        {
            var filePath = Path.Combine(Environment.CurrentDirectory, relativePathToFile);
            
            using (StreamReader reader = new StreamReader(filePath)) 
            using (CsvReader csvReader = GetInitialisedCsvReader(new CsvReader(reader))) {
                return csvReader.GetRecords<T>().ToList();
            }           
        }

        // Data seeding does not works with Entities that have Navigation (foreign key)
        // Therefore manual mapping is required
        public static List<object> GetRecordsFromCSV(string relativePathToFile, Func<CsvReader,object> getRecord) {
            var filePath = GetFullFilePath(relativePathToFile);

            var records = new List<object>();

            using (StreamReader reader = new StreamReader(filePath)) 
            using (CsvReader csvReader = new CsvReader(reader)) {
                csvReader.Read();
                csvReader.ReadHeader();
                while (csvReader.Read())
                {
                    records.Add(getRecord(csvReader));
                }
            }        
            
            return records;
        }

        public static List<object> GetHospitalsList(string relativePathToFile)
        {
            return GetRecordsFromCSV(relativePathToFile, 
                (CsvReader csvReader) => new Hospital {
                    HospitalId = Guid.Parse(csvReader.GetField("HospitalId")),
                    Name = csvReader.GetField("Name"),
                    CountryCode = csvReader.GetField("CountryCode"),
                    TBServiceCode = csvReader.GetField("TBServiceCode")
                }
            );
        }

        public static List<object> GetLAtoPHEC(string relativePathToFile)
        {
            return GetRecordsFromCSV(relativePathToFile, 
                (CsvReader csvReader) => new LocalAuthorityToPHEC {
                    PHECCode = csvReader.GetField("PHEC_Code"),
                    LocalAuthorityCode = csvReader.GetField("LA_Code")
                }
            );
        }

        public static List<object> GetLocalAuthorities(string relativePathToFile)
        {
            return GetRecordsFromCSV(relativePathToFile, 
                (CsvReader csvReader) => new LocalAuthority {
                    Name = csvReader.GetField("LA_Name"),
                    Code = csvReader.GetField("LA_Code")
                }
            );
        }

        public static List<object> GetPHECList(string relativePathToFile)
        {

            return GetRecordsFromCSV(relativePathToFile, 
                (CsvReader csvReader) => new PHEC {
                    Name = csvReader.GetField("PHEC_Name"),
                    Code = csvReader.GetField("PHEC_Code")
                }
            );
        }
    }
}