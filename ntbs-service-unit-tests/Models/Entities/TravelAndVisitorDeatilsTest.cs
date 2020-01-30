using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_service_unit_tests.Models.Entities
{
    public class TravelAndVisitorDetailsTest
    {
        public static IEnumerable<object[]> BaseDetails()
        {
            yield return new object[] {new TravelDetails {ShouldValidateFull = false, HasTravel = true}};
            yield return new object[] {new TravelDetails {ShouldValidateFull = true, HasTravel = true}};
            yield return new object[] {new VisitorDetails {ShouldValidateFull = false, HasVisitor = true}};
            yield return new object[] {new VisitorDetails {ShouldValidateFull = true, HasVisitor = true}};
        }

        [Theory, MemberData(nameof(BaseDetails))]
        public void SelectingYes_AndLeavingRestBlank_IsValidInDraftAndFullModes(ITravelOrVisitorDetails details)
        {
            // Arrange
            var validationResults = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            Assert.True(isValid, "Expected details to be valid");
        }

        [Theory, MemberData(nameof(BaseDetails))]
        public void ProvidingCountry_ButNotTotalNumberOfCountries_IsInvalid(ITravelOrVisitorDetails details)
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            details.Country1Id = 1;

            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            Assert.False(isValid, "Expected details to be invalid");
            Assert.Equal(ValidationMessages.TravelOrVisitTotalNumberOfCountriesRequired, validationResults.First().ErrorMessage);
        }

        [Theory, MemberData(nameof(BaseDetails))]
        public void ProvidingTotalNumberOfCountriesAlon_IsValid(ITravelOrVisitorDetails details)
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            details.TotalNumberOfCountries = 3;
    
            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            Assert.True(isValid, "Expected details to be valid");
        }

        [Theory, MemberData(nameof(BaseDetails))]
        public void ProvidingCountriesWithoutDurations_IsValid(ITravelOrVisitorDetails details)
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            details.TotalNumberOfCountries = 3;
            details.Country1Id = 1;
            details.Country2Id = 2;
            details.Country3Id = 3;
    
            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            Assert.True(isValid, "Expected details to be valid");
        }

        [Theory, MemberData(nameof(BaseDetails))]
        public void ProvidingDurationsWithoutCountries_IsNotValid(ITravelOrVisitorDetails details)
        {
            // Arrange
            var validationResults = new List<ValidationResult>();
            details.TotalNumberOfCountries = 3;
            details.StayLengthInMonths1 = 1;
            details.StayLengthInMonths2 = 2;
            details.StayLengthInMonths3 = 3;
    
            // Act
            var isValid = Validator.TryValidateObject(details, new ValidationContext(details), validationResults, true);

            // Assert
            Assert.False(isValid, "Expected details to be invalid");
            Assert.Equal(3, validationResults.Count);
            validationResults.ForEach(result =>
                Assert.Equal(ValidationMessages.TravelOrVisitDurationHasCountry, result.ErrorMessage));
        }
    }
}
