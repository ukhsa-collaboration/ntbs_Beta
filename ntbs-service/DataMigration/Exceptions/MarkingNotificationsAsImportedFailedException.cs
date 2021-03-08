using System;
using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models.Entities;

namespace ntbs_service.DataMigration.Exceptions
{
    public class MarkingNotificationsAsImportedFailedException : Exception
    {
        public MarkingNotificationsAsImportedFailedException(ICollection<Notification> notifications, Exception cause)
            : base(ErrorMessage(notifications), cause)
        {
        }

        private static string ErrorMessage(ICollection<Notification> notifications)
        {
            var ids = string.Join(", ", notifications.Select(n => n.NotificationId));
            return $"Failed to mark notifications {ids} as imported in the migration db. " +
                   "This might cause the notifications to keep showing up as legacy in search results until corrected";
        }
    }
}
