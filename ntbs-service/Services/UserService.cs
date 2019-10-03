using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;

namespace ntbs_service.Services
{
  public interface IUserService
  {
    Task<List<TBService>> TbServices(ClaimsPrincipal user);
    UserType GetUserType(ClaimsPrincipal user);
  }

  public class UserService : IUserService
  {
    // This allows us to use group names agnostic of any naming convention prefixes that exist in the setup
    // TODO NTBS-61 put into config
    private readonly string prefix = "pheNtbs - ";
    private readonly NtbsContext context;
    
    public UserService(NtbsContext context) {
      this.context = context;
    }
    async public Task<List<TBService>> TbServices(ClaimsPrincipal user)
    {
      var services = await context.TBService.ToListAsync();
      // National Team users have accss to all services' records
      if (GetUserType(user) == UserType.NationalTeam)
      {
        return services;
      }

      var roles = GetRoles(user);
      return services.Where(service => roles.Contains(service.ServiceAdGroup))
        .Union(services.Where(service => roles.Contains(service.PHECAdGroup)))
        .ToList();
    }

    private IEnumerable<string> GetRoles(ClaimsPrincipal user)
    {
      return user.FindAll(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType)
              .Select(claim => claim.Value)
              .Select(role => role.StartsWith(prefix) ? role.Substring(prefix.Length) : role);
    }

    public UserType GetUserType(ClaimsPrincipal user)
    {
      if (user.IsInRole(prefix + "Global.NIS.NTBS.NTA")) 
      { 
        return UserType.NationalTeam;
      }
      // TB service groups are of the format Global.NIS.NTBS.Service_<service-specifc-postfix>
      if (GetRoles(user).Where(role => role.Contains("Global.NIS.NTBS.Service")).Any())
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