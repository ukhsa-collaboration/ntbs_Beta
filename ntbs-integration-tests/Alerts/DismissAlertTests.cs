using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests.Alerts
{
    public class DismissAlertTests : TestRunnerBase
    {
        public DismissAlertTests(NtbsWebApplicationFactory<Startup> factory) : base(factory)
        {
        }

        public static string Dismiss(int alertId) => $"Alerts/{alertId}/Dismiss?page=Overview";
        public static string NotificationOverview(int notificationId) => $"Notifications/{notificationId}";

        [Fact]
        public async Task DismissAlert_ClosesAlertAndReturnsToNotificationOverview()
        {
            // Arrange
            var alertId = Utilities.ALERT_TO_DISMISS_ID;
            var notificationOverviewPath = NotificationOverview(Utilities.NOTIFIED_ID);
            var initialPage = await Client.GetAsync(notificationOverviewPath);
            var initialDocument = await GetDocumentAsync(initialPage);
            var alertElement = initialDocument.GetElementById($"alert-{alertId}");
            Assert.NotNull(alertElement);

            // Act
            var response = await Client.SendVerificationPostAsync(initialPage, initialDocument, Dismiss(alertId), null);

            // Assert
            response.AssertRedirectTo(notificationOverviewPath);
            var overviewPageAfterDismissal = await Client.GetAsync(notificationOverviewPath);
            var documentAfterDismissal = await GetDocumentAsync(overviewPageAfterDismissal);
            var alertElementAfterDismissal = documentAfterDismissal.GetElementById($"alert-{alertId}");
            Assert.Null(alertElementAfterDismissal);
        }

        [Fact]
        public async Task DismissClosedAlert_RedirectsToNotificationOverview()
        {
            // Arrange
            var alertId = Utilities.CLOSED_ALERT_ID;
            var notificationOverviewPath = NotificationOverview(Utilities.NOTIFIED_ID);
            var initialPage = await Client.GetAsync(notificationOverviewPath);
            var initialDocument = await GetDocumentAsync(initialPage);
            var alertElement = initialDocument.GetElementById($"alert-{alertId}");
            Assert.Null(alertElement);

            // Act
            var response = await Client.SendVerificationPostAsync(initialPage, initialDocument, Dismiss(alertId), null);

            // Assert
            response.AssertRedirectTo(notificationOverviewPath);
            var overviewPageAfterDismissal = await Client.GetAsync(notificationOverviewPath);
            var documentAfterDismissal = await GetDocumentAsync(overviewPageAfterDismissal);
            var alertElementAfterDismissal = documentAfterDismissal.GetElementById($"alert-{alertId}");
            Assert.Null(alertElementAfterDismissal);
        }

        [Fact]
        public async Task AttemptToDismissNonExistentAlert_RedirectsToIndexPage()
        {
            // Arrange
            var notAnAlertId = 999999;
            var notificationOverviewPath = NotificationOverview(Utilities.NOTIFIED_ID);
            var initialPage = await Client.GetAsync(notificationOverviewPath);
            var initialDocument = await GetDocumentAsync(initialPage);
            var alertElement = initialDocument.GetElementById($"alert-{notAnAlertId}");
            Assert.Null(alertElement);

            // Act
            var response = await Client.SendVerificationPostAsync(initialPage, initialDocument, Dismiss(notAnAlertId), null);

            // Assert
            response.AssertRedirectTo("/");
        }
    }
}
