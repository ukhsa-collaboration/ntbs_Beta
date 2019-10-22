using System.Collections.Generic;
using ntbs_integration_tests.NotificationPages;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_integration_tests.Helpers
{
    public static class Utilities
    {
        public const int DRAFT_ID = 1;
        public const int NOTIFIED_ID = 2;
        public const int DENOTIFIED_ID = 3;
        public const int NEW_ID = 1000;

        public const int DENOTIFY_WITH_DESCRIPTION = 10;
        public const int DENOTIFY_NO_DESCRIPTION = 11;

        public static void SeedDatabase(NtbsContext context)
        {
            // General purpose entities extensively shared between tests
            context.Notification.AddRange(GetSeedingNotifications(context));

            // Entities required for specific test suites
            context.Notification.AddRange(DenotifyPageTests.GetSeedingNotifications());

            context.SaveChanges();
        }

        public static List<Notification> GetSeedingNotifications(NtbsContext context)
        {
            return new List<Notification>
            {
                new Notification{ NotificationId = DRAFT_ID, NotificationStatus = NotificationStatus.Draft },
                new Notification
                {
                    NotificationId = NOTIFIED_ID,
                    NotificationStatus = NotificationStatus.Notified,
                    // Requires a notification site to pass full validation
                    NotificationSites = new List<NotificationSite>
                    {
                        new NotificationSite { NotificationId = NOTIFIED_ID, SiteId = (int)SiteId.PULMONARY }
                    }
                },
                new Notification()
                {
                    NotificationId = DENOTIFIED_ID,
                    NotificationStatus = NotificationStatus.Denotified,
                    // Requires a notification site to pass full validation
                    NotificationSites = new List<NotificationSite>
                    {
                        new NotificationSite { NotificationId = DENOTIFIED_ID, SiteId = (int)SiteId.PULMONARY }
                    }
                },
            };
        }
    }
}
