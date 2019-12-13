using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Helpers
{
    public static class NotificationExtensions
    {
        public static IEnumerable<NotificationBannerModel> CreateNotificationBanners(
            this IEnumerable<Notification> notifications,
            ClaimsPrincipal user,
            IAuthorizationService authorizationService)
        {
            return notifications.Select(async n =>
                {
                    var fullAccess = await authorizationService.CanEditNotificationAsync(user, n);
                    return new NotificationBannerModel(n, fullAccess: fullAccess, showLink: true);
                })
                .Select(n => n.Result);
        }
    }
}
