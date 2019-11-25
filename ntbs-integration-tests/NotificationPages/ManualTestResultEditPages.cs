using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Helpers;
using Xunit;
using System;
using ntbs_service.Models.Validations;

namespace ntbs_integration_tests.NotificationPages
{
    public class ManualTestResultEditPages : TestRunnerNotificationBase
    {
        const int TEST_ID = 10;
        protected override string NotificationSubPath => NotificationSubPaths.EditManualTestResult(null);

        public ManualTestResultEditPages(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>()
            {
                new Notification()
                {
                    NotificationId = Utilities.NOTIFICATION_WITH_MANUAL_TESTS,
                    NotificationStatus = NotificationStatus.Notified,
                    ManualTestResults = new List<ManualTestResult> () { new ManualTestResult
                    {
                        ManualTestResultId = TEST_ID,
                        TestDate = new DateTime(),
                        ManualTestTypeId = (int)ManualTestTypeId.Smear,
                        SampleTypeId = (int)SampleTypeId.LungBronchialTreeTissue,
                        Result = Result.Positive,
                    }}
                }
            };
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

        [Fact]
        public async Task PostEditOfManualTestResult_ReturnsSuccessAndAddsResultToTable()
        {
            // Arrange
            var NotificationId = Utilities.DRAFT_ID;
            var editUrl = GetCurrentPathForId(NotificationId) + TEST_ID;
            var editPage = await Client.GetAsync(editUrl);
            var editDocument = await GetDocumentAsync(editPage);

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
            var result = await SendPostFormWithData(editDocument, formData, editUrl);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditTestResults, NotificationId));
            var testsListPage = await Client.GetAsync(GetRedirectLocation(result)); // Follow the redirect to see results table
            var testsListDocument = await GetDocumentAsync(testsListPage);
            var manualResults = testsListDocument.GetElementById("manual-results");

            Assert.Contains("Histology", manualResults.TextContent);
            Assert.Contains("Blood", manualResults.TextContent);
            Assert.Contains("Negative", manualResults.TextContent);
            Assert.DoesNotContain("Smear", manualResults.TextContent);
        }

        [Fact]
        public async Task PostEditOfManualTestResult_ReturnsAllRequiredValidationErrors()
        {
            // Arrange
            var NotificationId = Utilities.DRAFT_ID;
            var editUrl = GetCurrentPathForId(NotificationId) + TEST_ID;
            var editPage = await Client.GetAsync(editUrl);
            var editDocument = await GetDocumentAsync(editPage);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedTestDate.Day"] = "",
                ["FormattedTestDate.Month"] = "",
                ["FormattedTestDate.Year"] = "",
                ["TestResultForEdit.ManualTestTypeId"] = "",
                ["TestResultForEdit.SampleTypeId"] = "",
                ["TestResultForEdit.Result"] = "",
            };
            var result = await SendPostFormWithData(editDocument, formData, editUrl);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            result.AssertValidationErrorResponse();

            resultDocument.AssertErrorMessage("test-date", string.Format(ValidationMessages.RequiredEnter, "Test date"));
            resultDocument.AssertErrorMessage("test-type", string.Format(ValidationMessages.RequiredSelect, "Test type"));
            resultDocument.AssertErrorMessage("sample-type", string.Format(ValidationMessages.RequiredSelect, "Sample type"));
            resultDocument.AssertErrorMessage("result", string.Format(ValidationMessages.RequiredSelect, "Result"));
        }
    }
}
