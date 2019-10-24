using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using ntbs_service.Models;

namespace ntbs_service.Services
{
    public interface IUserService
    {
        Task<IEnumerable<TBService>> GetTbServicesAsync(ClaimsPrincipal user);
        UserType GetUserType(ClaimsPrincipal user);
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
        public async Task<IEnumerable<TBService>> GetTbServicesAsync(ClaimsPrincipal user)
        {
            var tbServices = await context.GetAllTbServicesAsync();
            // National Team users have access to all services' records
            if (GetUserType(user) == UserType.NationalTeam)
            {
                return tbServices;
            }

            var roles = GetRoles(user);
            return tbServices
                .Where(service =>
                    roles.Contains(service.ServiceAdGroup)
                    || roles.Contains(service.PHECAdGroup))
                .ToList();
        }

        private IEnumerable<string> GetRoles(ClaimsPrincipal user)
        {
            return user.FindAll(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType)
                    .Select(claim => claim.Value)
                    .Select(role => role.StartsWith(config.AdGroupsPrefix) ? role.Substring(config.AdGroupsPrefix.Length) : role);
        }

        public UserType GetUserType(ClaimsPrincipal user)
        {
            if (user.IsInRole(config.AdGroupsPrefix + config.NationalTeamAdGroup))
            {
                return UserType.NationalTeam;
            }
            if (GetRoles(user).Any(role => role.Contains(config.ServiceGroupAdPrefix)))
            {
                return UserType.NhsUser;
            }
            return UserType.PheUser;
        }
    }

    public enum UserType
    {
        /** Members of the national team with access to all regions */
        NationalTeam,
        /** Members of PHECs, regions associated with multiple TB services */
        PheUser,
        /** Clinical staff, members of particular TB services */
        NhsUser
    }
}
