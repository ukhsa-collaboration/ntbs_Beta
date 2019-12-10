using ntbs_service.Models;

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
            notification.NotificationSites?.ForEach(x => x.ShouldValidateFull = notification.ShouldValidateFull);
        }
    }
}