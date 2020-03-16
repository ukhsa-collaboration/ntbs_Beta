using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests.AdminPages
{
    public class CloneNotificationTests : TestRunnerBase
    {
        public CloneNotificationTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        private const string PageRoute = "/Admin/CloneNotification";

        [Fact]
        public async Task DismissAlert_CorrectlyDismissesAlertAndReturnsHomePage()
        {
            // Arrange
            var pageContent = await GetDocumentForUrlAsync(PageRoute);

            // Act
            Dictionary<string, string> form = new Dictionary<string, string>
            {
                ["NotificationId"] = Utilities.NOTIFIED_ID.ToString(),
            };
            var result = await Client.SendPostFormWithData(pageContent, form, PageRoute);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            result.AssertRedirectTo("Notification");
        }
    }
}
