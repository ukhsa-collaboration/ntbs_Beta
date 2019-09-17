using System;
using System.Collections.Generic;
using ntbs_service.Helpers;
using ntbs_service.Models;
using Xunit;

namespace ntbs_service_tests.UnitTests.Helpers
{
    public class SeedingHelperTest
    {
        [Fact]
        public void GetRecordsFromCSV_GetsTBServicesData() {
            var results = SeedingHelper.GetRecordsFromCSV<TBService>("../../../UnitTests/TestData/tbservices.csv");
            var firstEntryInTBServicesList = new TBService {
                Code = "TBS0001",
                Name = "Abingdon Community Hospital"
            };
            const int expectedCount = 6;

            Assert.Equal(results.Count, expectedCount);
            Assert.Equal(results[0].Code, firstEntryInTBServicesList.Code);
            Assert.Equal(results[0].Name, firstEntryInTBServicesList.Name);

        }

    }
}