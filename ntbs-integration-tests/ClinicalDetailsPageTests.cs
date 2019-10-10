using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests
{
    public class ClinicalDetailsPageTests : TestRunnerBase
    {
        protected override string PageRoute
        {
            get { return Routes.ClinicalDetails; }
        }
        
        public ClinicalDetailsPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) {}

        [Fact]
        public async Task Post_ReturnsPageWithAllModelErrors_IfModelNotValid()
        {
            // Arrange
            var response = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var document = await GetDocumentAsync(response);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["NotificationSiteMap[OTHER]"] = "true",
                ["OtherSite.SiteDescription"] = "123",
                ["ClinicalDetails.BCGVaccinationState"] = "Yes",
                ["ClinicalDetails.BCGVaccinationYear"] = "1",
                ["FormattedSymptomDate.Day"] = "10",
                ["FormattedPresentationDate.Day"] = "1",
                ["FormattedPresentationDate.Month"] = "1",
                ["FormattedPresentationDate.Year"] = "2000",
                ["ClinicalDetails.IsShortCourseTreatment"] = "true",
                ["ClinicalDetails.IsMDRTreatment"] = "true",
            };

            // Act
            var result = await SendFormWithData(document, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            Assert.Equal(FullErrorMessage(ValidationMessages.StandardStringFormat), resultDocument.QuerySelector("span[id='other-site-error']").TextContent);
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidYear), resultDocument.QuerySelector("span[id='bcg-vaccination-error']").TextContent);
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidDate), resultDocument.QuerySelector("span[id='symptom-error']").TextContent);
            Assert.Equal(FullErrorMessage(ValidationMessages.DateValidityRange(ValidDates.EarliestClinicalDate)),
                resultDocument.QuerySelector("span[id='presentation-error']").TextContent);
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidTreatmentOptions), resultDocument.QuerySelector("span[id='short-course-error']").TextContent);
            Assert.Equal(FullErrorMessage(ValidationMessages.ValidTreatmentOptions), resultDocument.QuerySelector("span[id='mdr-error']").TextContent);
        }

        [Fact]
        public async Task Post_RedirectsToNextPageAndSavesContent_IfModelValid()
        {
            // Arrange
            var response = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var document = await GetDocumentAsync(response);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["NotificationSiteMap[PULMONARY]"] = "true",
                ["NotificationSiteMap[OTHER]"] = "true",
                ["OtherSite.SiteDescription"] = "tissue",
                ["ClinicalDetails.NoSampleTaken"] = "true",
                ["ClinicalDetails.BCGVaccinationState"] = "Yes",
                ["ClinicalDetails.BCGVaccinationYear"] = "2000",
                ["FormattedPresentationDate.Day"] = "1",
                ["FormattedPresentationDate.Month"] = "1",
                ["FormattedPresentationDate.Year"] = "2012",
                ["ClinicalDetails.IsPostMortem"] = "false",
                ["ClinicalDetails.IsShortCourseTreatment"] = "true",
                ["ClinicalDetails.IsMDRTreatment"] = "false",
            };

            // Act
            var result = await SendFormWithData(document, formData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(result.Headers.Location.OriginalString, BuildRoute(Routes.ContactTracing, Utilities.DRAFT_ID));

            response = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            document = await GetDocumentAsync(response);
            Assert.True(((IHtmlInputElement)document.GetElementById("NotificationSiteMap_PULMONARY_")).IsChecked);
            Assert.True(((IHtmlInputElement)document.GetElementById("NotificationSiteMap_OTHER_")).IsChecked);
            Assert.Equal("tissue", ((IHtmlInputElement)document.GetElementById("OtherSite_SiteDescription")).Value);
            Assert.True(((IHtmlInputElement)document.GetElementById("ClinicalDetails_NoSampleTaken")).IsChecked);
            Assert.True(((IHtmlInputElement)GetRadioWithValue(document, "ClinicalDetails_BCGVaccinationState", "Yes")).IsChecked);
            Assert.Equal("2000", ((IHtmlInputElement)document.GetElementById("ClinicalDetails_BCGVaccinationYear")).Value);
            Assert.Equal("1", ((IHtmlInputElement)document.GetElementById("FormattedPresentationDate_Day")).Value);
            Assert.Equal("1", ((IHtmlInputElement)document.GetElementById("FormattedPresentationDate_Month")).Value);
            Assert.Equal("2012", ((IHtmlInputElement)document.GetElementById("FormattedPresentationDate_Year")).Value);
            Assert.True(((IHtmlInputElement)GetRadioWithValue(document, "ClinicalDetails_IsPostMortem", "false")).IsChecked);
            Assert.True(((IHtmlInputElement)GetRadioWithValue(document, "ClinicalDetails_IsShortCourseTreatment", "true")).IsChecked);
            Assert.True(((IHtmlInputElement)GetRadioWithValue(document, "ClinicalDetails_IsMDRTreatment", "false")).IsChecked);
        }

        [Fact]
        public async Task Post_ClearsConditionalInputValues_IfRadiosSetToOtherValue()
        {
            // Arrange
            var response = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var document = await GetDocumentAsync(response);

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
                ["ClinicalDetails.IsPostMortem"] = "false"
            };

            // Act
            var result = await SendFormWithData(document, formData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(result.Headers.Location.OriginalString, BuildRoute(Routes.ContactTracing, Utilities.DRAFT_ID));

            response = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            document = await GetDocumentAsync(response);
            Assert.False(((IHtmlInputElement)document.GetElementById("NotificationSiteMap_OTHER_")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)document.GetElementById("OtherSite_SiteDescription")).Value);
            Assert.True(((IHtmlInputElement)GetRadioWithValue(document, "ClinicalDetails_BCGVaccinationState", "No")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)document.GetElementById("ClinicalDetails_BCGVaccinationYear")).Value);
            Assert.True(((IHtmlInputElement)GetRadioWithValue(document, "ClinicalDetails_DidNotStartTreatment", "true")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)document.GetElementById("FormattedTreatmentDate_Day")).Value);
            Assert.Equal("", ((IHtmlInputElement)document.GetElementById("FormattedTreatmentDate_Month")).Value);
            Assert.Equal("", ((IHtmlInputElement)document.GetElementById("FormattedTreatmentDate_Year")).Value);
            Assert.True(((IHtmlInputElement)GetRadioWithValue(document, "ClinicalDetails_IsPostMortem", "false")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)document.GetElementById("FormattedDeathDate_Day")).Value);
            Assert.Equal("", ((IHtmlInputElement)document.GetElementById("FormattedDeathDate_Month")).Value);
            Assert.Equal("", ((IHtmlInputElement)document.GetElementById("FormattedDeathDate_Year")).Value);
        }

        [Fact]
        public async Task ValidateDate_ReturnsCorrectErrorMessage()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["key"] = "DiagnosisDate",
                ["day"] = "1",
                ["month"] = "1",
                ["year"] = "1"
            };

            // Act
            var response = await client.GetAsync(BuildValidationPath(formData, "ValidateClinicalDetailsDate"));
            var result = (await response.Content.ReadAsStringAsync());
            Assert.Equal(ValidationMessages.DateValidityRange(ValidDates.EarliestClinicalDate), result);
        } 

        // TODO: Tests for full validation, all onget validate methods
    }
}
