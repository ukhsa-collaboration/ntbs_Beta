using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class ImmunosuppressionPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditImmunosuppression;

        public ImmunosuppressionPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task Post_ReturnsTypeRequiredError_IfYesSelected()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["ImmunosuppressionDetails.Status"] = "Yes",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            resultDocument.AssertErrorMessage("status", "At least one field must be selected");
        }

        [Fact]
        public async Task Post_ReturnsDetailsRequiredError_IfOtherCheckedSelected()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["ImmunosuppressionDetails.Status"] = "Yes",
                ["ImmunosuppressionDetails.HasOther"] = "True",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            resultDocument.AssertErrorMessage("description", "Please supply immunosuppression other details");
        }

        [Fact]
        public async Task Post_RedirectsToNextPageAndSavesContent()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(id);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);

            const string description = "Other Therapy";
            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = id.ToString(),
                ["ImmunosuppressionDetails.Status"] = "Yes",
                ["ImmunosuppressionDetails.HasOther"] = "True",
                ["ImmunosuppressionDetails.OtherDescription"] = description,
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetPathForId(NotificationSubPaths.EditPreviousHistory, id), GetRedirectLocation(result));

            var reloadedPage = await Client.GetAsync(url);
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("immunosuppression-yes")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("ImmunosuppressionDetails_HasOther")).IsChecked);
            Assert.Equal(description, ((IHtmlInputElement)reloadedDocument.GetElementById("ImmunosuppressionDetails_OtherDescription")).Value);
        }

        [Theory]
        [InlineData("Unknown", "false", "", "")]
        [InlineData("No", "false", "", "")]
        [InlineData("Yes", "false", "", ValidationMessages.ImmunosuppressionTypeRequired)]
        [InlineData("Yes", "true", "", ValidationMessages.ImmunosuppressionDetailRequired)]
        [InlineData("Yes", "true", "Other", "")]
        public async Task Validate_ReturnsExpectedResult(string status, string isOther, string description, string validationResult)
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["status"] = status,
                ["hasOther"] = isOther,
                ["otherDescription"] = description
            };

            // Act
            var response = await Client.GetAsync(GetHandlerPath(formData, "Validate"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(result))
            {
                var mappedResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                result = result.Contains("Status") ? mappedResult["ImmunosuppressionDetails.Status"] : mappedResult["ImmunosuppressionDetails.OtherDescription"];
            }
            Assert.Equal(validationResult, result);
        }
    }
}
