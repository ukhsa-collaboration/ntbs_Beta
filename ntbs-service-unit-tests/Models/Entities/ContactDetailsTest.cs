using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities
{
    public class ContactDetailsTest
    {
        [Fact]
        public void JobTitle_IsInvalid()
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var details = new User { JobTitle = "!!Job title~" };

            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            var expectedErrorMessage = string.Format(ValidationMessages.InvalidCharacter, "Job Title");
            Assert.False(isValid, "Expected details to be invalid");
            Assert.Equal(expectedErrorMessage, validationResults.First().ErrorMessage);
        }

        [Fact]
        public void PrimaryPhoneNumber_IsInvalid()
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var details = new User { PhoneNumberPrimary = "¬¬~07712190131" };

            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            var expectedErrorMessage = string.Format(ValidationMessages.InvalidCharacter, "Phone number #1");
            Assert.False(isValid, "Expected details to be invalid");
            Assert.Equal(expectedErrorMessage, validationResults.First().ErrorMessage);
        }

        [Fact]
        public void SecondaryPhoneNumber_IsInvalid()
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var details = new User { PhoneNumberSecondary = "!!077121901231~" };

            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            var expectedErrorMessage = string.Format(ValidationMessages.InvalidCharacter, "Phone number #2");
            Assert.False(isValid, "Expected details to be invalid");
            Assert.Equal(expectedErrorMessage, validationResults.First().ErrorMessage);
        }

        [Fact]
        public void PrimaryEmail_IsInvalid()
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var details = new User { EmailPrimary = "!primay@email~" };

            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            var expectedErrorMessage = string.Format(ValidationMessages.InvalidCharacter, "Email #1");
            Assert.False(isValid, "Expected details to be invalid");
            Assert.Equal(expectedErrorMessage, validationResults.First().ErrorMessage);
        }

        [Fact]
        public void SecondaryEmail_IsInvalid()
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var details = new User { EmailSecondary = "!!seconday@email~" };

            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            var expectedErrorMessage = string.Format(ValidationMessages.InvalidCharacter, "Email #2");
            Assert.False(isValid, "Expected details to be invalid");
            Assert.Equal(expectedErrorMessage, validationResults.First().ErrorMessage);
        }

        [Fact]
        public void Notes_IsInvalid()
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var details = new User { Notes = "¬!Notes~" };

            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            var expectedErrorMessage = string.Format(ValidationMessages.InvalidCharacter, "Notes");
            Assert.False(isValid, "Expected details to be invalid");
            Assert.Equal(expectedErrorMessage, validationResults.First().ErrorMessage);
        }

        [Fact]
        public void AllFieldsValid()
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            var details = new User
            {
                JobTitle = "Doctor",
                PhoneNumberPrimary = "0759000000",
                PhoneNumberSecondary = "079000000",
                EmailPrimary = "primary@email",
                EmailSecondary = "secondary@email",
                Notes = "Simple Notes"
            };

            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            Assert.True(isValid, "Expected details to be valid");
        }
    }
}
