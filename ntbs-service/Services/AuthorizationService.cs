using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IAuthorizationService
    {
        Task<(PermissionLevel permissionLevel, string reason)> GetPermissionLevelAsync(
            ClaimsPrincipal user,
            Notification notification);
        Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications);
        Task<bool> IsUserAuthorizedToManageAlert(ClaimsPrincipal user, Alert alert);
        Task<IList<Alert>> FilterAlertsForUserAsync(ClaimsPrincipal user, IList<Alert> alerts);
        Task SetFullAccessOnNotificationBannersAsync(
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

        public async Task SetFullAccessOnNotificationBannersAsync(
            IEnumerable<NotificationBannerModel> notificationBanners,
            ClaimsPrincipal user)
        {
            async Task SetPadlockForBannerAsync(ClaimsPrincipal u, NotificationBannerModel bannerModel)
            {
                bannerModel.ShowPadlock = !(await CanEditBannerModelAsync(u, bannerModel));
            }

            await Task.WhenAll(
                notificationBanners
                    .Select(n => SetPadlockForBannerAsync(user, n)));
        }

        public async Task<(PermissionLevel permissionLevel, string reason)> GetPermissionLevelAsync(
            ClaimsPrincipal user,
            Notification notification)
        {
            if (_userPermissionsFilter == null)
            {
                _userPermissionsFilter = await GetUserPermissionsFilterAsync(user);
            }

            if (_userPermissionsFilter.Type == UserType.NationalTeam)
            {
                return (PermissionLevel.Edit, null);
            }

            if (UserHasDirectRelationToNotification(notification)) 
            {
                return notification.NotificationStatus != NotificationStatus.Closed
                    ? (PermissionLevel.Edit, null)
                    : (PermissionLevel.ReadOnly, "You cannot edit this notification because it is closed");
            }

            if (UserBelongsToResidencePhecOfNotification(notification.PatientDetails.PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHECCode) 
                || UserHasDirectRelationToLinkedNotification(notification.Group?.Notifications)
                || UserPreviouslyHadDirectionRelationToNotification(notification))
            {
                return (PermissionLevel.ReadOnly, "You do not have permission to edit this notification");
            }

            return (PermissionLevel.None, null);
        }
        
        private async Task<bool> CanEditBannerModelAsync(
            ClaimsPrincipal user,
            NotificationBannerModel notificationBannerModel)
        {
            if (_userPermissionsFilter == null)
            {
                _userPermissionsFilter = await GetUserPermissionsFilterAsync(user);
            }
            
            switch (_userPermissionsFilter.Type)
            {
                case UserType.NationalTeam:
                    return true;
                case UserType.PheUser:
                {
                    var allowedCodes = _userPermissionsFilter.IncludedPHECCodes;
                    return allowedCodes.Contains(notificationBannerModel.TbServicePHECCode) ||
                           allowedCodes.Contains(notificationBannerModel.LocationPHECCode);
                }
                case UserType.NhsUser:
                    return _userPermissionsFilter.IncludedTBServiceCodes.Contains(notificationBannerModel.TbServiceCode);
                default:
                    return false;
            }
        }
        
        private bool UserHasDirectRelationToLinkedNotification(IEnumerable<Notification> linkedNotifications)
        {
            return linkedNotifications != null && linkedNotifications.Select(UserHasDirectRelationToNotification).Any(x => x);
        }

        private bool UserPreviouslyHadDirectionRelationToNotification(Notification notification)
        {
            foreach (var previousTbService in notification.PreviousTbServices)
            {
                if (UserBelongsToTbServiceOfNotification(previousTbService.TbServiceCode) || UserBelongsToTreatmentPhecOfNotification(previousTbService.PhecCode))
                {
                    return true;
                }
            }
            return false;
        }

        private bool UserHasDirectRelationToNotification(Notification notification)
        {
            return UserBelongsToTbServiceOfNotification(notification.HospitalDetails.TBServiceCode) || UserBelongsToTreatmentPhecOfNotification(notification.HospitalDetails.TBService?.PHECCode);
        }

        private bool UserBelongsToTbServiceOfNotification(string tbServiceCode)
        {
            return _userPermissionsFilter.Type == UserType.NhsUser && _userPermissionsFilter.IncludedTBServiceCodes.Contains(tbServiceCode);
        }
        
        private bool UserBelongsToTreatmentPhecOfNotification(string treatmentPhecCode)
        {
            return _userPermissionsFilter.Type == UserType.PheUser && _userPermissionsFilter.IncludedPHECCodes.Contains(treatmentPhecCode);
        }
        
        private bool UserBelongsToResidencePhecOfNotification(string residencePhecCode)
        {
            return _userPermissionsFilter.Type == UserType.PheUser && _userPermissionsFilter.IncludedPHECCodes.Contains(residencePhecCode);
        }

        public async Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications)
        {
            if (_userPermissionsFilter == null)
            {
                _userPermissionsFilter = await GetUserPermissionsFilterAsync(user);
            }

            if (_userPermissionsFilter.Type == UserType.NhsUser)
            {
                notifications = notifications.Where(n => _userPermissionsFilter.IncludedTBServiceCodes.Contains(n.HospitalDetails.TBServiceCode));
            }
            else if (_userPermissionsFilter.Type == UserType.PheUser)
            {
                // Having a method in LINQ clause breaks IQueryable abstraction. We have to use inline expression over methods
                notifications = notifications.Where(n => 
                    (
                        n.HospitalDetails.TBService != null && 
                        _userPermissionsFilter.IncludedPHECCodes.Contains(n.HospitalDetails.TBService.PHECCode)
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
