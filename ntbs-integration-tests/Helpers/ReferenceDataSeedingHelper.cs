using System;
using System.Collections.Generic;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Helpers;

namespace ntbs_integration_tests.Helpers
{
    public static class ReferenceDataSeedingHelper
    {
        public static List<Hospital> GetHospitalsList()
        {
            return CsvParser.GetRecordsFromCsv("../../../SeedingReferenceData/Hospital.csv",
                csvReader => new Hospital
                {
                    HospitalId = Guid.Parse(csvReader.GetField("HospitalId")),
                    Name = csvReader.GetField("Name"),
                    CountryCode = csvReader.GetField("CountryCode"),
                    TBServiceCode = csvReader.GetField("TBServiceCode"),
                    IsLegacy = csvReader.GetField<bool>("IsLegacy")
                }
            );
        }

        public static List<TBService> GetTBServices()
        {
            return CsvParser.GetRecordsFromCsv("../../../SeedingReferenceData/TbService.csv", csvReader => new TBService
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

        public static List<LocalAuthorityToPHEC> GetLAtoPHECs()
        {
            return CsvParser.GetRecordsFromCsv("../../../SeedingReferenceData/LocalAuthorityToPhec.csv",
                csvReader => new LocalAuthorityToPHEC
                {
                    PHECCode = csvReader.GetField("PHECCode"),
                    LocalAuthorityCode = csvReader.GetField("LocalAuthorityCode")
                }
            );
        }

        public static List<LocalAuthority> GetLocalAuthorities()
        {
            return CsvParser.GetRecordsFromCsv("../../../SeedingReferenceData/LocalAuthority.csv",
                csvReader => new LocalAuthority
                {
                    Name = csvReader.GetField("Name"),
                    Code = csvReader.GetField("Code")
                }
            );
        }

        public static List<PHEC> GetPHECList()
        {
            return CsvParser.GetRecordsFromCsv("../../../SeedingReferenceData/Phec.csv",
                csvReader => new PHEC
                {
                    Name = csvReader.GetField("Name"),
                    Code = csvReader.GetField("Code"),
                    AdGroup = csvReader.GetField("AdGroup")
                }
            );
        }

        public static List<PostcodeLookup> GetPostcodeLookups()
        {
            return CsvParser.GetRecordsFromCsv("../../../SeedingReferenceData/PostcodeLookup.csv",
                csvReader => new PostcodeLookup
                {
                    Postcode = csvReader.GetField("Postcode"),
                    LocalAuthorityCode = csvReader.GetField("LocalAuthorityCode"),
                    CountryCode = csvReader.GetField("CountryCode")
                }
            );
        }
    }
}
