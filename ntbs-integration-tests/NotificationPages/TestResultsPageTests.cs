using System.Net;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class TestResultsPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.ViewTestResults;

        public TestResultsPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        [Fact]
        public async void TestResultsPageDisplaysSpecimenData()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFIED_ID;

            // Act
            var document = await GetDocumentForUrlAsync(GetCurrentPathForId(notificationId));

            // Assert
            Assert.Contains("20 Dec 2010", document.GetElementById("1253490-date").TextContent);
            Assert.Contains("Lymph node", document.GetElementById("1253490-type").TextContent);
            Assert.Contains("M. tuberculosis complex", document.GetElementById("1253490-species").TextContent);
            Assert.Contains("Northern Ireland", document.GetElementById("1253490-labname").TextContent);
            Assert.Contains("Sensitive", document.GetElementById("1253490-inh").TextContent);
            Assert.Contains("Sensitive", document.GetElementById("1253490-rif").TextContent);
            Assert.Contains("No result", document.GetElementById("1253490-pza").TextContent);
            Assert.Contains("No result", document.GetElementById("1253490-emb").TextContent);
            Assert.Contains("Sensitive", document.GetElementById("1253490-amino").TextContent);
            Assert.Contains("Sensitive", document.GetElementById("1253490-quin").TextContent);
            Assert.Contains("No", document.GetElementById("1253490-mdr").TextContent);
            Assert.Contains("No", document.GetElementById("1253490-xdr").TextContent);
            Assert.Contains("Manual", document.GetElementById("1253490-match").TextContent);
        }
    }
}
