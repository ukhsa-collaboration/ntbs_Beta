using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class ComorbidityPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditComorbidities;

        public ComorbidityPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task PostComorbidities_RedirectsToNextPageAndSavesContent()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.DRAFT_ID);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["ComorbidityDetails.DiabetesStatus"] = "Yes",
                ["ComorbidityDetails.HepatitisBStatus"] = "No",
                ["ComorbidityDetails.HepatitisCStatus"] = "Unknown",
                ["ComorbidityDetails.LiverDiseaseStatus"] = "No"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditSocialContextAddresses, Utilities.DRAFT_ID), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(url);
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("diabetes-radio-button-Yes")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("hepatitis-b-radio-button-No")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("hepatitis-c-radio-button-Unknown")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("liver-radio-button-No")).IsChecked);
            Assert.False(((IHtmlInputElement)reloadedDocument.GetElementById("renal-radio-button-Yes")).IsChecked);
            Assert.False(((IHtmlInputElement)reloadedDocument.GetElementById("renal-radio-button-No")).IsChecked);
            Assert.False(((IHtmlInputElement)reloadedDocument.GetElementById("renal-radio-button-Unknown")).IsChecked);
        }

        [Fact]
        public async Task PostImmunosuppression_ReturnsTypeRequiredError_IfYesSelected()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["ImmunosuppressionStatus"] = "Yes",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            resultDocument.AssertErrorSummaryMessage(
                "ImmunosuppressionDetails-Status",
                "status", 
                "At least one immunosuppression type must be selected");
        }

        [Fact]
        public async Task PostImmunosuppression_ReturnsDetailsRequiredError_IfOtherCheckedSelected()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["ImmunosuppressionStatus"] = "Yes",
                ["HasOther"] = "True",
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            resultDocument.AssertErrorSummaryMessage(
                "ImmunosuppressionDetails-OtherDescription", 
                "description",
                "Please supply other immunosuppression details");
        }

        [Fact]
        public async Task PostImmunosuppression_RedirectsToNextPageAndSavesContent()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            const string description = "Other Therapy";
            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["ImmunosuppressionStatus"] = "Yes",
                ["HasOther"] = "True",
                ["OtherDescription"] = description,
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditSocialContextAddresses, id), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(url);
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("immunosuppression-yes")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("HasOther")).IsChecked);
            Assert.Equal(description, ((IHtmlInputElement)reloadedDocument.GetElementById("OtherDescription")).Value);
        }

        [Theory]
        [InlineData("Unknown", "false", "", "")]
        [InlineData("No", "false", "", "")]
        [InlineData("Yes", "false", "", "At least one immunosuppression type must be selected")]
        [InlineData("Yes", "true", "", "Please supply other immunosuppression details")]
        [InlineData("Yes", "true", "Other", "")]
        public async Task ValidateImmunosuppression_ReturnsExpectedResult(string status, string isOther, string description, string validationResult)
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["immunosuppressionStatus"] = status,
                ["hasOther"] = isOther,
                ["otherDescription"] = description
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "ValidateImmunosuppression"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(result))
            {
                var mappedResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                result = result.Contains("Status") ? mappedResult["ImmunosuppressionDetails.Status"] : mappedResult["ImmunosuppressionDetails.OtherDescription"];
            }
            Assert.Equal(validationResult, result);
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
                ["ImmunosuppressionStatus"] = "Yes",
                ["HasOther"] = "True",
                ["OtherDescription"] = "test description",
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
