using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class SocialContextAddressesEditPageTests : TestRunnerNotificationBase
    {
        private const int ADDRESS_ID_WITH_CURLY_BRACKETS = 13;
        protected override string NotificationSubPath => NotificationSubPaths.EditSocialContextAddresses;
        
        public SocialContextAddressesEditPageTests(NtbsWebApplicationFactory<EntryPoint> factory) : base(factory)
        {
          
        }
        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_CURLY_BRACKETS_IN_SOCIALCONTEXTADDRESSES,
                    NotificationStatus = NotificationStatus.Notified,
                    SocialContextAddresses = new List<SocialContextAddress>
                    {
                        new SocialContextAddress
                        {
                            SocialContextAddressId = ADDRESS_ID_WITH_CURLY_BRACKETS,
                            Details = "{{abc}}"
                        }
                    }
                }
            };
        }
        
        [Fact]
        public async Task GetEditOfAddresses_RemovesCurlyBrackets()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_ID_WITH_CURLY_BRACKETS_IN_SOCIALCONTEXTADDRESSES;
            var url = GetCurrentPathForId(notificationId);
            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var detailsContainer = document.GetElementById("social-context-addresses-list").TextContent;

            Assert.DoesNotContain("{", detailsContainer);
            Assert.DoesNotContain("}", detailsContainer);
            Assert.Contains("abc", detailsContainer);
        }

    }
}