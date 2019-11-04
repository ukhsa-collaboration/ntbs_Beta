using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class PatientPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditPatient;

        public PatientPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task PostDraft_ReturnsPageWithModelErrors_IfModelNotValid()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["Patient.GivenName"] = "111",
                ["Patient.FamilyName"] = "111",
                ["FormattedDob.Day"] = "31",
                ["FormattedDob.Month"] = "12",
                ["FormattedDob.Year"] = "1899",
                ["Patient.NhsNumber"] = "123",
                ["Patient.Address"] = "$$$",
                ["Patient.LocalPatientId"] = "|||"
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("given-name", ValidationMessages.StandardStringFormat);
            resultDocument.AssertErrorMessage("family-name", ValidationMessages.StandardStringFormat);
            // Cannot easily check for equality with FullErrorMessage here as the error field is formatted oddly due to there being two fields in the error span.
            Assert.Contains(ValidationMessages.DateValidityRange(ValidDates.EarliestBirthDate), resultDocument.GetError("dob"));
            resultDocument.AssertErrorMessage("nhs-number", ValidationMessages.NhsNumberLength);
            resultDocument.AssertErrorMessage("address", ValidationMessages.StringWithNumbersAndForwardSlashFormat);
            resultDocument.AssertErrorMessage("local-patient-id", ValidationMessages.InvalidCharacter);
        }

        [Fact]
        public async Task PostDraft_ReturnsPageWithModelErrors_IfYearOfUkEntryBeforeDob()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["FormattedDob.Day"] = "31",
                ["FormattedDob.Month"] = "12",
                ["FormattedDob.Year"] = "2000",
                ["Patient.CountryId"] = "1",
                ["Patient.YearOfUkEntry"] = "1999"
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("year-of-entry", ValidationMessages.YearOfUkEntryMustBeAfterDob);
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithAllModelErrors_IfModelNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["Patient.NhsNumberNotKnown"] = "false",
                ["Patient.NoFixedAbode"] = "false",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();

            resultDocument.AssertErrorMessage("family-name", ValidationMessages.FamilyNameIsRequired);
            resultDocument.AssertErrorMessage("given-name", ValidationMessages.GivenNameIsRequired);
            Assert.Contains(ValidationMessages.BirthDateIsRequired, resultDocument.GetError("dob"));
            resultDocument.AssertErrorMessage("nhs-number", ValidationMessages.NHSNumberIsRequired);
            resultDocument.AssertErrorMessage("postcode", ValidationMessages.PostcodeIsRequired);
            resultDocument.AssertErrorMessage("sex", ValidationMessages.SexIsRequired);
            resultDocument.AssertErrorMessage("ethnicity", ValidationMessages.EthnicGroupIsRequired);
            resultDocument.AssertErrorMessage("birth-country", ValidationMessages.BirthCountryIsRequired);
        }

        [Fact]
        public async Task Post_RedirectsToNextPageAndSavesContent_IfModelValid()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            const string givenName = "Test";
            const string familyName = "User";
            const string birthDay = "5";
            const string birthMonth = "5";
            const string birthYear = "1992";
            const string nhsNumber = "1234567891";
            const string address = "123 Fake Street, London";
            const string ethnicityId = "1";
            const string sexId = "2";
            const string countryId = "3";
            const string localPatientId = "123#";
            const string occupationId = "1";
            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["Patient.GivenName"] = givenName,
                ["Patient.FamilyName"] = familyName,
                ["FormattedDob.Day"] = birthDay,
                ["FormattedDob.Month"] = birthMonth,
                ["FormattedDob.Year"] = birthYear,
                ["Patient.NhsNumber"] = nhsNumber,
                ["Patient.Address"] = address,
                ["Patient.NoFixedAbode"] = "true",
                ["Patient.Postcode"] = "NW5 1TL",
                ["Patient.EthnicityId"] = ethnicityId,
                ["Patient.SexId"] = sexId,
                ["Patient.CountryId"] = countryId,
                ["Patient.LocalPatientId"] = localPatientId,
                ["Patient.OccupationId"] = occupationId
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditEpisode, id), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(url);
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            reloadedDocument.AssertInputTextValue("Patient_GivenName", givenName);
            reloadedDocument.AssertInputTextValue("Patient_FamilyName", familyName);
            reloadedDocument.AssertInputTextValue("FormattedDob_Day", birthDay);
            reloadedDocument.AssertInputTextValue("FormattedDob_Month", birthMonth);
            reloadedDocument.AssertInputTextValue("FormattedDob_Year", birthYear);
            reloadedDocument.AssertInputTextValue("Patient_NhsNumber", nhsNumber);
            reloadedDocument.AssertInputTextValue("Patient_Postcode", string.Empty);
            reloadedDocument.AssertInputTextValue("Patient_LocalPatientId", localPatientId);
            reloadedDocument.AssertInputSelectValue("Patient_EthnicityId", ethnicityId);
            reloadedDocument.AssertInputSelectValue("Patient_CountryId", countryId);
            reloadedDocument.AssertInputSelectValue("Patient_OccupationId", occupationId);
            reloadedDocument.AssertInputRadioValue("sexId-2", true);
            reloadedDocument.AssertTextAreaValue("Patient_Address", address);
        }

        [Fact]
        public async Task IfDateTooEarly_ValidatePatientDate_ReturnsEarliestBirthDateErrorMessage()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["key"] = "Dob",
                ["day"] = "1",
                ["month"] = "1",
                ["year"] = "1899"
            };

            // Act
            var response = await Client.GetAsync(GetValidationPath(formData, "ValidatePatientDate"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(ValidationMessages.DateValidityRange(ValidDates.EarliestBirthDate), result);
        }

        [Theory]
        [InlineData("ABC", ValidationMessages.NhsNumberFormat)]
        [InlineData("123", ValidationMessages.NhsNumberLength)]
        public async Task WhenNhsNumberInvalid_ValidatePatientProperty_ReturnsExpectedResult(string nhsNumber, string validationResult)
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["key"] = "NhsNumber",
                ["value"] = nhsNumber
            };

            // Act
            var response = await Client.GetAsync(GetValidationPath(formData, "ValidatePatientProperty"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(validationResult, result);
        }

        [Theory]
        [InlineData("true", ValidationMessages.NHSNumberIsRequired)]
        [InlineData("false", "")]
        public async Task DependentOnShouldValidateFull_ValidatePatientProperty_ReturnsRequiredOrNoError(string shouldValidateFull, string validationResult)
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["shouldValidateFull"] = shouldValidateFull,
                ["key"] = "NhsNumber"
            };

            // Act
            var response = await Client.GetAsync(GetValidationPath(formData, "ValidatePatientProperty"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(validationResult, result);
        }
    }
}
