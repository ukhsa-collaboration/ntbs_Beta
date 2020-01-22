using System.Collections.Generic;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_ui_tests.Helpers
{
    public static class Utilities
    {
        // We're using high IDs here, as the in-memory db doesn't correctly handle id growth state
        // (see https://github.com/dotnet/efcore/issues/6872 - potentially fixed in .NET Core 3 ?)
        // This makes the id unlikely to clash with the ids created in tests
        public const int TO_BE_DENOTIFIED_ID = 10001;
        
        public const string TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID = "TBS0001";
        
        public const string PHEC_CONTAINING_ABINGDON_CODE = "E45000019";
        
        public static void SeedDatabase(NtbsContext context)
        {
            // General purpose entities shared between tests
            context.Notification.AddRange(GetSeedingNotifications());

            context.SaveChanges();
        }


        private static List<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = TO_BE_DENOTIFIED_ID,
                    NotificationStatus = NotificationStatus.Notified,
                    NotificationSites = new List<NotificationSite>
                    {
                        new NotificationSite { NotificationId = TO_BE_DENOTIFIED_ID, SiteId = (int)SiteId.PULMONARY }
                    }
                },
            };
        }
    }
}
