using System;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_service_unit_tests.Models
{
    // TODO NTBS-960 revisit this and split up as necessary
    public class NotificationTest
    {
        readonly Notification TestNotification = new Notification {
            PatientDetails = new PatientDetails(),
            ClinicalDetails = new ClinicalDetails(),
            SocialRiskFactors = new SocialRiskFactors()
        };

        [Fact]
        public void FormatsFullNameCorrectly()
        {
            // Arrange
            TestNotification.PatientDetails.FamilyName = "example";
            TestNotification.PatientDetails.GivenName = "name";

            // Act
            var fullName = TestNotification.PatientDetails.FullName;

            // Assert
            Assert.Equal("EXAMPLE, name", fullName);
        }

        [Fact]
        public void FormatsPostcodeCorrectly()
        {
            // Arrange
            TestNotification.PatientDetails.NoFixedAbode = false;
            TestNotification.PatientDetails.Postcode = " NW12 3RT   ";

            // Act
            var postcode = TestNotification.PatientDetails.FormattedNoAbodeOrPostcodeString;

            // Assert
            Assert.Equal("NW12 3RT", postcode);
        }

        [Fact]
        public void FormatsDateCorrectly() 
        {
            // Arrange
            TestNotification.ClinicalDetails.SymptomStartDate = new DateTime(2000, 1, 1);
            
            // Act
            var formattedDate = TestNotification.ClinicalDetails.FormattedSymptomStartDate;

            // Assert
            Assert.Equal("01 Jan 2000", formattedDate);
        }

        [Fact]
        public void CalculatesDaysBetweenDates() 
        {
            // Arrange
            TestNotification.ClinicalDetails.SymptomStartDate = new DateTime(2000, 1, 1);
            TestNotification.ClinicalDetails.FirstPresentationDate = new DateTime(2000, 1, 4);
            
            // Act
            var days = TestNotification.ClinicalDetails.DaysFromOnsetToFirstPresentation;

            // Assert
            Assert.Equal("3 days", days);
        }

        // [Fact]
        // public void CreatesSocialRiskTimePeriodsStringCorrectly() 
        // {
        //     // Arrange
        //     TestNotification.SocialRiskFactors.RiskFactorDrugs = new RiskFactorDetails(RiskFactorType.Drugs)
        //     {
        //         Status = Status.Yes,
        //         IsCurrent = true,
        //         InPastFiveYears = false,
        //         MoreThanFiveYearsAgo = true
        //     };
        //     
        //     // Act
        //     var timePeriods = TestNotification.DrugRiskFactorTimePeriods;
        //
        //     // Assert
        //     Assert.Equal("current, more than 5 years ago", timePeriods);
        // }

        [Fact]
        public void CreatesVaccinationStateStringCorrectly() {
            // Arrange
            TestNotification.ClinicalDetails.BCGVaccinationState = Status.Yes;
            TestNotification.ClinicalDetails.BCGVaccinationYear = 2000;
            
            // Act
            var stateAndYear = TestNotification.ClinicalDetails.BCGVaccinationStateAndYear;

            // Assert
            Assert.Equal("Yes - 2000", stateAndYear);
        }

        [Theory]
        [InlineData(2019, 1, 1, "RR/MDR/XDR treatment - 01 Jan 2019")]
        [InlineData(null, null, null, "RR/MDR/XDR treatment")]
        public void CreatesMdrTreatmentStringCorrectly(int? year, int? month, int? day, string expectedResult) {
            // Arrange
            if (year != null && month != null && day != null) {
                var dateTime = new DateTime((int)year, (int)month, (int)day);
                TestNotification.ClinicalDetails.MDRTreatmentStartDate = dateTime;
            }

            TestNotification.ClinicalDetails.TreatmentRegimen = TreatmentRegimen.MdrTreatment;

            // Act
            var stateAndDate = TestNotification.ClinicalDetails.FormattedTreatmentRegimen;

            // Assert
            Assert.Equal(expectedResult, stateAndDate);
        }
    }
}
