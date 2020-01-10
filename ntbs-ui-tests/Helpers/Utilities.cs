using System.Collections.Generic;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_ui_tests.Helpers
{
    // Currently not actually used by anything, need to fix bug in NTBS-725
    public static class Utilities
    {
        public const int DRAFT_ID = 1;
        public const int NOTIFIED_ID = 2;
        
        public const string TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID = "TBS0001";

        public static void SeedDatabase(NtbsContext context)
        {
            // General purpose entities shared between tests
            context.Notification.AddRange(GetSeedingNotifications());

            context.SaveChanges();
        }


        public static List<Notification> GetSeedingNotifications()
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
            };
        }
    }
}
