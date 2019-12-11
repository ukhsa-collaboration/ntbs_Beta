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
        Task<bool> CanEdit(ClaimsPrincipal user, Notification notification);
        Task<bool> CanEditBannerModel(ClaimsPrincipal user, NotificationBannerModel notificationBannerModel);
        Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications);
        Task<bool> IsUserAuthorizedToManageAlert(ClaimsPrincipal user, Alert alert);
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

        public async Task<bool> CanEditBannerModel(ClaimsPrincipal user, NotificationBannerModel notificationBannerModel)
        {
            var userTbServiceCodes = (await _userService.GetTbServicesAsync(user)).Select(s => s.Code);
            return  notificationBannerModel.TbServiceCode == null || userTbServiceCodes.Contains(notificationBannerModel.TbServiceCode);
        }

        public async Task<bool> CanEdit(ClaimsPrincipal user, Notification notification)
        {
            if (_filter == null)
            {
                _filter = await GetUserPermissionsFilterAsync(user);
            }

            if (_filter.FilterByTBService)
            {
                return _filter.IncludedTBServiceCodes.Contains(notification.Episode.TBServiceCode);
            }
            else if (_filter.FilterByPHEC)
            {
                var locationPhecCode = notification.PatientDetails.PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHECCode;
                var servicePhecCode = notification.Episode.TBService?.PHECCode;

                var allowedCodes = _filter.IncludedPHECCodes;
                return allowedCodes.Contains(servicePhecCode) || allowedCodes.Contains(locationPhecCode);
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