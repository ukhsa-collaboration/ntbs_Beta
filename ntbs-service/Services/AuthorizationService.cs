using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IAuthorizationService
    {
        Task<PermissionLevel> GetPermissionLevelForNotificationAsync(ClaimsPrincipal user, Notification notification);
        Task<bool> CanEditBannerModelAsync(ClaimsPrincipal user, NotificationBannerModel notificationBannerModel);
        Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications);
        Task<bool> IsUserAuthorizedToManageAlert(ClaimsPrincipal user, Alert alert);
        Task<IList<Alert>> FilterAlertsForUserAsync(ClaimsPrincipal user, IList<Alert> alerts);
        void SetFullAccessOnNotificationBanners(
            IEnumerable<NotificationBannerModel> notificationBanners,
            ClaimsPrincipal user);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserService _userService;
        private UserPermissionsFilter _filter;

        public AuthorizationService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<bool> IsUserAuthorizedToManageAlert(ClaimsPrincipal user, Alert alert)
        {
            var userTbServiceCodes = (await _userService.GetTbServicesAsync(user)).Select(s => s.Code);
            return userTbServiceCodes.Contains(alert.TbServiceCode);
        }

        public void SetFullAccessOnNotificationBanners(
            IEnumerable<NotificationBannerModel> notificationBanners,
            ClaimsPrincipal user)
        {
            notificationBanners.ToList().ForEach(async n =>
            {
                if(n.NotificationStatus != NotificationStatus.Legacy)
                {
                    n.ShowPadlock = await CanEditBannerModelAsync(user, n);
                }
            });
        }

        public async Task<bool> CanEditBannerModelAsync(ClaimsPrincipal user,
            NotificationBannerModel notificationBannerModel)
        {
            if (_filter == null)
            {
                _filter = await GetUserPermissionsFilterAsync(user);
            }

            if (_filter.FilterByTBService)
            {
                return _filter.IncludedTBServiceCodes.Contains(notificationBannerModel.TbServiceCode);
            }

            if (_filter.FilterByPHEC)
            {
                var allowedCodes = _filter.IncludedPHECCodes;
                return allowedCodes.Contains(notificationBannerModel.TbServicePHECCode) ||
                       allowedCodes.Contains(notificationBannerModel.LocationPHECCode);
            }
            return true;
            
        }

        public async Task<PermissionLevel> GetPermissionLevelForNotificationAsync(ClaimsPrincipal user,
            Notification notification)
        {
            if (_filter == null)
            {
                _filter = await GetUserPermissionsFilterAsync(user);
            }
            
            if (UserHasDirectRelationToNotification(notification))
            {
                return PermissionLevel.Edit;
            }

            if (UserHasDirectRelationToLinkedNotification(notification?.Group?.Notifications))
            {
                return PermissionLevel.ReadOnly;
            }

            return PermissionLevel.None;
        }
        
        private bool UserHasDirectRelationToLinkedNotification(IEnumerable<Notification> linkedNotifications)
        {
            return linkedNotifications.Select(UserHasDirectRelationToNotification).Any(x => x == true);
        }

        private bool UserHasDirectRelationToNotification(Notification notification)
        {
            return UserBelongsToTbServiceOfNotification(notification) || UserBelongsToTreatmentPhecOfNotification(notification) ||
                UserBelongsToResidencePhecOfNotification(notification);
        }

        private bool UserBelongsToTbServiceOfNotification(Notification notification)
        {
            if (_filter.FilterByTBService)
            {
                return _filter.IncludedTBServiceCodes.Contains(notification.Episode.TBServiceCode);
            }
            return false;
        }
        
        private bool UserBelongsToTreatmentPhecOfNotification(Notification notification)
        {
            if (_filter.FilterByPHEC)
            {
                return _filter.IncludedPHECCodes.Contains(notification.Episode.TBService?.PHECCode);
            }
            return false;
        }
        
        private bool UserBelongsToResidencePhecOfNotification(Notification notification)
        {
            if (_filter.FilterByPHEC)
            {
                return _filter.IncludedPHECCodes.Contains(notification.PatientDetails.PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHECCode);
            }
            return false;
        }

        public async Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications)
        {
            if (_filter == null)
            {
                _filter = await GetUserPermissionsFilterAsync(user);
            }

            return notifications.Where(n => UserHasDirectRelationToNotification(n));
        }

        public async Task<IList<Alert>> FilterAlertsForUserAsync(ClaimsPrincipal user, IList<Alert> alerts)
        {
            var userTbServiceCodes = (await _userService.GetTbServicesAsync(user)).Select(s => s.Code);
            for (int i = alerts.Count - 1; i >= 0; i--)
            {
                if (alerts[i].TbServiceCode != null && !userTbServiceCodes.Contains(alerts[i].TbServiceCode))
                    alerts.RemoveAt(i);
            }
            return alerts;
        }

        private async Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            return await _userService.GetUserPermissionsFilterAsync(user);
        }
    }
}
