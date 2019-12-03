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
    public class ClinicalDetailsPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditClinicalDetails;

        public ClinicalDetailsPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task PostDraft_ReturnsPageWithAllModelErrors_IfModelNotValid()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialPage = await Client.GetAsync(url);
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
                ["FormattedFirstPresentationDate.Year"] = "2050",
                ["FormattedTbServicePresentationDate.Day"] = "1",
                ["FormattedTbServicePresentationDate.Month"] = "1",
                ["FormattedTbServicePresentationDate.Year"] = "2050",
                ["FormattedDiagnosisDate.Day"] = "1",
                ["FormattedDiagnosisDate.Month"] = "1",
                ["FormattedDiagnosisDate.Year"] = "2050",
                ["ClinicalDetails.DidNotStartTreatment"] = "false",
                ["FormattedTreatmentDate.Day"] = "1",
                ["ClinicalDetails.IsShortCourseTreatment"] = "true",
                ["ClinicalDetails.IsMDRTreatment"] = "true",
            };

            // Act
            var result = await SendPostFormWithData(document, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("other-site", "Site description can only contain letters and the symbols ' - . ,");
            resultDocument.AssertErrorMessage("bcg-vaccination", "BCG vaccination year has an invalid year");
            resultDocument.AssertErrorMessage("symptom", "Symptom onset date does not have a valid date selection");
            resultDocument.AssertErrorMessage("first-presentation", "Presentation to any health service must be today or earlier");
            resultDocument.AssertErrorMessage("tb-service-presentation", "Presentation to TB service must be today or earlier");
            resultDocument.AssertErrorMessage("diagnosis", "Diagnosis date must be today or earlier");
            resultDocument.AssertErrorMessage("treatment", "Treatment start date does not have a valid date selection");
            resultDocument.AssertErrorMessage("short-course", "Short course and MDR treatment cannot both be true");
            resultDocument.AssertErrorMessage("mdr", "Short course and MDR treatment cannot both be true");
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithAllModelErrors_IfModelNotValid()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_ID);
            var initialPage = await Client.GetAsync(url);
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                ["NotificationSiteMap[OTHER]"] = "true",
                ["ClinicalDetails.BCGVaccinationState"] = "Yes",
                ["ClinicalDetails.IsPostMortem"] = "true"
            };

            // Act
            var result = await SendPostFormWithData(document, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("other-site", "Site description is a mandatory field");
            resultDocument.AssertErrorMessage("bcg-vaccination", "BCG vaccination year is a mandatory field");
            resultDocument.AssertErrorMessage("postmortem", "Date of death is a mandatory field");
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithDiseaseSiteRequiredError_IfDiseaseSiteNotSet()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_ID);
            var initialPage = await Client.GetAsync(url);
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                // There is an enum conversion error when not sending any value for notificationSiteMap, so use false
                ["NotificationSiteMap[OTHER]"] = "false",
            };

            // Act
            var result = await SendPostFormWithData(document, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("notification-sites", "Please choose at least one site of disease");
        }

        [Fact]
        public async Task PostNotified_ReturnsPageWithDiagnosisDateRequiredError_IfDiagnosisDateNotSet()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_ID);
            var initialPage = await Client.GetAsync(url);
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
                // There is an enum conversion error when not sending any value for notificationSiteMap, so use false
                ["NotificationSiteMap[OTHER]"] = "false",
                ["FormattedDiagnosisDate.Day"] = "",
                ["FormattedDiagnosisDate.Month"] = "",
                ["FormattedDiagnosisDate.Year"] = ""
            };

            // Act
            var result = await SendPostFormWithData(document, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("diagnosis", "Diagnosis date is a mandatory field");
        }

        [Fact]
        public async Task Post_RedirectsToNextPageAndSavesContent_IfModelValid()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialPage = await Client.GetAsync(url);
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
                ["FormattedTbServicePresentationDate.Day"] = "3",
                ["FormattedTbServicePresentationDate.Month"] = "3",
                ["FormattedTbServicePresentationDate.Year"] = "2013",
                ["FormattedDiagnosisDate.Day"] = "4",
                ["FormattedDiagnosisDate.Month"] = "4",
                ["FormattedDiagnosisDate.Year"] = "2014",
                ["ClinicalDetails.IsPostMortem"] = "false",
                ["ClinicalDetails.IsShortCourseTreatment"] = "true",
                ["ClinicalDetails.IsMDRTreatment"] = "false",
                ["ClinicalDetails.DotStatus"] = "Yes",
                ["ClinicalDetails.EnhancedCaseManagementStatus"] = "No"
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditContactTracing, Utilities.DRAFT_ID), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(GetCurrentPathForId(Utilities.DRAFT_ID));
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
            Assert.Equal("3", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedTbServicePresentationDate_Day")).Value);
            Assert.Equal("3", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedTbServicePresentationDate_Month")).Value);
            Assert.Equal("2013", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedTbServicePresentationDate_Year")).Value);
            Assert.Equal("4", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDiagnosisDate_Day")).Value);
            Assert.Equal("4", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDiagnosisDate_Month")).Value);
            Assert.Equal("2014", ((IHtmlInputElement)reloadedDocument.GetElementById("FormattedDiagnosisDate_Year")).Value);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("postmortem-no")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("short-course-yes")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("mdr-no")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("dot-yes")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("enhanced-case-management-no")).IsChecked);
        }

        [Fact]
        public async Task Post_ClearsConditionalInputValues_IfRadiosSetToOtherValue()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialPage = await Client.GetAsync(url);
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
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditContactTracing, Utilities.DRAFT_ID), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(GetCurrentPathForId(Utilities.DRAFT_ID));
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
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateClinicalDetailsDate"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Diagnosis date does not have a valid date selection", result);
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
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateClinicalDetailsDate"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Diagnosis date must not be before 01/01/2010", result);
        }

        [Theory]
        [InlineData("true", "Please choose at least one site of disease")]
        [InlineData("false", "")]
        public async Task ValidateNotificationSites_ReturnsExpectedResult(string shouldValidateFull, string validationResult)
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["shouldValidateFull"] = shouldValidateFull
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateNotificationSites"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(validationResult, result);
        }

        [Theory]
        [InlineData("false", "123", "Site description can only contain letters and the symbols ' - . ,")]
        [InlineData("true", "", "Site description is a mandatory field")]
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
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateNotificationSiteProperty"));

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
                ["existingYear"] = existingYear.ToString(),
                ["propertyName"] = "BCG vaccination year"
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateClinicalDetailsYearComparison"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("BCG vaccination year should be later than birth year (1990)", result);
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
            var defaultUrl = GetCurrentPathForId(0);
            var response = await Client.GetAsync($"{defaultUrl}/ValidateClinicalDetailsProperties?{string.Join("&", keyValuePairs)}");

            // Assert check just response.Content
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Short course and MDR treatment cannot both be true", result);
        }

        [Fact]
        public async Task ValidateClinicalDetails_ReturnsModelError_WhenMDRTreatmentSetToNoWhenMdrDetailsFilledIn()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.MDR_DETAILS_EXIST);
            var initialPage = await Client.GetAsync(url);
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.MDR_DETAILS_EXIST.ToString(),
                ["NotificationSiteMap[PULMONARY]"] = "true",
                ["ClinicalDetails.IsMDRTreatment"] = "false"
            };

            // Act
            var result = await SendPostFormWithData(document, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("mdr", 
                "You cannot change the value of this field because an MDR Enhanced Surveillance Questionnaire exists. Please contact NTBS@phe.gov.uk");
        }
    }
}
