using System;
using System.Collections.Generic;
using CsvHelper;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Helpers
{
    public static class SeedingHelper
    {
        public static List<Hospital> GetHospitalsList(string relativePathToFile)
        {
            return CsvParser.GetRecordsFromCsv(relativePathToFile, 
                (CsvReader csvReader) => new Hospital {
                    HospitalId = Guid.Parse(csvReader.GetField("HospitalId")),
                    Name = csvReader.GetField("Name"),
                    CountryCode = csvReader.GetField("CountryCode"),
                    TBServiceCode = csvReader.GetField("TBServiceCode"),
                    IsLegacy = csvReader.GetField<bool>("IsLegacy")
                }
            );
        }

        public static List<TBService> GetTBServices(string relativePathToFile)
        {
            return CsvParser.GetRecordsFromCsv(relativePathToFile, csvReader => new TBService
                {
                    Code = csvReader.GetField("Code"),
                    Name = csvReader.GetField("Name"),
                    PHECCode = string.IsNullOrEmpty(csvReader.GetField("PHEC_Code"))
                        ? null
                        : csvReader.GetField("PHEC_Code"),
                    ServiceAdGroup = string.IsNullOrEmpty(csvReader.GetField("Service_Ad_Group"))
                        ? null
                        : csvReader.GetField("Service_Ad_Group"),
                    IsLegacy = csvReader.GetField<bool>("IsLegacy")
                }
            );
        }

        public static List<LocalAuthorityToPHEC> GetLAtoPHEC(string relativePathToFile)
        {
            return CsvParser.GetRecordsFromCsv(relativePathToFile,
                (CsvReader csvReader) => new LocalAuthorityToPHEC {
                    PHECCode = csvReader.GetField("PHEC_Code"),
                    LocalAuthorityCode = csvReader.GetField("LA_Code")
                }
            );
        }

        public static List<LocalAuthority> GetLocalAuthorities(string relativePathToFile)
        {
            return CsvParser.GetRecordsFromCsv(relativePathToFile, 
                (CsvReader csvReader) => new LocalAuthority {
                    Name = csvReader.GetField("LA_Name"),
                    Code = csvReader.GetField("LA_Code")
                }
            );
        }

        public static List<PHEC> GetPHECList(string relativePathToFile)
        {

            return CsvParser.GetRecordsFromCsv(relativePathToFile, 
                (CsvReader csvReader) => new PHEC {
                    Name = csvReader.GetField("PHEC_Name"),
                    Code = csvReader.GetField("PHEC_Code"),
                    AdGroup = csvReader.GetField("Ad_Group")
                }
            );
        }
    }
}
