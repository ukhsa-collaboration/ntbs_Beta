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
        Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications);
        Task<bool> IsUserAuthorizedToManageAlert(ClaimsPrincipal user, Alert alert);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserService userService;
        private UserPermissionsFilter filter;

        public AuthorizationService(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<bool> IsUserAuthorizedToManageAlert(ClaimsPrincipal user, Alert alert)
        {
            var userTbServiceCodes = (await userService.GetTbServicesAsync(user)).Select(s => s.Code);
            return userTbServiceCodes.Contains(alert.TbServiceCode);
        }

        public async Task<bool> CanEdit(ClaimsPrincipal user, Notification notification)
        {
            if (filter == null)
            {
                filter = await GetUserPermissionsFilterAsync(user);
            }

            if (filter.FilterByTBService)
            {
                return filter.IncludedTBServiceCodes.Contains(notification.Episode.TBServiceCode);
            }
            else if (filter.FilterByPHEC)
            {
                return filter.IncludedPHECCodes.Contains(notification.Episode.TBService?.PHECCode)
                    || filter.IncludedPHECCodes.Contains(notification.PatientDetails.PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHECCode);
            }

            return true;
        }

        public async Task<IQueryable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IQueryable<Notification> notifications)
        {
            if (filter == null)
            {
                filter = await GetUserPermissionsFilterAsync(user);
            }
    
            if (filter.FilterByTBService)
            {
                notifications = notifications.Where(n => filter.IncludedTBServiceCodes.Contains(n.Episode.TBServiceCode));
            }
            else if (filter.FilterByPHEC)
            {
                notifications = notifications.Where(n => (n.Episode.TBService != null && filter.IncludedPHECCodes.Contains(n.Episode.TBService.PHECCode))
                                                        || (n.PatientDetails.PostcodeLookup != null && 
                                                            n.PatientDetails.PostcodeLookup.LocalAuthority != null && 
                                                            n.PatientDetails.PostcodeLookup.LocalAuthority.LocalAuthorityToPHEC != null && 
                                                            filter.IncludedPHECCodes.Contains(n.PatientDetails.PostcodeLookup.LocalAuthority.LocalAuthorityToPHEC.PHECCode)));
            }
            return notifications;
        }

        private async Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            return await userService.GetUserPermissionsFilterAsync(user);
        }
    }
}