using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface IUserService
    {
        Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user);
        Task<TBService> GetDefaultTBService(ClaimsPrincipal user);
        Task<IEnumerable<TBService>> GetTbServicesAsync(ClaimsPrincipal user);
    }

    public class UserService : IUserService
    {
        private readonly AdfsOptions config;
        private readonly NtbsContext context;

        public UserService(NtbsContext context, IOptionsMonitor<AdfsOptions> options)
        {
            this.context = context;
            config = options.CurrentValue;
        }
        public async Task<UserPermissionsFilter> GetUserPermissionsFilterAsync(ClaimsPrincipal user)
        {
            var userFilter = new UserPermissionsFilter() { Type = GetUserType(user) };

            if (userFilter.Type == UserType.NationalTeam)
            {
                // National Team users have access to all services' records
                return userFilter;
            }

            var roles = GetRoles(user);
            if (userFilter.Type == UserType.NhsUser)
            {
                userFilter.IncludedTBServiceCodes = await context.TbService.Where(tb => roles.Contains(tb.ServiceAdGroup)).Select(tb => tb.Code).ToListAsync();
            }
            else
            {
                userFilter.IncludedPHECCodes = await context.PHEC.Where(ph => roles.Contains(ph.AdGroup)).Select(ph => ph.Code).ToListAsync();
            }

            return userFilter;
        }

        public async Task<TBService> GetDefaultTBService(ClaimsPrincipal user)
        {
            var type = GetUserType(user);
            var roles = GetRoles(user);

            return await UserServicesQuery(type, roles).FirstAsync();
        }

        public async Task<IEnumerable<TBService>> GetTbServicesAsync(ClaimsPrincipal user)
        {
            var type = GetUserType(user);
            var roles = GetRoles(user);

            return await UserServicesQuery(type, roles).ToListAsync();
        }

        private IQueryable<TBService> UserServicesQuery(UserType type, IEnumerable<string> roles)
        {
            switch (type)
            {
                case UserType.NationalTeam:
                    return context.TbService;
                case UserType.NhsUser:
                    return context.TbService.Where(tb => roles.Contains(tb.ServiceAdGroup));
                default:
                    return context.TbService.Include(tb => tb.PHEC).Where(tb => roles.Contains(tb.PHEC.AdGroup));
            }
        }

        private IEnumerable<string> GetRoles(ClaimsPrincipal user)
        {
            return user.FindAll(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType)
                    .Select(claim => claim.Value)
                    .Select(role => role.StartsWith(config.AdGroupsPrefix) ? role.Substring(config.AdGroupsPrefix.Length) : role);
        }

        private UserType GetUserType(ClaimsPrincipal user)
        {
            if (user.IsInRole(config.AdGroupsPrefix + config.NationalTeamAdGroup))
            {
                return UserType.NationalTeam;
            }
            if (GetRoles(user).Where(role => role.Contains(config.ServiceGroupAdPrefix)).Any())
            {
                return UserType.NhsUser;
            }
            return UserType.PheUser;
        }
    }
}