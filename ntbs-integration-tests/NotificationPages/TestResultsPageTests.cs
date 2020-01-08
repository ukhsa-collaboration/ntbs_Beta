using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class TestResultsPageTest : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.EditTestResults;

        public TestResultsPageTest(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async Task IfMatchingLabResultsExist_DisplaysLabResults()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFIED_ID;

            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(notificationId));

            // Assert
            var document = await GetDocumentAsync(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(document.QuerySelector("div[id='lab-results-summary']"));
            Assert.NotNull(document.QuerySelector("div[id='specimens-details']"));
        }

        [Fact]
        public async Task IfMatchingSpecimenExist_DisplaysSpecimenDetails()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFIED_ID;

            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(notificationId));

            // Assert
            var document = await GetDocumentAsync(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.NotNull(document.QuerySelector("div[id='lab-results-summary']"));
            Assert.NotNull(document.QuerySelector("div[id='specimens-details']"));
        }
    }
}