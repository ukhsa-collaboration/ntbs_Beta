using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Projections;
using ntbs_service.Pages;

namespace ntbs_service.Services
{
    public interface IAuthorizationService
    {
        Task<(PermissionLevel permissionLevel, string reason)> GetPermissionLevelAsync(
            ClaimsPrincipal contextUser,
            Notification notification);
        Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications);
        Task<bool> IsUserAuthorizedToManageAlert(ClaimsPrincipal user, Alert alert);
        Task<IList<AlertWithTbServiceForDisplay>> FilterAlertsForUserAsync(ClaimsPrincipal user, IList<AlertWithTbServiceForDisplay> alerts);
        Task SetFullAccessOnNotificationBannersAsync(
            IEnumerable<NotificationBannerModel> notificationBanners,
            ClaimsPrincipal user);
        Task<bool> CanViewBannerModelAsync(
            ClaimsPrincipal user,
            NotificationBannerModel notificationBannerModel);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserService _userService;
        private UserPermissionsFilter _userPermissionsFilter;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserHelper _userHelper;

        public AuthorizationService(
            IUserService userService,
            INotificationRepository notificationRepository,
            IUserHelper userHelper)
        {
            _userService = userService;
            _notificationRepository = notificationRepository;
            _userHelper = userHelper;
        }

        public async Task<bool> IsUserAuthorizedToManageAlert(ClaimsPrincipal user, Alert alert)
        {
            if (_userHelper.UserIsReadOnly(user))
            {
                return false;
            }

            var userTbServiceCodes = (await _userService.GetTbServicesAsync(user)).Select(s => s.Code).ToList();
            if (alert is TransferAlert transferAlert)
            {
                return userTbServiceCodes.Contains(transferAlert.TbServiceCode);
            }

            if (alert.NotificationId != null)
            {
                return userTbServiceCodes.Contains(
                    (await _notificationRepository.GetNotificationAsync((int)alert.NotificationId)).HospitalDetails.TBServiceCode);
            }

            return false;
        }

        public async Task SetFullAccessOnNotificationBannersAsync(
            IEnumerable<NotificationBannerModel> notificationBanners,
            ClaimsPrincipal user)
        {
            async Task SetPadlockAndLinkForBannerAsync(ClaimsPrincipal u, NotificationBannerModel bannerModel)
            {
                bannerModel.ShowPadlock = bannerModel.Source == NotificationBannerModel.NTBS_SOURCE && !(await CanViewBannerModelAsync(u, bannerModel));
                bannerModel.ShowLink = !UserIsReadOnlyAndNotificationIsDraftOrLegacy(u, bannerModel);
            }

            foreach (var n in notificationBanners)
            {
                await SetPadlockAndLinkForBannerAsync(user, n);
            }
        }

        public async Task<(PermissionLevel permissionLevel, string reason)> GetPermissionLevelAsync(
            ClaimsPrincipal contextUser,
            Notification notification)
        {
            if (_userPermissionsFilter == null)
            {
                _userPermissionsFilter = await GetUserPermissionsFilterAsync(contextUser);
            }

            var userIsReadOnly = _userHelper.UserIsReadOnly(contextUser);

            if (_userPermissionsFilter.Type == UserType.NationalTeam)
            {
                return userIsReadOnly
                    ? (PermissionLevel.ReadOnly, Messages.NoEditPermission)
                    : (PermissionLevel.Edit,
                        // National team members are allowed to modify even closed notifications, but it is useful
                        // for them to be able to tell when they are closed.
                        notification.NotificationStatus == NotificationStatus.Closed ? Messages.Closed : null);
            }

            if (UserHasDirectRelationToNotification(notification))
            {
                return userIsReadOnly
                    ? (PermissionLevel.ReadOnly, Messages.NoEditPermission)
                    : notification.NotificationStatus == NotificationStatus.Closed
                        ? (PermissionLevel.ReadOnly, Messages.ClosedNoEdit)
                        : (PermissionLevel.Edit, null);
            }

            if (UserBelongsToResidencePhecOfNotification(notification)
                || UserHasDirectRelationToLinkedNotification(notification)
                || UserPreviouslyHadDirectionRelationToNotification(notification))
            {
                return (PermissionLevel.ReadOnly, Messages.NoEditPermission);
            }

            return (PermissionLevel.None, Messages.UnauthorizedWarning);
        }

