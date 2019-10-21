using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Newtonsoft.Json;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class ImmunosuppressionPageTests : TestRunnerBase
    {
        protected override string PageRoute => Routes.Immunosuppression;

        public ImmunosuppressionPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) {}

        [Fact]
        public async Task Post_ReturnsTypeRequiredError_IfYesSelected()
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["ImmunosuppressionDetails.Status"] = "Yes",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(FullErrorMessage(ValidationMessages.ImmunosuppressionTypeRequired), DocumentExtensions.GetError(resultDocument, "status"));
        }

        [Fact]
        public async Task Post_ReturnsDetailsRequiredError_IfOtherCheckedSelected()
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var initialDocument = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["ImmunosuppressionDetails.Status"] = "Yes",
                ["ImmunosuppressionDetails.HasOther"] = "True",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.EnsureSuccessStatusCode();

            Assert.Equal(FullErrorMessage(ValidationMessages.ImmunosuppressionDetailRequired), DocumentExtensions.GetError(resultDocument, "description"));
        }

        [Fact]
        public async Task Post_RedirectsToNextPageAndSavesContent()
        {
            // Arrange
            var initialPage = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var initialDocument = await GetDocumentAsync(initialPage);

            const string Description = "Other Therapy";
            var formData = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.DRAFT_ID.ToString(),
                ["ImmunosuppressionDetails.Status"] = "Yes",
                ["ImmunosuppressionDetails.HasOther"] = "True",
                ["ImmunosuppressionDetails.OtherDescription"] = Description,
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Equal(BuildEditRoute(Routes.PreviousHistory, Utilities.DRAFT_ID), GetRedirectLocation(result));

            var reloadedPage = await client.GetAsync(GetPageRouteForId(Utilities.DRAFT_ID));
            var reloadedDocument = await GetDocumentAsync(reloadedPage);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("immunosuppression-yes")).IsChecked);
            Assert.True(((IHtmlInputElement)reloadedDocument.GetElementById("ImmunosuppressionDetails_HasOther")).IsChecked);
            Assert.Equal(Description, ((IHtmlInputElement)reloadedDocument.GetElementById("ImmunosuppressionDetails_OtherDescription")).Value);
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
            var response = await client.GetAsync(BuildValidationPath(formData, "Validate"));

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            if (!string.IsNullOrEmpty(result))
            {
                var mappedResult = JsonConvert.DeserializeObject<Dictionary<string, string>>(result);
                if (result.Contains("Status"))
                {
                    result = mappedResult["ImmunosuppressionDetails.Status"];
                }
                else
                {
                    result = mappedResult["ImmunosuppressionDetails.OtherDescription"];
                }
            }
            Assert.Equal(validationResult, result);
        }
    }
}
