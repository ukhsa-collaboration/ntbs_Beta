using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class ManualTestResultEditPages : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditManualTestResult("");

        public ManualTestResultEditPages(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }


        [Fact]
        public async Task PostNewManualTestResult_ReturnsSuccessAndAddsResultToTable()
        {
            // Arrange
            var NotificationId = Utilities.DRAFT_ID;
            var url = GetCurrentPathForId(NotificationId);
            var initialPage = await Client.GetAsync(url);
            var initialDocument = await GetDocumentAsync(initialPage);


            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedTestDate.Day"] = "2",
                ["FormattedTestDate.Month"] = "2",
                ["FormattedTestDate.Year"] = "1992",
                ["TestResultForEdit.ManualTestTypeId"] = ManualTestTypeId.Histology.ToString(),
                ["TestResultForEdit.SampleTypeId"] = SampleTypeId.Blood.ToString(),
                ["TestResultForEdit.Result"] = Result.Negative.ToString(),
            };
            var result = await SendPostFormWithData(initialDocument, formData, url);
            // var document = await GetDocumentAsync(result);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditTestResults, NotificationId));
            // var manualResults = document.GetElementById("manual-results");
            // Assert.Contains("Histology", manualResults.TextContent);
            // Assert.Contains("Blood", manualResults.TextContent);
            // Assert.Contains("Negative", manualResults.TextContent);
        }
    }
}
