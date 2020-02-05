using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class MDRDetailsPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditMDRDetails;

        public MDRDetailsPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task Post_ReturnsPageWithFieldsRequiredErrors_IfExposureYesSelected()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrl(url);

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
            resultDocument.AssertErrorSummaryMessage("MDRDetails-CaseInUKStatus", "case-in-uk", "Please specify whether the contact was a case in the UK");
        }

        [Fact]
        public async Task Post_ReturnsPageWithErrors_IfInvalidValuesSubmitted()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.RelationshipToCase"] = "123",
                ["MDRDetails.CaseInUKStatus"] = "Yes",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.DRAFT_ID}",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorSummaryMessage("MDRDetails-RelationshipToCase", "relationship-description", "Relationship of the current case to the contact can only contain letters and the symbols ' - . ,");
            resultDocument.AssertErrorSummaryMessage("MDRDetails-RelatedNotificationId", "related-notification", "The NTBS ID does not match an existing ID in the system");
        }

        [Fact]
        public async Task Post_RedirectsToNextPageAndSavesContent_IfModelValid()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.RelationshipToCase"] = "Cousins",
                ["MDRDetails.CaseInUKStatus"] = "Yes",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.NOTIFIED_ID}",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditMDRDetails, Utilities.DRAFT_ID), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(GetCurrentPathForId(Utilities.DRAFT_ID));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            reloadedDocument.AssertInputRadioValue("exposure-yes", true);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelationshipToCase", "Cousins");
            reloadedDocument.AssertInputRadioValue("uk-yes", true);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelatedNotificationId", $"{Utilities.NOTIFIED_ID}");
        }

        [Fact]
        public async Task Post_ClearsConditionalInputValues_IfExposureNotTrue()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Unknown",
                ["MDRDetails.RelationshipToCase"] = "Cousins",
                ["MDRDetails.CaseInUKStatus"] = "Unknown",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.NOTIFIED_ID}",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditMDRDetails, Utilities.DRAFT_ID), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(GetCurrentPathForId(Utilities.DRAFT_ID));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("exposure-unknown")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("MDRDetails_RelationshipToCase")).Value);
            Assert.False(((IHtmlInputElement)reloadedDocument.GetElementById("uk-unknown")).IsChecked);
            Assert.Equal("", ((IHtmlInputElement)reloadedDocument.GetElementById("MDRDetails_RelatedNotificationId")).Value);
            reloadedDocument.AssertInputRadioValue("exposure-unknown", true);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelationshipToCase", "");
            reloadedDocument.AssertInputRadioValue("uk-unknown", false);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelatedNotificationId", "");
        }

        [Fact]
        public async Task Post_ClearsRelatedNotificationId_IfContactNotInUK()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.RelationshipToCase"] = "Cousins",
                ["MDRDetails.CaseInUKStatus"] = "No",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.NOTIFIED_ID}",
                ["MDRDetails.CountryId"] = "1",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditMDRDetails, Utilities.DRAFT_ID), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(GetCurrentPathForId(Utilities.DRAFT_ID));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            reloadedDocument.AssertInputRadioValue("exposure-yes", true);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelationshipToCase", "Cousins");
            reloadedDocument.AssertInputRadioValue("uk-no", true);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelatedNotificationId", "");
            reloadedDocument.AssertInputSelectValue("MDRDetails_CountryId", "1");
        }

        [Fact]
        public async Task Post_ClearsCountry_IfContactInUK()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.RelationshipToCase"] = "Cousins",
                ["MDRDetails.CaseInUKStatus"] = "Yes",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.NOTIFIED_ID}",
                ["MDRDetails.CountryId"] = "1",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditMDRDetails, Utilities.DRAFT_ID), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(GetCurrentPathForId(Utilities.DRAFT_ID));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            reloadedDocument.AssertInputRadioValue("exposure-yes", true);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelationshipToCase", "Cousins");
            reloadedDocument.AssertInputRadioValue("uk-yes", true);
            reloadedDocument.AssertInputTextValue("MDRDetails_RelatedNotificationId", $"{Utilities.NOTIFIED_ID}");
            reloadedDocument.AssertInputSelectValue("MDRDetails_CountryId", null);
        }

        [Fact]
        public async Task ValidateMDRDetailsProperty_ReturnsErrorIfDescriptionContainsNumbers()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["key"] = "RelationshipToCase",
                ["value"] = "hello 123"
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateMDRDetailsProperty"));

             // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal("Relationship of the current case to the contact can only contain letters and the symbols ' - . ,", result);
        }

        [Theory]
        [InlineData(Utilities.DRAFT_ID)]
        [InlineData(Utilities.DENOTIFIED_ID)]
        [InlineData(Utilities.NEW_ID)]
        public async Task ValidateMDRDetailsRelatedNotification_ReturnsErrorIfInvalidId(int attemptedId)
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["value"] = $"{attemptedId}"
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateMDRDetailsRelatedNotification"));

             // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("The NTBS ID does not match an existing ID in the system", result);
        }

        [Fact]
        public async Task ValidateMDRDetailsRelatedNotification_ReturnsErrorIfIdNotInteger()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["value"] = "1e1"
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateMDRDetailsRelatedNotification"));

             // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("The NTBS ID must be an integer", result);
        }

        [Fact]
        public async Task ValidateMDRDetailsRelatedNotification_ReturnsNotificationInfoIfValidId()
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["value"] = $"{Utilities.NOTIFIED_ID}"
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateMDRDetailsRelatedNotification"));

             // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Contains("Name", result);
            Assert.Contains("Dob", result);
        }
        
        [Fact]
        public async Task RedirectsToOverviewWithCorrectAnchorFragment_ForNotified()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.RelationshipToCase"] = "Cousins",
                ["MDRDetails.CaseInUKStatus"] = "Yes",
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
            var document = await GetDocumentForUrl(url);

            // Assert
            var overviewLink = RouteHelper.GetNotificationOverviewPathWithSectionAnchor(id, NotificationSubPath);
            Assert.NotNull(document.QuerySelector($"a[href='{overviewLink}']"));
        }
    }
}
