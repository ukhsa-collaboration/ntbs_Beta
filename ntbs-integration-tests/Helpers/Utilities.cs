using System;
using System.Collections.Generic;
using System.Linq;
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
        public const int NOTIFIED_ID_WITH_NOTIFICATION_DATE = 4;
        public const int NEW_ID = 1000;

        public const int DENOTIFY_WITH_DESCRIPTION = 10;
        public const int DENOTIFY_NO_DESCRIPTION = 11;

        public const int DELETE_WITH_DESCRIPTION = 20;
        public const int DELETE_NO_DESCRIPTION = 21;

        public const int NOTIFIED_WITH_TBSERVICE = 30;
      
        // These IDs match actual reference data - see app db seeding
        public const string HOSPITAL_FLEETWOOD_HOSPITAL_ID = "1EE2B39A-428F-44C7-B4BB-000649636591";
        public const string HOSPITAL_FLEET_HOSPITAL_ID = "f9454382-9fbd-4524-8b65-04c1b449469c";
        public const string HOSPITAL_FAKE_ID = "f9454382-9fbd-4524-8b65-000000000000";
        public const string TBSERVICE_ROYAL_DERBY_HOSPITAL_ID = "TBS0181";
        public const string TBSERVICE_ROYAL_FREE_LONDON_TB_SERVICE_ID = "TBS0182";
        public const string PERMITTED_SERVICE_CODE = "TBS0008";
        public const string UNPERMITTED_SERVICE_CODE = "TBS0009";
        public const string PERMITTED_PHEC_CODE = "E45000019";
        public const string UNPERMITTED_PHEC_CODE = "E45000020";
        public const string PERMITTED_POSTCODE = "TW153AA";
        public const string UNPERMITTED_POSTCODE = "NW51TL";

        public static void SeedDatabase(NtbsContext context)
        {
            // General purpose entities extensively shared between tests
            context.Notification.AddRange(GetSeedingNotifications());
            context.PostcodeLookup.AddRange(GetTestPostcodeLookups());

            // Entities required for specific test suites
            context.Notification.AddRange(DenotifyPageTests.GetSeedingNotifications());
            context.Notification.AddRange(DeletePageTests.GetSeedingNotifications());
            context.Notification.AddRange(EpisodesPageTests.GetSeedingNotifications());

            context.SaveChanges();
        }

        // Unlike other data, these are not seeded via fluent migrator so we need to add some test poscodes manually
        private static IEnumerable<PostcodeLookup> GetTestPostcodeLookups()
        {
            return new List<PostcodeLookup>
            {
                // Matches permitted PHEC_CODE
                new PostcodeLookup { Postcode = PERMITTED_POSTCODE, LocalAuthorityCode = "E10000030", CountryCode = "E92000001" },
                // Matches unpermitted PHEC_CODE
                new PostcodeLookup { Postcode = UNPERMITTED_POSTCODE, LocalAuthorityCode = "E09000007", CountryCode = "E92000001" }
            };
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
                new Notification()
                {
                    NotificationId = DENOTIFIED_ID,
                    NotificationStatus = NotificationStatus.Denotified,
                    // Requires a notification site to pass full validation
                    NotificationSites = new List<NotificationSite>
                    {
                        new NotificationSite { NotificationId = DENOTIFIED_ID, SiteId = (int)SiteId.PULMONARY }
                    }
                }
            };
        }

        public static void SetServiceCodeForNotification(NtbsContext context, int notificationId, string code)
        {
            var notification = context.Notification.Find(notificationId);
            notification.Episode.TBServiceCode = code;
            context.SaveChanges();
        }

        public static void SetPostcodeForNotification(NtbsContext context, int notificationId, string code)
        {
            var notification = context.Notification.Find(notificationId);
            notification.PatientDetails.PostcodeToLookup = code;
            context.SaveChanges();
        }
    }
}
