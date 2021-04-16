using System;
using System.Collections.Generic;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_ui_tests.Hooks;

namespace ntbs_ui_tests.Helpers
{
    public static class Utilities
    {
        public static Notification GetNotificationForUser(string notificationName, UserConfig userConfig)
        {
            if (!Notifications.TryGetValue(notificationName, out var baseNotification))
            {
                throw new ArgumentException($"Unexpected notification name {notificationName} given");
            }
            baseNotification.CreationDate = DateTime.Now;
            baseNotification.HospitalDetails = new HospitalDetails
            {
                TBServiceCode = userConfig.TbServiceCode,
                CaseManagerUsername = userConfig.Username
            };
            return baseNotification;
        }

        private static IDictionary<string, Notification> Notifications =>
            new Dictionary<string, Notification>
            {
                {
                    "TO_BE_DENOTIFIED",
                    new Notification
                    {
                        NotificationStatus = NotificationStatus.Notified,
                        NotificationSites = new List<NotificationSite>
                        {
                            new NotificationSite
                            {
                                SiteId = (int)SiteId.PULMONARY
                            }
                        }
                    }
                },
                {
                    "M_BOVIS",
                    new Notification
                    {
                        NotificationStatus = NotificationStatus.Notified,
                        NotificationSites = new List<NotificationSite>
                        {
                            new NotificationSite
                            {
                                SiteId = (int)SiteId.PULMONARY
                            }
                        },
                        DrugResistanceProfile = new DrugResistanceProfile {Species = "M. bovis"}
                    }
                }
            };
    }
}
