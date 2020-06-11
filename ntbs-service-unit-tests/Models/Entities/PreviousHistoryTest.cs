using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities
{
    public class PreviousHistoryTest
    {
        [Theory]
        [InlineData(1899, "Please enter a valid year")]
        [InlineData(2040, "Year of previous diagnosis must be the current year or earlier")]
        [InlineData(1950, "Year of previous diagnosis must be later than date of birth year")]
        public void SettingInvalidYears_ReturnsCorrectErrorMessage(int year, string errorMessage)
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var details = new PreviousTbHistory
            {
                ShouldValidateFull = false,
                DobYear = 2000,
                PreviouslyHadTb = Status.Yes,
                PreviousTbDiagnosisYear = year,
            };

            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            Assert.Contains(validationResults, r => r.ErrorMessage == errorMessage);
            Assert.False(isValid, "Expected details to be invalid");
        }
    }
}
