using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities
{
    public class SocialRiskFactorsTest
    {
        [Fact]
        public void CreatesSocialRiskTimePeriodsStringCorrectly()
        {
            // Arrange
            var socialRiskFactors = new SocialRiskFactors
            {
                RiskFactorDrugs = new RiskFactorDetails(RiskFactorType.Drugs)
                {
                    Status = Status.Yes,
                    IsCurrent = true,
                    InPastFiveYears = false,
                    MoreThanFiveYearsAgo = true
                }
            };

            // Act
            var timePeriods = socialRiskFactors.DrugRiskFactorTimePeriods;

            // Assert
            Assert.Equal("current, more than 5 years ago", timePeriods);
        }
    }
}