        public async Task<bool> CanViewBannerModelAsync(
            ClaimsPrincipal user,
            NotificationBannerModel notificationBannerModel)
        {
            if (_userPermissionsFilter == null)
            {
                _userPermissionsFilter = await GetUserPermissionsFilterAsync(user);
            }

            return _userPermissionsFilter.Type == UserType.NationalTeam
                   || UserBelongsToTbServiceOfNotification(notificationBannerModel.TbServiceCode)
                   || UserBelongsToPhecFromNotification(notificationBannerModel.TbServicePHECCode)
                   || UserBelongsToPhecFromNotification(notificationBannerModel.LocationPHECCode)
                   || notificationBannerModel.LinkedNotificationPhecCodes.Any(UserBelongsToPhecFromNotification)
                   || notificationBannerModel.LinkedNotificationTbServiceCodes.Any(UserBelongsToTbServiceOfNotification)
                   || notificationBannerModel.PreviousTbServiceCodes.Any(UserBelongsToTbServiceOfNotification)
                   || notificationBannerModel.PreviousPhecCodes.Any(UserBelongsToPhecFromNotification);
        }

        private bool UserHasDirectRelationToLinkedNotification(Notification notification)
        {
            var linkedNotifications = notification.Group?.Notifications;
            return linkedNotifications != null && linkedNotifications.Select(UserHasDirectRelationToNotification).Any(x => x);
        }

        private bool UserPreviouslyHadDirectionRelationToNotification(Notification notification)
        {
            foreach (var previousTbService in notification.PreviousTbServices)
            {
                if (UserBelongsToTbServiceOfNotification(previousTbService.TbServiceCode) || UserBelongsToPhecFromNotification(previousTbService.PhecCode))
                {
                    return true;
                }
            }
            return false;
        }

        private bool UserHasDirectRelationToNotification(Notification notification)
        {
            return UserBelongsToTbServiceOfNotification(notification.HospitalDetails.TBServiceCode) || UserBelongsToPhecFromNotification(notification.HospitalDetails.TBService?.PHECCode);
        }

        private bool UserBelongsToTbServiceOfNotification(string tbServiceCode)
        {
            return _userPermissionsFilter.Type == UserType.NhsUser && _userPermissionsFilter.IncludedTBServiceCodes.Contains(tbServiceCode);
        }

        private bool UserBelongsToPhecFromNotification(string phecCode)
        {
            return _userPermissionsFilter.Type == UserType.PheUser && _userPermissionsFilter.IncludedPHECCodes.Contains(phecCode);
        }

        private bool UserBelongsToResidencePhecOfNotification(Notification notification)
        {
            var phecCode = notification.PatientDetails.PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHECCode;
            return _userPermissionsFilter.Type == UserType.PheUser
                   && _userPermissionsFilter.IncludedPHECCodes.Contains(phecCode);
        }

        public async Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user,
            IQueryable<Notification> notifications)
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

        public async Task<IList<AlertWithTbServiceForDisplay>> FilterAlertsForUserAsync(ClaimsPrincipal user, IList<AlertWithTbServiceForDisplay> alerts)
        {
            var userTbServiceCodes = (await _userService.GetTbServicesAsync(user)).Select(s => s.Code).ToList();
            var userType = _userService.GetUserType(user);
            alerts = alerts.Where(a => userTbServiceCodes.Contains(a.TbServiceCode)).ToList();
            if (userType == UserType.PheUser)
            {
                alerts = alerts.Where(a => a.AlertType != AlertType.TransferRequest &&
                                           a.AlertType != AlertType.TransferRejected).ToList();
            }
            return alerts.ToList();
        }

        private async Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            return await _userService.GetUserPermissionsFilterAsync(user);
        }

        private bool UserIsReadOnlyAndNotificationIsDraftOrLegacy(ClaimsPrincipal user, NotificationBannerModel bannerModel)
        {
            return _userHelper.UserIsReadOnly(user) &&
                   (bannerModel.NotificationStatus == NotificationStatus.Draft || bannerModel.Source != NotificationBannerModel.NTBS_SOURCE);
        }
    }
}
