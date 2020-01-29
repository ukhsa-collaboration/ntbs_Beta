using System;
using System.Collections.Generic;
using System.IO;
using CsvHelper;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Helpers
{
    public static class SeedingHelper
    {

        private static string GetFullFilePath(string relativePathToFile) 
            => Path.Combine(Environment.CurrentDirectory, relativePathToFile);

        // Data seeding does not work with Entities that have Navigation (foreign key)
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

        public static List<object> GetTBServices(string relativePathToFile)
        {
            return GetRecordsFromCSV(relativePathToFile, 
                (CsvReader csvReader) => new TBService {
                    Code = csvReader.GetField("Code"),
                    Name = csvReader.GetField("Name"),
                    PHECCode = string.IsNullOrEmpty(csvReader.GetField("PHEC_Code")) ? null : csvReader.GetField("PHEC_Code"),
                    ServiceAdGroup = string.IsNullOrEmpty(csvReader.GetField("Service_Ad_Group")) ? null : csvReader.GetField("Service_Ad_Group")
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
                    Code = csvReader.GetField("PHEC_Code"),
                    AdGroup = csvReader.GetField("Ad_Group")
                }
            );
        }
    }
}
