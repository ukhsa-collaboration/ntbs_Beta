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
            Assert.Equal(FullErrorMessage(ValidationMessages.StandardStringFormat), resultDocument.GetError("given-name"));
            Assert.Equal(FullErrorMessage(ValidationMessages.StandardStringFormat), resultDocument.GetError("family-name"));
            // Cannot easily check for equality with FullErrorMessage here as the error field is formatted oddly due to there being two fields in the error span.
            Assert.Contains(ValidationMessages.DateValidityRange(ValidDates.EarliestBirthDate), resultDocument.GetError("dob"));
            Assert.Equal(FullErrorMessage(ValidationMessages.NhsNumberLength), resultDocument.GetError("nhs-number"));
            Assert.Equal(FullErrorMessage(ValidationMessages.StringWithNumbersAndForwardSlashFormat), resultDocument.GetError("address"));
            Assert.Equal(FullErrorMessage(ValidationMessages.InvalidCharacter), resultDocument.GetError("local-patient-id"));
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
            Assert.Equal(FullErrorMessage(ValidationMessages.YearOfUkEntryMustBeAfterDob), resultDocument.GetError("year-of-entry"));
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
            Assert.Equal(FullErrorMessage(ValidationMessages.FamilyNameIsRequired), resultDocument.GetError("family-name"));
            Assert.Equal(FullErrorMessage(ValidationMessages.GivenNameIsRequired), resultDocument.GetError("given-name"));
            Assert.Contains(ValidationMessages.BirthDateIsRequired, resultDocument.GetError("dob"));
            Assert.Equal(FullErrorMessage(ValidationMessages.NHSNumberIsRequired), resultDocument.GetError("nhs-number"));
            Assert.Equal(FullErrorMessage(ValidationMessages.PostcodeIsRequired), resultDocument.GetError("postcode"));
            Assert.Equal(FullErrorMessage(ValidationMessages.SexIsRequired), resultDocument.GetError("sex"));
            Assert.Equal(FullErrorMessage(ValidationMessages.EthnicGroupIsRequired), resultDocument.GetError("ethnicity"));
            Assert.Equal(FullErrorMessage(ValidationMessages.BirthCountryIsRequired), resultDocument.GetError("birth-country"));
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
                ["Patient.LocalPatientId"] = localPatientId
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditEpisode, id), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(url);
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            Assert.Equal(givenName, ((IHtmlInputElement)reloadedDocument.GetElementById("Patient_GivenName")).Value);
            Assert.Equal(familyName, ((IHtmlInputElement)reloadedDocument.GetElementById("Patient_FamilyName")).Value);
            Assert.Equal(birthDay, ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDob_Day")).Value);
            Assert.Equal(birthMonth, ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDob_Month")).Value);
            Assert.Equal(birthYear, ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDob_Year")).Value);
            Assert.Equal(nhsNumber, ((IHtmlInputElement)reloadedDocument.GetElementById("Patient_NhsNumber")).Value);
            Assert.Equal(address, ((IHtmlTextAreaElement)reloadedDocument.GetElementById("Patient_Address")).Value);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("Patient_Postcode")).Value);
            Assert.Equal(ethnicityId, ((IHtmlSelectElement)reloadedDocument.GetElementById("Patient_EthnicityId")).SelectedIndex.ToString());
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("sexId-2")).IsChecked);
            Assert.Equal(countryId, ((IHtmlSelectElement)reloadedDocument.GetElementById("Patient_CountryId")).SelectedIndex.ToString());
            Assert.Equal(localPatientId, ((IHtmlInputElement)reloadedDocument.GetElementById("Patient_LocalPatientId")).Value);
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

        [Theory]
        [InlineData("2100", ValidationMessages.YearOfUkEntryMustNotBeInFuture)]
        [InlineData("2010", "")]
        public async Task Validate_EntryToUkYear_ReturnsExpectedResult(string year, string validationResult)
        {
            // Arrange
            var formData = new Dictionary<string, string> { [""] = year };

            // Act
            var response = await Client.GetAsync(GetValidationPath(formData, "ValidateYearOfUkEntry"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(validationResult, result);
        }

        [Theory]
        [InlineData("1899")]
        [InlineData("2101")]
        public async Task Validate_EntryToUkYear_ReturnsExpectedResultForRange(string year)
        {
            // Arrange
            var expectedValidationResult = string.Format(ValidationMessages.ValidYearRange, null, 1900, 2100);
            var formData = new Dictionary<string, string> { [""] = year };

            // Act
            var response = await Client.GetAsync(GetValidationPath(formData, "ValidateYearOfUkEntry"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(expectedValidationResult, result);
        }
    }
}
