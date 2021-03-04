using System;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities
{
    public class ClinicalDetailsTest
    {
        [Fact]
        public void FormatsDateCorrectly()
        {
            // Arrange
            var clinicalDetails = new ClinicalDetails { SymptomStartDate = new DateTime(2000, 1, 1) };

            // Act
            var formattedDate = clinicalDetails.FormattedSymptomStartDate;

            // Assert
            Assert.Equal("01 Jan 2000", formattedDate);
        }

        [Fact]
        public void CalculatesDaysBetweenDates()
        {
            // Arrange
            var clinicalDetails = new ClinicalDetails
            {
                SymptomStartDate = new DateTime(2000, 1, 1),
                FirstPresentationDate = new DateTime(2000, 1, 4)
            };

            // Act
            var days = clinicalDetails.DaysFromOnsetToFirstPresentation;

            // Assert
            Assert.Equal("3 days", days);
        }

        [Fact]
        public void CreatesVaccinationStateStringCorrectly()
        {
            // Arrange
            var clinicalDetails = new ClinicalDetails { BCGVaccinationState = Status.Yes, BCGVaccinationYear = 2000, };

            // Act
            var stateAndYear = clinicalDetails.BCGVaccinationStateAndYear;

            // Assert
            Assert.Equal("Yes - 2000", stateAndYear);
        }

        [Theory]
        [InlineData(2019, 1, 1, "RR/MDR/XDR treatment - 01 Jan 2019")]
        [InlineData(null, null, null, "RR/MDR/XDR treatment")]
        public void CreatesMdrTreatmentStringCorrectly(int? year, int? month, int? day, string expectedResult)
        {

            // Arrange
            var clinicalDetails = new ClinicalDetails();
            if (year != null && month != null && day != null)
            {
                var dateTime = new DateTime((int)year, (int)month, (int)day);
                clinicalDetails.MDRTreatmentStartDate = dateTime;
            }

            clinicalDetails.TreatmentRegimen = TreatmentRegimen.MdrTreatment;

            // Act
            var stateAndDate = clinicalDetails.FormattedTreatmentRegimen;

            // Assert
            Assert.Equal(expectedResult, stateAndDate);
        }
    }
}
