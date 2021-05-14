using System;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities.Alerts
{
    public class DataQualityDotVotAlertTest
    {
        [Theory]
        [InlineData("Yes", true, true, false, false)]
        [InlineData("Yes", true, false, true, false)]
        [InlineData("Yes", false, false, true, false)]
        [InlineData("Yes", true, false, false, true)]
        [InlineData("Yes", false, false, false, false)]
        [InlineData("No", true, true, false, true)]
        [InlineData("No", true, false, true, true)]
        [InlineData("No", false, false, true, false)]
        [InlineData("No", true, false, false, false)]
        [InlineData("Unknown", true, true, false, false)]
        [InlineData(null, true, true, false, false)]
        public void DotVotAlert_CorrectlyClassifiesNotification(
            string dotOfferedString,
            bool notificationNotified,
            bool allRisks,
            bool anyRisk,
            bool notificationShouldQualify)
        {
            // Arrange
            Enum.TryParse(dotOfferedString, out Status dotOffered);
            var notificationStatus = notificationNotified ? NotificationStatus.Notified : NotificationStatus.Closed;
            var alcoholMisuseStatus = allRisks || anyRisk ? Status.Yes : (Status?)null;
            var allStatuses = allRisks ? Status.Yes : (Status?)null;
            var notification = new Notification
            {
                NotificationStatus = notificationStatus,
                ClinicalDetails = new ClinicalDetails { IsDotOffered = dotOffered },
                SocialRiskFactors = new SocialRiskFactors
                {
                    AlcoholMisuseStatus = alcoholMisuseStatus,
                    RiskFactorDrugs = new RiskFactorDetails(RiskFactorType.Drugs) { Status = allStatuses },
                    RiskFactorHomelessness = new RiskFactorDetails(RiskFactorType.Homelessness) { Status = allStatuses },
                    RiskFactorImprisonment = new RiskFactorDetails(RiskFactorType.Imprisonment) { Status = allStatuses },
                    MentalHealthStatus = allStatuses,
                    RiskFactorSmoking = new RiskFactorDetails(RiskFactorType.Smoking) { Status = allStatuses },
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
