using System.Collections.Generic;
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
        public static void SeedDatabase(NtbsContext db)
        {
            db.Notification.AddRange(GetSeedingNotifications());
            db.SaveChanges();
        }

        public static List<Notification> GetSeedingNotifications()
        {
            return new List<Notification>()
            {
                new Notification(){ NotificationId = DRAFT_ID, NotificationStatus = NotificationStatus.Draft },
                new Notification(){ NotificationId = NOTIFIED_ID, NotificationStatus = NotificationStatus.Notified },
                new Notification(){ NotificationId = DENOTIFIED_ID, NotificationStatus = NotificationStatus.Denotified },
            };
        }
    }
}