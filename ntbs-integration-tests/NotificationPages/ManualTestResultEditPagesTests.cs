using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Newtonsoft.Json;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Models.FilteredSelectLists;
using ntbs_service.Models.Validations;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class ManualTestResultEditPagesTests : TestRunnerNotificationBase
    {
        const int TEST_ID = 10;
        const int TEST_TO_DELETE_ID = 11;
        protected override string NotificationSubPath => NotificationSubPaths.EditManualTestResult(0);

        public ManualTestResultEditPagesTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_WITH_MANUAL_TESTS,
                    NotificationStatus = NotificationStatus.Notified,
                    TestData = { ManualTestResults = new List<ManualTestResult> () {
                        new ManualTestResult {
                            ManualTestResultId = TEST_ID,
                            TestDate = new DateTime(2012, 1, 1),
                            ManualTestTypeId = (int)ManualTestTypeId.Smear,
                            SampleTypeId = (int)SampleTypeId.LungBronchialTreeTissue,
                            Result = Result.Positive,
                        },
                        new ManualTestResult {
                            ManualTestResultId = TEST_TO_DELETE_ID,
                            TestDate = new DateTime(2013, 1, 1),
                            ManualTestTypeId = (int)ManualTestTypeId.LineProbeAssay,
                            SampleTypeId = (int)SampleTypeId.GastricWashings,
                            Result = Result.Awaiting,
                        }
                    }}
                }
            };
        }

        [Fact]
        public async Task PostNewManualTestResult_ReturnsSuccessAndAddsResultToTable()
        {
            // Arrange
            const int notificationId = Utilities.DRAFT_ID;
            var url = GetPathForId(NotificationSubPaths.AddManualTestResult, notificationId);
            var initialDocument = await GetDocumentForUrl(url);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedTestDate.Day"] = "2",
                ["FormattedTestDate.Month"] = "2",
                ["FormattedTestDate.Year"] = "2012",
                ["TestResultForEdit.ManualTestTypeId"] = ((int)ManualTestTypeId.Smear).ToString(),
                ["TestResultForEdit.SampleTypeId"] = ((int)SampleTypeId.Blood).ToString(),
                ["TestResultForEdit.Result"] = ((int)Result.Negative).ToString(),
            };
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditTestResults, notificationId));
            var testsListPage = await Client.GetAsync(GetRedirectLocation(result)); // Follow the redirect to see results table
            var testsListDocument = await GetDocumentAsync(testsListPage);
            // We can't pick based on id, as we don't know the id created
            var manualResultText = testsListDocument.GetElementById("manual-results")
                .GetElementsByTagName("tbody")[0]
                .GetElementsByTagName("tr")[0]
                .TextContent;

            Assert.Contains("Smear", manualResultText);
            Assert.Contains("Blood", manualResultText);
            Assert.Contains("Negative", manualResultText);
        }

        [Fact]
        public async Task PostEditOfManualTestResult_ReturnsSuccessAndAmendsResultInTable()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_MANUAL_TESTS;
            var editUrl = GetCurrentPathForId(notificationId) + TEST_ID;

            var editPage = await Client.GetAsync(editUrl);
            var editDocument = await GetDocumentAsync(editPage);
            var manualResultTextBeforeChanges = editDocument.GetElementById($"manual-test-result-{TEST_ID}").TextContent;
            Assert.Contains("Smear", manualResultTextBeforeChanges);
            Assert.Contains("Lung bronchial tree tissue", manualResultTextBeforeChanges);
            Assert.Contains("Positive", manualResultTextBeforeChanges);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedTestDate.Day"] = "3",
                ["FormattedTestDate.Month"] = "2",
                ["FormattedTestDate.Year"] = "2012",
                ["TestResultForEdit.ManualTestTypeId"] = ((int)ManualTestTypeId.Smear).ToString(),
                ["TestResultForEdit.SampleTypeId"] = ((int)SampleTypeId.BronchialWashings).ToString(),
                ["TestResultForEdit.Result"] = ((int)Result.Negative).ToString(),
            };
            var result = await SendPostFormWithData(editDocument, formData, editUrl);

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditTestResults, notificationId));
            var testsListPage = await Client.GetAsync(GetRedirectLocation(result)); // Follow the redirect to see results table
            var testsListDocument = await GetDocumentAsync(testsListPage);
            var manualResultText = testsListDocument.GetElementById($"manual-test-result-{TEST_ID}").TextContent;

            Assert.Contains("Smear", manualResultText);
            Assert.Contains("Bronchial washings", manualResultText);
            Assert.Contains("Negative", manualResultText);
        }

        [Fact]
        public async Task PostEditOfManualTestResult_ReturnsAllRequiredValidationErrors()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_MANUAL_TESTS;
            var editUrl = GetCurrentPathForId(notificationId) + TEST_ID;
            var editDocument = await GetDocumentForUrl(editUrl);

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

        [Fact]
        public async Task PostEditWithInvalidTestAndSampleTypeCombination_ReturnsErrors()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_MANUAL_TESTS;
            var editUrl = GetCurrentPathForId(notificationId) + TEST_ID;
            var editDocument = await GetDocumentForUrl(editUrl);

            // Act
            var formData = new Dictionary<string, string>
            {
                ["FormattedTestDate.Day"] = "",
                ["FormattedTestDate.Month"] = "",
                ["FormattedTestDate.Year"] = "",
                ["TestResultForEdit.ManualTestTypeId"] = ((int)ManualTestTypeId.Histology).ToString(),
                ["TestResultForEdit.SampleTypeId"] = ((int)SampleTypeId.BronchialWashings).ToString(),
                ["TestResultForEdit.Result"] = "",
            };
            var result = await SendPostFormWithData(editDocument, formData, editUrl);
            var resultDocument = await GetDocumentAsync(result);

            // Assert
            result.AssertValidationErrorResponse();
            resultDocument.AssertErrorMessage("sample-type", string.Format(ValidationMessages.InvalidTestAndSampleTypeCombination, "Sample type"));
        }

        [Fact]
        public async Task GetEditWithInvalidTest_ReturnsNotFound()
        {
            // Arrange
            const int notificationId = Utilities.DRAFT_ID;
            var editUrl = GetCurrentPathForId(notificationId) + TEST_ID;

            // Act
            var editPage = await Client.GetAsync(editUrl);

            // Assert
            Assert.Equal(HttpStatusCode.NotFound, editPage.StatusCode);
        }

        [Fact]
        public async Task PostDelete_ReturnsSuccessAndRemovesResult()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_WITH_MANUAL_TESTS;
            var editUrl = GetCurrentPathForId(notificationId) + TEST_TO_DELETE_ID;
            var editDocument = await GetDocumentForUrl(editUrl);

            // Act
            var formData = new Dictionary<string, string> { };
            var result = await SendPostFormWithData(editDocument, formData, editUrl, "Delete");

            // Assert
            result.AssertRedirectTo(GetPathForId(NotificationSubPaths.EditTestResults, notificationId));
            var testsListPage = await Client.GetAsync(GetRedirectLocation(result)); // Follow the redirect to see results table
            var testsListDocument = await GetDocumentAsync(testsListPage);
            Assert.Null(testsListDocument.GetElementById($"manual-test-result-{TEST_TO_DELETE_ID}"));
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task GetFilteredSampleTypesForManualTestType_ReturnsExpectedValues(bool isEdit)
        {
            // Arrange
            const int manualTestTypeId = (int)ManualTestTypeId.Histology;
            const string handlerPath = "FilteredSampleTypesForManualTestType";
            var notificationSubPath = isEdit ? NotificationSubPath : NotificationSubPaths.AddManualTestResult;
            var endpointPath = $"{notificationSubPath}/{handlerPath}";
            var endpointUrl = GetPathForId($"{endpointPath}?value={manualTestTypeId.ToString()}", 0);

            // Act
            var response = await Client.GetAsync(endpointUrl);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            var filteredLists = JsonConvert.DeserializeObject<FilteredManualTestPageSelectLists>(result);
            Assert.Equal(((int)SampleTypeId.BronchoscopySample).ToString(), filteredLists.SampleTypes.First().Value);
            Assert.Equal("BRONCHOSCOPY SAMPLE", filteredLists.SampleTypes.First().Text.ToUpperInvariant());
            Assert.Equal("RESPIRATORY", filteredLists.SampleTypes.First().Group.ToUpperInvariant());
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task IfDateTooEarly_ValidateTestResultForEditDate_ReturnsEarliestDateErrorMessage(bool isEdit)
        {
            // Arrange
            var formData = new Dictionary<string, string>
            {
                ["key"] = "TestDate",
                ["day"] = "1",
                ["month"] = "1",
                ["year"] = "1899"
            };
            const string handlerPath = "ValidateTestResultForEditDate";
            var notificationSubPath = isEdit ? NotificationSubPath : NotificationSubPaths.AddManualTestResult;
            var endpointPath = $"{notificationSubPath}/{handlerPath}";
            var endpointUrl = GetPathForId($"{endpointPath}", 0, formData);

            // Act
            var response = await Client.GetAsync(endpointUrl);

            // Assert
            var result = await response.Content.ReadAsStringAsync();
            Assert.Equal(ValidationMessages.DateValidityRangeStart("Test date", "01/01/2010"), result);
        }
    }
}
