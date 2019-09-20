using System;
using System.Collections.Generic;
using ntbs_service.Models;
using Xunit;

namespace ntbs_service_tests.UnitTests.Models
{
    public class NotificationTest
    {

        public static Notification TestNotification = new Notification {
            PatientDetails = new PatientDetails {
                FamilyName = "example",
                GivenName = "name",
                NoFixedAbode = false,
                Postcode = "NW 123 RT",
                NhsNumber = "1234567890"
            },
            ClinicalDetails = new ClinicalDetails {
                SymptomStartDate = new DateTime(2000, 1, 1),
                PresentationDate = new DateTime(2000, 1, 4),
                BCGVaccinationState = ntbs_service.Models.Enums.Status.Yes,
                BCGVaccinationYear = 2000
            },
            SocialRiskFactors = new SocialRiskFactors {
                RiskFactorDrugs = new RiskFactorBase {
                    Status = ntbs_service.Models.Enums.Status.Yes,
                    IsCurrent = true,
                    InPastFiveYears = false,
                    MoreThanFiveYearsAgo = true
                }
            }
        };

        [Fact]
        public void FormatsFullNameCorrectly()
        {
            // Arrange
            var expectedFullName = "EXAMPLE, name";
            var fullName = TestNotification.FullName;

            // Assert
            Assert.Equal(expectedFullName, fullName);
        }

        [Fact]
        public void FormatsPostcodeCorrectly()
        {
            // Arrange
            var expectedPostcode = "NW12 3RT";
            var postcode = TestNotification.FormattedNoAbodeOrPostcodeString;

            // Assert
            Assert.Equal(expectedPostcode, postcode);
        }

        [Fact]
        public void FormatsDateCorrectly() {
            // Arrange
            var expectedDate = "01-Jan-2000";
            var formattedDate = TestNotification.FormattedSymptomStartDate;

            // Assert
            Assert.Equal(expectedDate, formattedDate);
        }

        [Fact]
        public void CalculatesDaysBetweenDates() {
            // Arrange
            var expectedDays = 3;
            var days = TestNotification.DaysFromOnsetToPresentation;

            // Assert
            Assert.Equal(expectedDays, days);
        }

        public void CreatesSocialRiskTimePeriodsStringCorrectly() {
            // Arrange
            var expectedTimePeriods = "current, more than 5 years ago";
            var timePeriods = TestNotification.DrugRiskFactorTimePeriods;

            // Assert
            Assert.Equal(expectedTimePeriods, timePeriods);
        }

        public void CreatesVaccinationStateStringCorrectly() {
            // Arrange
            var expectedStateAndYear = "Yes - 2000";
            var stateAndYear = TestNotification.BCGVaccinationStateAndYear;

            // Assert
            Assert.Equal(expectedStateAndYear, stateAndYear);
        }
    }
}