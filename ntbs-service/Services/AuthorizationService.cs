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
        private UserPermissionsFilter _userPermissionsFilter;

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

        public async Task<PermissionLevel> GetPermissionLevelForNotificationAsync(ClaimsPrincipal user,
            Notification notification)
        {
            if (_userPermissionsFilter == null)
            {
                _userPermissionsFilter = await GetUserPermissionsFilterAsync(user);
            }
            
            if (UserHasDirectRelationToNotification(notification) || _userPermissionsFilter.Type == UserType.NationalTeam)
            {
                return PermissionLevel.Edit;
            }

            if (UserHasDirectRelationToLinkedNotification(notification?.Group?.Notifications))
            {
                return PermissionLevel.ReadOnly;
            }

            return PermissionLevel.None;
        }
        
        private async Task<bool> CanEditBannerModelAsync(ClaimsPrincipal user,
            NotificationBannerModel notificationBannerModel)
        {
            if (_userPermissionsFilter == null)
            {
                _userPermissionsFilter = await GetUserPermissionsFilterAsync(user);
            }
            if (_userPermissionsFilter.Type == UserType.NationalTeam)
            {
                return true;
            }
            if (_userPermissionsFilter.Type == UserType.PheUser)
            {
                var allowedCodes = _userPermissionsFilter.IncludedPHECCodes;
                return allowedCodes.Contains(notificationBannerModel.TbServicePHECCode) ||
                       allowedCodes.Contains(notificationBannerModel.LocationPHECCode);
            }
            if (_userPermissionsFilter.Type == UserType.NhsUser)
            {
                return _userPermissionsFilter.IncludedTBServiceCodes.Contains(notificationBannerModel.TbServiceCode);
            }
            return false;
            
        }
        
        private bool UserHasDirectRelationToLinkedNotification(IEnumerable<Notification> linkedNotifications)
        {
            return linkedNotifications != null && linkedNotifications.Select(UserHasDirectRelationToNotification).Any(x => x == true);
        }

        private bool UserHasDirectRelationToNotification(Notification notification)
        {
            return UserBelongsToTbServiceOfNotification(notification) || UserBelongsToTreatmentPhecOfNotification(notification) ||
                UserBelongsToResidencePhecOfNotification(notification);
        }

        private bool UserBelongsToTbServiceOfNotification(Notification notification)
        {
            return _userPermissionsFilter.Type == UserType.NhsUser && _userPermissionsFilter.IncludedTBServiceCodes.Contains(notification.Episode.TBServiceCode);
        }
        
        private bool UserBelongsToTreatmentPhecOfNotification(Notification notification)
        {
            return _userPermissionsFilter.Type == UserType.PheUser && _userPermissionsFilter.IncludedPHECCodes.Contains(notification.Episode.TBService?.PHECCode);
        }
        
        private bool UserBelongsToResidencePhecOfNotification(Notification notification)
        {
            return _userPermissionsFilter.Type == UserType.PheUser && _userPermissionsFilter.IncludedPHECCodes.Contains(notification.PatientDetails.PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHECCode);
        }

        public async Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications)
        {
            if (_userPermissionsFilter == null)
            {
                _userPermissionsFilter = await GetUserPermissionsFilterAsync(user);
            }

            if (_userPermissionsFilter.Type == UserType.NhsUser)
            {
                notifications = notifications.Where(n => _userPermissionsFilter.IncludedTBServiceCodes.Contains(n.Episode.TBServiceCode));
            }
            else if (_userPermissionsFilter.Type == UserType.PheUser)
            {
                // Having a method in LINQ clause breaks IQuaryable abstraction. We have to use inline expression over methods
                notifications = notifications.Where(n => 
                    (
                        n.Episode.TBService != null && 
                        _userPermissionsFilter.IncludedPHECCodes.Contains(n.Episode.TBService.PHECCode)
                    ) || (
                        n.PatientDetails.PostcodeLookup != null && 
                        n.PatientDetails.PostcodeLookup.LocalAuthority != null && 
                        n.PatientDetails.PostcodeLookup.LocalAuthority.LocalAuthorityToPHEC != null && 
                        _userPermissionsFilter.IncludedPHECCodes.Contains(n.PatientDetails.PostcodeLookup.LocalAuthority.LocalAuthorityToPHEC.PHECCode)
                    )
                );
            }
            return notifications;
        }

        public async Task<IList<Alert>> FilterAlertsForUserAsync(ClaimsPrincipal user, IList<Alert> alerts)
        {
            var userTbServiceCodes = (await _userService.GetTbServicesAsync(user)).Select(s => s.Code);
            var userType = _userService.GetUserType(user);
            for (int i = alerts.Count - 1; i >= 0; i--)
            {
                // For transfer alerts PHE users belonging to a PHEC cannot see and action transfer alerts as they are
                // aimed to be actioned on a TB service level
                if (userType == UserType.PheUser && (alerts[i].AlertType == AlertType.TransferRequest ||
                                                     alerts[i].AlertType == AlertType.TransferRejected))
                {
                    alerts.RemoveAt(i);
                }
                else if (alerts[i].TbServiceCode != null && !userTbServiceCodes.Contains(alerts[i].TbServiceCode))
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
