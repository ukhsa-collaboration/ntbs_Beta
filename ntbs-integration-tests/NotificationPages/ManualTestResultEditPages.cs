using System.Collections.Generic;
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

        public ManualTestResultEditPages(NtbsWebApplicationFactory<Startup> factory, ITestOutputHelper output) : base(factory)
        {
        }


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
                ["TestResultForEdit.ManualTestTypeId"] = ((int)ManualTestTypeId.Histology).ToString(),
                ["TestResultForEdit.SampleTypeId"] = ((int)SampleTypeId.Blood).ToString(),
                ["TestResultForEdit.Result"] = ((int)Result.Negative).ToString(),
            };
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditTestResults, NotificationId));
            var testsListPage = await Client.GetAsync(GetRedirectLocation(result)); // Follow the redirect to see results table
            var testsListDocument = await GetDocumentAsync(testsListPage);
            var manualResults = testsListDocument.GetElementById("manual-results");

            Assert.Contains("Histology", manualResults.TextContent);
            Assert.Contains("Blood", manualResults.TextContent);
            Assert.Contains("Negative", manualResults.TextContent);
        }
    }
}
