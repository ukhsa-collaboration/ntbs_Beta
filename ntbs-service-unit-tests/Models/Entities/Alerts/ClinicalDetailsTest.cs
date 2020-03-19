using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities.Alerts
{
    public class DataQualityDotVotAlertTest
    {
        [Theory]
        [InlineData(true, true, false, false)]
        [InlineData(true, false, true, false)]
        [InlineData(true, false, false, true)]
        [InlineData(false, true, false, true)]
        [InlineData(false, false, true, true)]
        [InlineData(false, false, false, false)]
        [InlineData(null, true, false, false)]
        public void DotVotAlert_CorrectlyClassifiesNotification(
            bool? dotOffered,
            bool allRisks,
            bool anyRisk,
            bool notificationShouldQualify)
        {
            // Arrange
            var alcoholMisuseStatus = allRisks || anyRisk ? Status.Yes : (Status?)null;
            var allStatuses = allRisks ? Status.Yes : (Status?)null;
            var notification = new Notification
            {
                ClinicalDetails = new ClinicalDetails {IsDotOffered = dotOffered},
                SocialRiskFactors = new SocialRiskFactors
                {
                    AlcoholMisuseStatus = alcoholMisuseStatus,
                    RiskFactorDrugs = new RiskFactorDetails(RiskFactorType.Drugs) {Status = allStatuses},
                    RiskFactorHomelessness = new RiskFactorDetails(RiskFactorType.Homelessness) {Status = allStatuses},
                    RiskFactorImprisonment = new RiskFactorDetails(RiskFactorType.Imprisonment) {Status = allStatuses},
                    MentalHealthStatus = allStatuses,
                    SmokingStatus = allStatuses,
                    AsylumSeekerStatus = allStatuses,
                    ImmigrationDetaineeStatus = allStatuses
                }
            };

            // Act
            var notificationQualifies = DataQualityDotVotAlert.NotificationQualifies(notification);

            // Assert
            Assert.Equal(notificationShouldQualify, notificationQualifies);
        }
    }
}
