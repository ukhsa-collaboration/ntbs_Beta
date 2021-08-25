using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
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
                    var (permissionLevel, _) = await authorizationService.GetPermissionLevelAsync(user, n);
                    var fullAccess = permissionLevel == PermissionLevel.Edit;
                    return new NotificationBannerModel(
                        n,
                        showPadlock: permissionLevel == PermissionLevel.None,
                        showLink: fullAccess || n.NotificationStatus != NotificationStatus.Draft);
                })
                .Select(n => n.Result);
        }
    }
}
