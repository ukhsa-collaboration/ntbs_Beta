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
        Task<IEnumerable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IEnumerable<Notification> notifications);
    }

    public class AuthorizationService : IAuthorizationService
    {
        private readonly IUserService userService;
        private UserPermissionsFilter filter;

        public AuthorizationService(IUserService userService)
        {
            this.userService = userService;
        }

        public async Task<bool> CanEdit(ClaimsPrincipal user, Notification notification)
        {
            if (filter == null)
            {
                filter = await SetUserPermissionsFilterAsync(user);
            }

            if (filter.Type == UserType.NationalTeam)
            {
                return true;
            }

            if (filter.FilterByTBService)
            {
                return MatchesTBServiceCode(filter, notification);
            }
            else
            {
                return MatchesPHECCode(filter, notification);
            }
        }

        public async Task<IEnumerable<Notification>> FilterNotificationsByUserAsync(ClaimsPrincipal user, IEnumerable<Notification> notifications)
        {
            if (filter == null)
            {
                filter = await SetUserPermissionsFilterAsync(user);
            }
    
            if (filter.FilterByTBService)
            {
                notifications = notifications.Where(n => MatchesTBServiceCode(filter, n));
            }
            else if (filter.FilterByPHEC)
            {
                notifications = notifications.Where(n => MatchesPHECCode(filter, n));
            }
            return notifications;
        }

        private async Task<UserPermissionsFilter> SetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            return await userService.GetUserPermissionsFilterAsync(user);
        }

        private bool MatchesTBServiceCode(UserPermissionsFilter filter, Notification notification)
        {
            return filter.IncludedTBServiceCodes.Contains(notification.Episode.TBServiceCode);
        }

        private bool MatchesPHECCode(UserPermissionsFilter filter, Notification notification)
        {
            return filter.IncludedPHECCodes.Contains(notification.Episode.TBService?.PHECCode)
                || filter.IncludedPHECCodes.Contains(notification.PatientDetails.PostcodeLookup?.LocalAuthority.LocalAuthorityToPHEC.PHECCode);
        }
    }
}