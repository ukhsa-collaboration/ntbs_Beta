using ntbs_service.Helpers;
using ntbs_service.Models;
using Xunit;

namespace ntbs_service_unit_tests.Helpers
{
    public class SeedingHelperTest
    {
        [Fact]
        public void GetRecordsFromCSV_GetsTBServicesData() {
            var results = SeedingHelper.GetTBServices("../../../TestData/tbservices.csv");
            var firstEntryInTBServicesList = new TBService {
                Code = "TBS0001",
                Name = "Abingdon Community Hospital",
                PHECCode = "E45000019"
            };
            const int expectedCount = 6;

            Assert.Equal(results.Count, expectedCount);
            Assert.Equal(((TBService)results[0]).Code, firstEntryInTBServicesList.Code);
            Assert.Equal(((TBService)results[0]).Name, firstEntryInTBServicesList.Name);
            Assert.Equal(((TBService)results[0]).PHECCode, firstEntryInTBServicesList.PHECCode);
        }

    }
}