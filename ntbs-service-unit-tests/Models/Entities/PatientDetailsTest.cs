using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities
{
    public class PatientDetailsTest
    {
        [Theory]
        [InlineData("AB10AA")]
        [InlineData("AB10AB")]
        [InlineData("AB10AD")]
        [InlineData("AB10AE")]
        public void Postcode_WithMatchingLookup_IsValid(string postcode)
        {
            // Arrange
            var patientDetails = new PatientDetails { Postcode = postcode, PostcodeToLookup = postcode };
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(patientDetails, new ValidationContext(patientDetails), validationResults, true);

            // Assert
            Assert.True(isValid, "Expected no postcode errors");
        }

        [Theory]
        [InlineData("DE142GL")]
        [InlineData("W187SB")]
        [InlineData("MK777FB")]
        public void Postcode_WithNoMatchingLookup_IsInvalid(string postcode)
        {
            // Arrange
            var patientDetails = new PatientDetails { Postcode = postcode };
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(patientDetails, new ValidationContext(patientDetails), validationResults, true);

            // Assert
            Assert.False(isValid, "Expected postcode errors");
            validationResults.ForEach(result =>
                Assert.Equal("Postcode is not found", result.ErrorMessage));

        }

        [Theory]
        [InlineData("DE142GL")]
        [InlineData("W187SB")]
        [InlineData("MK777FB")]
        public void Postcode_WithNoMatchingLookupForLegacyNotification_IsValid(string postcode)
        {
            // Arrange
            var patientDetails = new PatientDetails { Postcode = postcode, IsLegacy = true };
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(patientDetails, new ValidationContext(patientDetails), validationResults, true);

            // Assert
            Assert.True(isValid, "Expected no postcode errors for legacy notifications");
        }

        [Theory]
        [InlineData("CB29J")]
        [InlineData("4156255")]
        [InlineData("garbage")]
        public void Postcode_WithJunkContent_IsInvalidEvenForLegacyNotifications(string postcode)
        {
            // Arrange
            var patientDetails = new PatientDetails { Postcode = postcode, IsLegacy = true };
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(patientDetails, new ValidationContext(patientDetails), validationResults, true);

            // Assert
            Assert.False(isValid, "Expected postcode errors as postcode doesn't conform to postcode format");
            validationResults.ForEach(result =>
                Assert.Equal("Postcode is not valid", result.ErrorMessage));

        }

        [Fact]
        public void FormatsFullNameCorrectly()
        {
            // Arrange
            var patientDetails = new PatientDetails { FamilyName = "example", GivenName = "name" };

            // Act
            var fullName = patientDetails.FullName;

            // Assert
            Assert.Equal("EXAMPLE, name", fullName);
        }

        [Fact]
        public void FormatsPostcodeCorrectly()
        {
            // Arrange
            var patientDetails = new PatientDetails
            {
                NoFixedAbode = false,
                Postcode = " NW12 3RT   "
            };

            // Act
            var postcode = patientDetails.FormattedNoAbodeOrPostcodeString;

            // Assert
            Assert.Equal("NW12 3RT", postcode);
        }

        [Theory]
        [InlineData(Countries.UkId, true)]
        [InlineData(Countries.UnknownId, false)]
        [InlineData(33, false)]
        [InlineData(null, false)]
        public void UkBorn_IsSetToCorrectValueDependentOnBirthCountry(int? countryId, bool? expectedResult)
        {
            // Arrange
            var notification = new Notification {PatientDetails = new PatientDetails() {CountryId = countryId}};

            // Assert
            Assert.Equal(expectedResult, notification.PatientDetails.UkBorn);
        }
    }
}
