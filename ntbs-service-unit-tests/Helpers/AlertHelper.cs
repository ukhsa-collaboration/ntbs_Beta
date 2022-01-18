using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;

namespace ntbs_service_unit_tests.Helpers
{
    internal static class AlertHelper
    {
        public static DataQualityPotentialDuplicateAlert 
            CreateOpenDuplicateAlert(int notificationId, int duplicateId, int alertId)
        {
            return new DataQualityPotentialDuplicateAlert
            {
                NotificationId = notificationId, 
                DuplicateId = duplicateId, 
                AlertId = alertId, 
                AlertStatus = AlertStatus.Open
            };
        }
    }
}
