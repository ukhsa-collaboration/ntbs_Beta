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
    public class ClinicalDetailsPageTests : TestRunnerBase
    {
        protected override string PageRoute => Routes.ClinicalDetails;

        public ClinicalDetailsPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) {}

        [Fact]
        public async Task PostDraft_ReturnsPageWithAllModelErrors_IfModelNotValid()
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["NotificationSiteMap[OTHER]"] = "true",
                ["OtherSite.SiteDescription"] = "123",
                ["ClinicalDetails.BCGVaccinationState"] = "Yes",
                ["ClinicalDetails.BCGVaccinationYear"] = "1",
                ["ClinicalDetails.IsSymptomatic"] = "true",
                ["FormattedSymptomDate.Day"] = "10",
                ["FormattedFirstPresentationDate.Day"] = "1",
                ["FormattedFirstPresentationDate.Month"] = "1",
                ["FormattedFirstPresentationDate.Year"] = "2000",
                ["FormattedTBServicePresentationDate.Day"] = "1",
                ["FormattedTBServicePresentationDate.Month"] = "1",
                ["FormattedTBServicePresentationDate.Year"] = "2000",
                ["FormattedDiagnosisDate.Day"] = "1",
                ["FormattedDiagnosisDate.Month"] = "1",
                ["FormattedDiagnosisDate.Year"] = "2000",
                ["ClinicalDetails.DidNotStartTreatment"] = "false",
                ["FormattedTreatmentDate.Day"] = "1",
                ["ClinicalDetails.IsShortCourseTreatment"] = "true",
                ["ClinicalDetails.IsMDRTreatment"] = "true",
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            Assert.Equal(FullErrorMessage(ValidationMessages.StandardStringFormat), resultDocument.GetError("other-site"));
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidYear), resultDocument.GetError("bcg-vaccination"));
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidDate), resultDocument.GetError("symptom"));
            Assert.Equal(FullErrorMessage(ValidationMessages.DateValidityRange(ValidDates.EarliestClinicalDate)),
                resultDocument.GetError("first-presentation"));
            Assert.Equal(FullErrorMessage(ValidationMessages.DateValidityRange(ValidDates.EarliestClinicalDate)),
                resultDocument.GetError("tb-service-presentation"));
            Assert.Equal(FullErrorMessage(ValidationMessages.DateValidityRange(ValidDates.EarliestClinicalDate)),
                resultDocument.GetError("diagnosis"));
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidDate), resultDocument.GetError("treatment"));
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidTreatmentOptions), resultDocument.GetError("short-course"));
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidTreatmentOptions), resultDocument.GetError("mdr"));
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithAllModelErrors_IfModelNotValid()
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(Utilities.NOTIFIED_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["NotificationSiteMap[OTHER]"] = "true",
                ["ClinicalDetails.BCGVaccinationState"] = "Yes",
                ["ClinicalDetails.IsPostMortem"] = "true"
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            Assert.Equal(FullErrorMessage(ValidationMessages.DiseaseSiteOtherIsRequired), resultDocument.GetError("other-site"));
            Assert.Equal(FullErrorMessage(ValidationMessages.BCGYearIsRequired), resultDocument.GetError("bcg-vaccination"));
            Assert.Equal(FullErrorMessage(ValidationMessages.DeathDateIsRequired), resultDocument.GetError("postmortem"));
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithDiseaseSiteRequiredError_IfDiseaseSiteNotSet()
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(Utilities.NOTIFIED_ID));
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                // There is an enum conversion error when not sending any value for notificationSiteMap, so use false
                ["NotificationSiteMap[OTHER]"] = "false",
            };

            // Act
            var result = await SendPostFormWithData(document, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            Assert.Equal(ValidationMessages.DiseaseSiteIsRequired, resultDocument.GetError("notification-sites"));
        }

        [Fact]
        public async Task Post_RedirectsToNextPageAndSavesContent_IfModelValid()
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["NotificationSiteMap[PULMONARY]"] = "true",
                ["NotificationSiteMap[OTHER]"] = "true",
                ["OtherSite.SiteDescription"] = "tissue",
                ["ClinicalDetails.NoSampleTaken"] = "true",
                ["ClinicalDetails.BCGVaccinationState"] = "Yes",
                ["ClinicalDetails.BCGVaccinationYear"] = "2000",
                ["ClinicalDetails.IsSymptomatic"] = "true",
                ["FormattedSymptomDate.Day"] = "1",
                ["FormattedSymptomDate.Month"] = "1",
                ["FormattedSymptomDate.Year"] = "2011",
                ["FormattedFirstPresentationDate.Day"] = "2",
                ["FormattedFirstPresentationDate.Month"] = "2",
                ["FormattedFirstPresentationDate.Year"] = "2012",
                ["FormattedTBServicePresentationDate.Day"] = "3",
                ["FormattedTBServicePresentationDate.Month"] = "3",
                ["FormattedTBServicePresentationDate.Year"] = "2013",
                ["FormattedDiagnosisDate.Day"] = "4",
                ["FormattedDiagnosisDate.Month"] = "4",
                ["FormattedDiagnosisDate.Year"] = "2014",
                ["ClinicalDetails.IsPostMortem"] = "false",
                ["ClinicalDetails.IsShortCourseTreatment"] = "true",
                ["ClinicalDetails.IsMDRTreatment"] = "false",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(BuildEditRoute(Routes.ContactTracing, Utilities.DRAFT_ID), GetRedirectLocation(result));

            var reloadedPage = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("NotificationSiteMap_PULMONARY_")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("NotificationSiteMap_OTHER_")).IsChecked);
            Assert.Equal("tissue", ((IHtmlInputElement)reloadedDocument.GetElementById("OtherSite_SiteDescription")).Value);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("ClinicalDetails_NoSampleTaken")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("bcg-vaccination-yes")).IsChecked);
            Assert.Equal("2000", ((IHtmlInputElement)reloadedDocument.GetElementById("ClinicalDetails_BCGVaccinationYear")).Value);
            Assert.Equal("1", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedSymptomDate_Day")).Value);
            Assert.Equal("1", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedSymptomDate_Month")).Value);
            Assert.Equal("2011", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedSymptomDate_Year")).Value);
            Assert.Equal("2", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedFirstPresentationDate_Day")).Value);
            Assert.Equal("2", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedFirstPresentationDate_Month")).Value);
            Assert.Equal("2012", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedFirstPresentationDate_Year")).Value);
            Assert.Equal("3", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedTBServicePresentationDate_Day")).Value);
            Assert.Equal("3", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedTBServicePresentationDate_Month")).Value);
            Assert.Equal("2013", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedTBServicePresentationDate_Year")).Value);
            Assert.Equal("4", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDiagnosisDate_Day")).Value);
            Assert.Equal("4", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDiagnosisDate_Month")).Value);
            Assert.Equal("2014", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDiagnosisDate_Year")).Value);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("postmortem-no")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("short-course-yes")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("mdr-no")).IsChecked);
        }

        [Fact]
        public async Task Post_ClearsConditionalInputValues_IfRadiosSetToOtherValue()
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["NotificationSiteMap[OTHER]"] = "false",
                ["OtherSite.SiteDescription"] = "tissue",
                ["ClinicalDetails.BCGVaccinationState"] = "No",
                ["ClinicalDetails.BCGVaccinationYear"] = "2000",
                ["FormattedTreatmentDate.Day"] = "1",
                ["FormattedTreatmentDate.Month"] = "1",
                ["FormattedTreatmentDate.Year"] = "2012",
                ["ClinicalDetails.DidNotStartTreatment"] = "true",
                ["FormattedDeathDate.Day"] = "1",
                ["FormattedDeathDate.Month"] = "1",
                ["FormattedDeathDate.Year"] = "2012",
                ["ClinicalDetails.IsPostMortem"] = "false",
                ["FormattedSymptomDate.Day"] = "10",
                ["FormattedSymptomDate.Month"] = "10",
                ["FormattedSymptomDate.Year"] = "2012",
                ["ClinicalDetails.IsSymptomatic"] = "false"
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(BuildEditRoute(Routes.ContactTracing, Utilities.DRAFT_ID), result.Headers.Location.OriginalString);

            var reloadedPage = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            Assert.False(((IHtmlInputElement)reloadedDocument.GetElementById("NotificationSiteMap_OTHER_")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("OtherSite_SiteDescription")).Value);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("bcg-vaccination-no")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("ClinicalDetails_BCGVaccinationYear")).Value);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("treatment-no")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedTreatmentDate_Day")).Value);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedTreatmentDate_Month")).Value);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedTreatmentDate_Year")).Value);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("postmortem-no")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDeathDate_Day")).Value);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDeathDate_Month")).Value);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDeathDate_Year")).Value);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("symptomatic-no")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedSymptomDate_Day")).Value);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedSymptomDate_Month")).Value);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedSymptomDate_Year")).Value);
        }

        [Fact]
        public async Task IfInvalidDate_ValidateClinicalDetailsDate_ReturnsValidDateErrorMessage()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["key"] = "DiagnosisDate",
                ["day"] = "1",
                ["month"] = "0",
                ["year"] = "2009"
            };

            // Act
            var response = await client.GetAsync(BuildValidationPath(formData, "ValidateClinicalDetailsDate"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(ValidationMessages.ValidDate, result);
        }

        [Fact]
        public async Task IfDateTooEarly_ValidateClinicalDetailsDate_ReturnsEarliestClinicalDateErrorMessage()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["key"] = "DiagnosisDate",
                ["day"] = "1",
                ["month"] = "1",
                ["year"] = "2009"
            };

            // Act
            var response = await client.GetAsync(BuildValidationPath(formData, "ValidateClinicalDetailsDate"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(ValidationMessages.DateValidityRange(ValidDates.EarliestClinicalDate), result);
        }

        [Theory]
        [InlineData("true", ValidationMessages.DiseaseSiteIsRequired)]
        [InlineData("false", "")]
        public async Task ValidateNotificationSites_ReturnsExpectedResult(string shouldValidateFull, string validationResult)
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["shouldValidateFull"] = shouldValidateFull
            };

            // Act
            var response = await client.GetAsync(BuildValidationPath(formData, "ValidateNotificationSites"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(validationResult, result);
        }

        [Theory]
        [InlineData("false", "123", ValidationMessages.StandardStringFormat)]
        [InlineData("true", "", ValidationMessages.DiseaseSiteOtherIsRequired)]
        [InlineData("false", "", "")]
        public async Task ValidateNotificationSiteProperty_ReturnsExpectedResult(string shouldValidateFull, string value, string validationResult)
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["key"] = "SiteDescription",
                ["value"] = value,
                ["shouldValidateFull"] = shouldValidateFull
            };

            // Act
            var response = await client.GetAsync(BuildValidationPath(formData, "ValidateNotificationSiteProperty"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(validationResult, result);
        }

        [Fact]
        public async Task ValidateClinicalDetailsYearComparison_ReturnsErrorIfNewYearEarlierThanExisting()
        {
             // Arrange
            var existingYear = 1990;
            var formData = new Dictionary<string, string>
            {
                ["newYear"] = "1960",
                ["existingYear"] = existingYear.ToString()
            };

            // Act
            var response = await client.GetAsync(BuildValidationPath(formData, "ValidateClinicalDetailsYearComparison"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(ValidationMessages.ValidYearLaterThanBirthYear(existingYear), result);
        }

        [Fact]
        public async Task ValidateClinicalDetailsProperties_ReturnsErrorIfBothTreatmentsSetToTrue()
        {
             // Arrange
            var keyValuePairs = new string[]
            {
                "keyValuePairs[0][key]=IsShortCourseTreatment",
                "keyValuePairs[0][value]=true",
                "keyValuePairs[1][key]=IsMDRTreatment",
                "keyValuePairs[1][value]=true"
            };

            // Act
            var response = await client.GetAsync($"{PageRoute}/ValidateClinicalDetailsProperties?{string.Join("&", keyValuePairs)}");

            // Assert check just response.Content
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains(ValidationMessages.ValidTreatmentOptions, result);
        }
    }
}
