using ntbs_service.Models;

namespace ntbs_service.Pages.Notifications
{
    public interface INotificationLayoutWithBanner
    {
        NotificationBannerModel NotificationBannerModel { get; set; }
    }
}
