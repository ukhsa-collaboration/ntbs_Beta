using ntbs_service.Models;

namespace ntbs_service.Pages.Notifications
{
    public interface IWithNotificationBanner
    {
        NotificationBannerModel NotificationBannerModel { get; set; }
    }
}
