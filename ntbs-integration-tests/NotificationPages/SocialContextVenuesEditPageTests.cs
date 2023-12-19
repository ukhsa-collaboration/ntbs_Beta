
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
    public class SocialContextVenuesEditPageTests : TestRunnerNotificationBase
    {
        private const int VENUE_ID_WITH_CURLY_BRACKETS = 14;
        protected override string NotificationSubPath => NotificationSubPaths.EditSocialContextVenues;
        
        public SocialContextVenuesEditPageTests(NtbsWebApplicationFactory<EntryPoint> factory) : base(factory)
        {
          
        }
        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_CURLY_BRACKETS_IN_SOCIALCONTEXTVENUES,
                    NotificationStatus = NotificationStatus.Notified,
                    SocialContextVenues = new List<SocialContextVenue>
                    {
                        new SocialContextVenue()
                        {
                            SocialContextVenueId = VENUE_ID_WITH_CURLY_BRACKETS,
                            Details = "{{abc}}",
                            Name = "{{def}}"
                        }
                    }
                }
            };
        }
        
        [Fact]
        public async Task GetEditOfVenues_RemovesCurlyBrackets()
        {
            // Arrange
            const int notificationId = Utilities.NOTIFICATION_ID_WITH_CURLY_BRACKETS_IN_SOCIALCONTEXTVENUES;
            var url = GetCurrentPathForId(notificationId);
            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var detailsContainer = document.GetElementById("social-context-venues-list").TextContent;

            Assert.DoesNotContain("{", detailsContainer);
            Assert.DoesNotContain("}", detailsContainer);
            Assert.Contains("abc", detailsContainer);
            Assert.Contains("def", detailsContainer);
        }

    }
}