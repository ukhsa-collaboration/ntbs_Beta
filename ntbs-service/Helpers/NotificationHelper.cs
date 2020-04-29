using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Helpers
{
    public static class NotificationHelper
    {
        public static void SetShouldValidateFull(Notification notification)
        {
            notification.ShouldValidateFull = true;
            foreach (var property in notification.GetType().GetProperties())
            {
                if (property.PropertyType.IsSubclassOf(typeof(ModelBase)))
                {
                    var ownedModel = property.GetValue(notification);
                    ownedModel.GetType().GetProperty("ShouldValidateFull").SetValue(ownedModel, true);
                }
            }
            notification.NotificationSites?.ForEach(site => site.ShouldValidateFull = notification.ShouldValidateFull);
        }

        public static TreatmentEvent CreateTreatmentStartEvent(Notification notification)
        {
            return new TreatmentEvent
            {
                NotificationId = notification.NotificationId,
                TreatmentEventType = TreatmentEventType.TreatmentStart,
                EventDate = notification.ClinicalDetails.TreatmentStartDate ?? notification.NotificationDate,
                CaseManager = notification.HospitalDetails.CaseManager,
                TbService = notification.HospitalDetails.TBService
            };
        }
    }
}