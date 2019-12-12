using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IAuthorizationService
    {
        Task<bool> CanEditNotification(ClaimsPrincipal user, Notification notification);
        Task<bool> CanEditBannerModel(ClaimsPrincipal user, NotificationBannerModel notificationBannerModel);
        Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications);
        Task<bool> IsUserAuthorizedToManageAlert(ClaimsPrincipal user, Alert alert);
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
            return notificationBanners.Select(async n =>
            {
                if(n.NotificationStatus != NotificationStatus.Legacy)
                {
                    var fullAccess = await CanEditBannerModel(user, n);
                    n.FullAccess = fullAccess;
                }
                return n;
            })
            .Select(n => n.Result);
        }

        public async Task<bool> CanEditBannerModel(ClaimsPrincipal user, NotificationBannerModel notificationBannerModel)
        {
            var tbServiceCode = notificationBannerModel.TbServiceCode;
            var tbServicePhecCode = notificationBannerModel.TbServicePHECCode;
            var locationPhecCode = notificationBannerModel.LocationPHECCode;
            return await GetUserPermissionFromLocationAndService(user, tbServiceCode, locationPhecCode, tbServicePhecCode);
        }

        public async Task<bool> CanEditNotification(ClaimsPrincipal user, Notification notification)
        {
            var tbServiceCode = notification.Episode.TBServiceCode;
            var tbServicePhecCode = notification.Episode.TBService?.PHECCode;
            var locationPhecCode = notification.PatientDetails.PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHECCode;
            return await GetUserPermissionFromLocationAndService(user, tbServiceCode, locationPhecCode, tbServicePhecCode);
        }

        private async Task<bool> GetUserPermissionFromLocationAndService(ClaimsPrincipal user, string tbServiceCode, string locationPhecCode, string tbServicePhecCode)
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

        private async Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            return await _userService.GetUserPermissionsFilterAsync(user);
        }
    }
}