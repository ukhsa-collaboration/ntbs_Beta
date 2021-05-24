using ntbs_service.Helpers;
using ntbs_service.Models.ReferenceEntities;
using Xunit;

namespace ntbs_service_unit_tests.Helpers
{
    public class SeedingHelperTest
    {
        [Fact]
        public void GetRecordsFromCSV_GetsTBServicesData()
        {
            var results = SeedingHelper.GetTBServices("../../../TestData/tbservices.csv");
            var firstEntryInTBServicesList = new TBService
            {
                Code = "TBS0001",
                Name = "Abingdon Community Hospital",
                PHECCode = "E45000019",
                ServiceAdGroup = "App.Auth.NIS.NTBS.Service_Abingdon"
            };
            const int expectedCount = 6;

            Assert.Equal(results.Count, expectedCount);
            Assert.Equal(firstEntryInTBServicesList.Code, results[0].Code);
            Assert.Equal(firstEntryInTBServicesList.Name, results[0].Name);
            Assert.Equal(firstEntryInTBServicesList.PHECCode, results[0].PHECCode);
            Assert.Equal(firstEntryInTBServicesList.ServiceAdGroup, results[0].ServiceAdGroup);
        }

    }
}
