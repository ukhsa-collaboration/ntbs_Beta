using ntbs_service.Models.Enums;

namespace ntbs_service.Helpers
{
    public static class CssClassHelpers
    {
        public static string GetClassSuffixForNotificationStatus(NotificationStatus notificationStatus)
        {
            switch (notificationStatus)
            {
                case NotificationStatus.Draft:
                    return "--draft";
                case NotificationStatus.Notified:
                    return "--notified";
                case NotificationStatus.Denotified:
                    return "--denotified";
                case NotificationStatus.Legacy:
                    return "--legacy";
            }

            return "";
        }
    }
}
