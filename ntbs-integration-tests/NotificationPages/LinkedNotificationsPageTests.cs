using System.Linq;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class LinkedNotificationsPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.LinkedNotifications;

        public LinkedNotificationsPageTests(NtbsWebApplicationFactory<EntryPoint> factory) : base(factory)
        {
        }

        [Fact]
        public async Task Get_RendersCorrectNavigationLinks()
        {
            // Arrange
            using (var client = Factory
                .WithUserAuth(TestUser.ServiceUserForAbingdonAndPermitted)
                .WithNotificationAndTbServiceConnected(Utilities.LINKED_NOTIFICATION_ABINGDON_TB_SERVICE, Utilities.PERMITTED_SERVICE_CODE)
                .CreateClientWithoutRedirects())
            {
                // Act
                var changesPath = GetCurrentPathForId(Utilities.LINKED_NOTIFICATION_ABINGDON_TB_SERVICE);
                var response = await client.GetAsync(changesPath);
                var document = await GetDocumentAsync(response);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var navLinks = document.GetElementsByClassName("app-subnav__link");
                Assert.NotNull(navLinks.SingleOrDefault(elem => elem.TextContent.Contains("Notification details")));
                Assert.NotNull(navLinks.SingleOrDefault(elem => elem.TextContent.Contains("Linked notifications (1)")));
                Assert.NotNull(navLinks.SingleOrDefault(elem => elem.TextContent.Contains("Notification changes")));
                Assert.NotNull(navLinks.SingleOrDefault(elem => elem.TextContent.Contains("Case manager details")));
            }
        }

        [Fact]
        public async Task Get_RendersBannerForLinkedNotification()
        {
            // Arrange
            using (var client = Factory
                .WithUserAuth(TestUser.ServiceUserForAbingdonAndPermitted)
                .WithNotificationAndTbServiceConnected(Utilities.LINKED_NOTIFICATION_ABINGDON_TB_SERVICE, Utilities.PERMITTED_SERVICE_CODE)
                .CreateClientWithoutRedirects())
            {
                // Act
                var changesPath = GetCurrentPathForId(Utilities.LINKED_NOTIFICATION_ABINGDON_TB_SERVICE);
                var response = await client.GetAsync(changesPath);
                var document = await GetDocumentAsync(response);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var linkedNotificationBanner = document.GetElementsByClassName("nhsuk-grid-column-full")
                    .Where(e => e.TextContent.Contains("Linked Notifications"))
                    .First()
                    .GetElementsByClassName("notification-banner")
                    .First();
                Assert.Matches("Name\\s*DELVEY, Anna", linkedNotificationBanner.TextContent);
                Assert.Matches("Country of birth\\s*Unknown", linkedNotificationBanner.TextContent);
            }
        }
    }
}
