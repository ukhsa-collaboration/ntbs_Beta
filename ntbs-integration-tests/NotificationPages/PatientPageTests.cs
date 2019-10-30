using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class PatientPageTests : TestRunnerBase
    {
        protected override string PageRoute => Routes.Patient;

        public PatientPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task PostDraft_ReturnsPageWithModelErrors_IfModelNotValid()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var pageContent = await GetDocumentAsync(initialPage);

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
            var result = await SendPostFormWithData(pageContent, formData);

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
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var pageContent = await GetDocumentAsync(initialPage);

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
            var result = await SendPostFormWithData(pageContent, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            Assert.Equal(FullErrorMessage(ValidationMessages.YearOfUkEntryMustBeAfterDob), resultDocument.GetError("year-of-entry"));
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithAllModelErrors_IfModelNotValid()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.NOTIFIED_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["Patient.NhsNumberNotKnown"] = "false",
                ["Patient.NoFixedAbode"] = "false",
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            Assert.Equal(FullErrorMessage(ValidationMessages.FamilyNameIsRequired), resultDocument.GetError("family-name"));
            Assert.Equal(FullErrorMessage(ValidationMessages.GivenNameIsRequired), resultDocument.GetError("given-name"));
            Assert.Contains(ValidationMessages.BirthDateIsRequired, resultDocument.GetError("dob"));
            Assert.Equal(FullErrorMessage(ValidationMessages.NHSNumberIsRequired), resultDocument.GetError("nhs-number"));
            Assert.Equal(FullErrorMessage(ValidationMessages.PostcodeIsRequired), resultDocument.GetError("postcode"));
            Assert.Equal(ValidationMessages.SexIsRequired, resultDocument.GetError("sex"));
            Assert.Equal(ValidationMessages.EthnicGroupIsRequired, resultDocument.GetError("ethnicity"));
            Assert.Equal(ValidationMessages.BirthCountryIsRequired, resultDocument.GetError("birth-country"));
        }

        [Fact]
        public async Task Post_RedirectsToNextPageAndSavesContent_IfModelValid()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var initialDocument = await GetDocumentAsync(initialPage);

            const string GivenName = "Test";
            const string FamilyName = "User";
            const string BirthDay = "5";
            const string BirthMonth = "5";
            const string BirthYear = "1992";
            const string NhsNumber = "1234567891";
            const string Address = "123 Fake Street, London";
            const string EthnicityId = "1";
            const string SexId = "2";
            const string CountryId = "3";
            const string LocalPatientId = "123#";
            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["Patient.GivenName"] = GivenName,
                ["Patient.FamilyName"] = FamilyName,
                ["FormattedDob.Day"] = BirthDay,
                ["FormattedDob.Month"] = BirthMonth,
                ["FormattedDob.Year"] = BirthYear,
                ["Patient.NhsNumber"] = NhsNumber,
                ["Patient.Address"] = Address,
                ["Patient.NoFixedAbode"] = "true",
                ["Patient.Postcode"] = "NW5 1TL",
                ["Patient.EthnicityId"] = EthnicityId,
                ["Patient.SexId"] = SexId,
                ["Patient.CountryId"] = CountryId,
                ["Patient.LocalPatientId"] = LocalPatientId
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(BuildEditRoute(Routes.Episode, Utilities.DRAFT_ID), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            Assert.Equal(GivenName, ((IHtmlInputElement)reloadedDocument.GetElementById("Patient_GivenName")).Value);
            Assert.Equal(FamilyName, ((IHtmlInputElement)reloadedDocument.GetElementById("Patient_FamilyName")).Value);
            Assert.Equal(BirthDay, ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDob_Day")).Value);
            Assert.Equal(BirthMonth, ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDob_Month")).Value);
            Assert.Equal(BirthYear, ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDob_Year")).Value);
            Assert.Equal(NhsNumber, ((IHtmlInputElement)reloadedDocument.GetElementById("Patient_NhsNumber")).Value);
            Assert.Equal(Address, ((IHtmlTextAreaElement)reloadedDocument.GetElementById("Patient_Address")).Value);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("Patient_Postcode")).Value);
            Assert.Equal(EthnicityId, ((IHtmlSelectElement)reloadedDocument.GetElementById("Patient_EthnicityId")).SelectedIndex.ToString());
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("sexId-2")).IsChecked);
            Assert.Equal(CountryId, ((IHtmlSelectElement)reloadedDocument.GetElementById("Patient_CountryId")).SelectedIndex.ToString());
            Assert.Equal(LocalPatientId, ((IHtmlInputElement)reloadedDocument.GetElementById("Patient_LocalPatientId")).Value);
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
            var response = await Client.GetAsync(BuildValidationPath(formData, "ValidatePatientDate"));

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
            var response = await Client.GetAsync(BuildValidationPath(formData, "ValidatePatientProperty"));

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
            var response = await Client.GetAsync(BuildValidationPath(formData, "ValidatePatientProperty"));

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
            var response = await Client.GetAsync(BuildValidationPath(formData, "ValidateYearOfUkEntry"));

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
            var response = await Client.GetAsync(BuildValidationPath(formData, "ValidateYearOfUkEntry"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(expectedValidationResult, result);
        }
    }
}
