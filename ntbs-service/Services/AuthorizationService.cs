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
        Task<bool> CanEditNotificationAsync(ClaimsPrincipal user, Notification notification);
        Task<bool> CanEditBannerModelAsync(ClaimsPrincipal user, NotificationBannerModel notificationBannerModel);
        Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications);
        Task<bool> IsUserAuthorizedToManageAlert(ClaimsPrincipal user, Alert alert);
        Task<IList<Alert>> FilterAlertsForUserAsync(ClaimsPrincipal user, IList<Alert> alerts);
        IEnumerable<NotificationBannerModel> SetFullAccessOnNotificationBanners(
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

        public IEnumerable<NotificationBannerModel> SetFullAccessOnNotificationBanners(
            IEnumerable<NotificationBannerModel> notificationBanners,
            ClaimsPrincipal user)
        {
            notificationBanners.ToList().ForEach(async n =>
            {
                if(n.NotificationStatus != NotificationStatus.Legacy)
                {
                    n.FullAccess = await CanEditBannerModelAsync(user, n);
                }
            });
            return notificationBanners;
        }

        public async Task<bool> CanEditBannerModelAsync(ClaimsPrincipal user, NotificationBannerModel notificationBannerModel)
        {
            var tbServiceCode = notificationBannerModel.TbServiceCode;
            var tbServicePhecCode = notificationBannerModel.TbServicePHECCode;
            var locationPhecCode = notificationBannerModel.LocationPHECCode;
            return await AuthorizeUserAccess(user, tbServiceCode, locationPhecCode, tbServicePhecCode);
        }

        public async Task<bool> CanEditNotificationAsync(ClaimsPrincipal user, Notification notification)
        {
            var tbServiceCode = notification.Episode.TBServiceCode;
            var tbServicePhecCode = notification.Episode.TBService?.PHECCode;
            var locationPhecCode = notification.PatientDetails.PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHECCode;
            return await AuthorizeUserAccess(user, tbServiceCode, locationPhecCode, tbServicePhecCode);
        }

        private async Task<bool> AuthorizeUserAccess(ClaimsPrincipal user, string tbServiceCode, string locationPhecCode, string tbServicePhecCode)
        {
            if (_filter == null)
            {
                _filter = await GetUserPermissionsFilterAsync(user);
            }

            if (_filter.FilterByTBService)
            {
                return _filter.IncludedTBServiceCodes.Contains(tbServiceCode);
            }

            else if (_filter.FilterByPHEC)
            {
                var allowedCodes = _filter.IncludedPHECCodes;
                return allowedCodes.Contains(tbServicePhecCode) || allowedCodes.Contains(locationPhecCode);
            }

            return true;
        }

        public async Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications)
        {
            if (_filter == null)
            {
                _filter = await GetUserPermissionsFilterAsync(user);
            }
    
            if (_filter.FilterByTBService)
            {
                notifications = notifications.Where(n => _filter.IncludedTBServiceCodes.Contains(n.Episode.TBServiceCode));
            }
            else if (_filter.FilterByPHEC)
            {
                // Having a method in LINQ clause breaks IQuaryable abstraction. We have to use inline expression over methods
                notifications = notifications.Where(n => 
                    (
                        n.Episode.TBService != null && 
                        _filter.IncludedPHECCodes.Contains(n.Episode.TBService.PHECCode)
                    ) || (
                        n.PatientDetails.PostcodeLookup != null && 
                        n.PatientDetails.PostcodeLookup.LocalAuthority != null && 
                        n.PatientDetails.PostcodeLookup.LocalAuthority.LocalAuthorityToPHEC != null && 
                        _filter.IncludedPHECCodes.Contains(n.PatientDetails.PostcodeLookup.LocalAuthority.LocalAuthorityToPHEC.PHECCode)
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
