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
    public class MDRDetailsPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditMDRDetails;

        public MDRDetailsPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task Post_ReturnsPageWithFieldsRequiredErrors_IfExposureYesSelected()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialPage = await Client.GetAsync(url);
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes"
            };

            // Act
            var result = await SendPostFormWithData(document, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("relationship-description", ValidationMessages.RelationshipToCaseIsRequired);
            resultDocument.AssertErrorMessage("case-in-uk", ValidationMessages.CaseInUKStatusIsRequired);
        }

        [Fact]
        public async Task Post_ReturnsPageWithErrors_IfInvalidValuesSubmitted()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialPage = await Client.GetAsync(url);
            var document = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.RelationshipToCase"] = "123",
                ["MDRDetails.CaseInUKStatus"] = "Yes",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.NEW_ID}",
            };

            // Act
            var result = await SendPostFormWithData(document, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);

            result.EnsureSuccessStatusCode();
            resultDocument.AssertErrorMessage("relationship-description", ValidationMessages.StandardStringFormat);
            resultDocument.AssertErrorMessage("related-notification", ValidationMessages.RelatedNotificationIdInvalid);
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
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Yes",
                ["MDRDetails.RelationshipToCase"] = "Cousins",
                ["MDRDetails.CaseInUKStatus"] = "Yes",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.NOTIFIED_ID}",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

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
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["MDRDetails.ExposureToKnownCaseStatus"] = "Unknown",
                ["MDRDetails.RelationshipToCase"] = "Cousins",
                ["MDRDetails.CaseInUKStatus"] = "Unknown",
                ["MDRDetails.RelatedNotificationId"] = $"{Utilities.NOTIFIED_ID}",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

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
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

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
            var result = await SendPostFormWithData(initialDocument, formData, url);

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
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

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
            var result = await SendPostFormWithData(initialDocument, formData, url);

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
            Assert.Equal(ValidationMessages.StandardStringFormat, result);
        }

        [Theory]
        [InlineData("10000", ValidationMessages.RelatedNotificationIdInvalid)]
        [InlineData("1e1", ValidationMessages.RelatedNotificationIdMustBeInteger)]
        public async Task ValidateMDRDetailsRelatedNotification_ReturnsErrorIfInvalidId(string attemptedId, string errorMessage)
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["value"] = attemptedId
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateMDRDetailsRelatedNotification"));

             // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(errorMessage, result);
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
    }
}
