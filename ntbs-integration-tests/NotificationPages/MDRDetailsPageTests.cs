using System.Collections.Generic;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class MdrDetailsPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditMDRDetails;

        public MdrDetailsPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task Post_ReturnsPageWithFieldsRequiredErrors_IfExposureYesSelected()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("MDRDetails-RelationshipToCase", "relationship-description", "Please supply details of the relationship to case");
            resultDocument.AssertErrorSummaryMessage("MDRDetails-CountryId", "exposure-country", "Please select Country of exposure");
        }

        [Fact]
        public async Task Post_ReturnsPageWithErrors_IfInvalidValuesSubmitted()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.RelationshipToCase"] = "Some\r\nvalue",
                ["MDRDetails.CountryId"] = "1",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("MDRDetails-RelationshipToCase", "relationship-description", "Invalid character found in Relationship of the current case to the contact");
        }

        [Fact]
        public async Task Post_ReturnsPageWithErrors_IfNoDateSubmitted()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.CountryId"] = "1",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("MDRDetails-MDRTreatmentStartDate", "mdr-treatment-date", "MDR treatment start date is a mandatory field");
        }

        [Fact]
        public async Task Post_RedirectsToNextPageAndSavesContent_IfModelValid()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["FormattedMdrTreatmentDate.Day"] = "02",
                ["FormattedMdrTreatmentDate.Month"] = "03",
                ["FormattedMdrTreatmentDate.Year"] = "2021",
                ["MDRDetails.ExpectedTreatmentDurationInMonths"] = "23-24",
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.RelationshipToCase"] = "Cousins",
                ["MDRDetails.NotifiedToPheStatus"] = "Yes",
                ["MDRDetails.CountryId"] = "235",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.NOTIFIED_ID}",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditMBovisExposureToKnownCases, id));

            var reloadedPage = await Client.GetAsync(GetCurrentPathForId(id));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            reloadedDocument.AssertInputRadioValue("exposure-yes", true);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelationshipToCase", "Cousins");
            reloadedDocument.AssertInputTextValue("MDRDetails_RelatedNotificationId", $"{Utilities.NOTIFIED_ID}");
            reloadedDocument.AssertInputTextValue("FormattedMdrTreatmentDate_Day", "2");
            reloadedDocument.AssertInputTextValue("FormattedMdrTreatmentDate_Month", "3");
            reloadedDocument.AssertInputTextValue("FormattedMdrTreatmentDate_Year", "2021");
            reloadedDocument.AssertInputTextValue("MDRDetails_ExpectedTreatmentDurationInMonths", "23-24");
        }

        [Fact]
        public async Task Post_ClearsConditionalInputValues_IfExposureNotTrue()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["FormattedMdrTreatmentDate.Day"] = "02",
                ["FormattedMdrTreatmentDate.Month"] = "03",
                ["FormattedMdrTreatmentDate.Year"] = "2021",
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Unknown",
                ["MDRDetails.RelationshipToCase"] = "Cousins",
                ["MDRDetails.NotifiedToPheStatus"] = "Unknown",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.NOTIFIED_ID}",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditMBovisExposureToKnownCases, id));

            var reloadedPage = await Client.GetAsync(GetCurrentPathForId(id));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("exposure-unknown")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("MDRDetails_RelationshipToCase")).Value);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("MDRDetails_RelatedNotificationId")).Value);
            reloadedDocument.AssertInputRadioValue("exposure-unknown", true);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelationshipToCase", "");
            reloadedDocument.AssertInputTextValue("MDRDetails_RelatedNotificationId", "");
        }

        [Fact]
        public async Task Post_ClearsRelatedNotificationId_IfContactNotInUK()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["FormattedMdrTreatmentDate.Day"] = "02",
                ["FormattedMdrTreatmentDate.Month"] = "03",
                ["FormattedMdrTreatmentDate.Year"] = "2021",
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.RelationshipToCase"] = "Cousins",
                ["MDRDetails.NotifiedToPheStatus"] = "No",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.NOTIFIED_ID}",
                ["MDRDetails.CountryId"] = "1",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditMBovisExposureToKnownCases, id));

            var reloadedPage = await Client.GetAsync(GetCurrentPathForId(id));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            reloadedDocument.AssertInputRadioValue("exposure-yes", true);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelationshipToCase", "Cousins");
            reloadedDocument.AssertInputRadioValue("notified-no", false);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelatedNotificationId", "");
            reloadedDocument.AssertInputSelectValue("MDRDetails_CountryId", "1");
            reloadedDocument.AssertInputTextValue("FormattedMdrTreatmentDate_Day", "2");
            reloadedDocument.AssertInputTextValue("FormattedMdrTreatmentDate_Month", "3");
            reloadedDocument.AssertInputTextValue("FormattedMdrTreatmentDate_Year", "2021");
        }

        [Fact]
        public async Task RedirectsToOverviewWithCorrectAnchorFragment_ForNotified()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["FormattedMdrTreatmentDate.Day"] = "02",
                ["FormattedMdrTreatmentDate.Month"] = "03",
                ["FormattedMdrTreatmentDate.Year"] = "2021",
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.RelationshipToCase"] = "Cousins",
                ["MDRDetails.NotifiedToPheStatus"] = "No",
                ["MDRDetails.CountryId"] = "235",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.NOTIFIED_ID}",
            };

            // Act
            var result = await Client.SendPostFormWithData(document, formData, url);

            // Assert
            var sectionAnchorId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(NotificationSubPath);
            result.AssertRedirectTo($"/Notifications/{id}#{sectionAnchorId}");
        }

        [Fact]
        public async Task NotifiedPageHasReturnLinkToOverview()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);

            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var overviewLink = RouteHelper.GetNotificationOverviewPathWithSectionAnchor(id, NotificationSubPath);
            Assert.NotNull(document.QuerySelector($"a[href='{overviewLink}']"));
        }
    }
}
