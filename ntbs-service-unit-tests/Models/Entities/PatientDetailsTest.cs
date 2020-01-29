using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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
            var patientDetails = new PatientDetails {Postcode = postcode, PostcodeToLookup = postcode};
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
            var patientDetails = new PatientDetails {Postcode = postcode};
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
            var patientDetails = new PatientDetails {Postcode = postcode, IsLegacy = true};
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
            var patientDetails = new PatientDetails {Postcode = postcode, IsLegacy = true};
            var validationResults = new List<ValidationResult>();
            
            // Act
            var isValid = Validator.TryValidateObject(patientDetails, new ValidationContext(patientDetails), validationResults, true); 

            // Assert
            Assert.False(isValid, "Expected postcode errors as postcode doesn't conform to postcode format");
            validationResults.ForEach(result =>
                Assert.Equal("Postcode is not valid", result.ErrorMessage));
            
        }
    }
}
