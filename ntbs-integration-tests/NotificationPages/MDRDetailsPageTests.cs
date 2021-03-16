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
                ["MDRDetails.RelationshipToCase"] = "123",
                ["MDRDetails.CountryId"] = "1",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("MDRDetails-RelationshipToCase", "relationship-description", "Relationship of the current case to the contact can only contain letters and the symbols ' - . ,");
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
        }

        [Fact]
        public async Task ValidateMDRDetailsProperty_ReturnsErrorIfDescriptionContainsNumbers()
        {
            // Arrange
            var initialPage = await Client.GetAsync(GetCurrentPathForId(Utilities.NOTIFIED_ID));
            var initialDocument = await GetDocumentAsync(initialPage);
            var request = new InputValidationModel()
            {
                Key = "RelationshipToCase",
                Value = "hello 123"
            };

            // Act
            
            var response = await Client.SendVerificationPostAsync(
                initialPage,
                initialDocument,
                GetHandlerPath(null, "ValidateMDRDetailsProperty", Utilities.NOTIFIED_ID),
                request);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Relationship of the current case to the contact can only contain letters and the symbols ' - . ,", result);
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
