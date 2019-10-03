using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;

namespace ntbs_service.Services
{
  public interface ITbServiceManager
  {
    Task<List<TBService>> ForUser(ClaimsPrincipal user);
  }

  public class TbServiceManager : ITbServiceManager
  {
    private readonly NtbsContext context;
    
    public TbServiceManager(NtbsContext context) {
      this.context = context;
    }
    async public Task<List<TBService>> ForUser(ClaimsPrincipal user)
    {
      // This allows us to use group names agnostic of any naming convention prefixes that exist in the setup
      // TODO NTBS-61 put into config
      var prefix = "pheNtbs - ";

      var services = await context.TBService.ToListAsync();
      // National team users
      // TODO NTBS-61 put into config
      if (user.IsInRole(prefix + "Global.NIS.NTBS.NTA")) {
        return services;
      }

      var roles = user.FindAll(claim => claim.Type == ClaimsIdentity.DefaultRoleClaimType)
        .Select(claim => claim.Value)
        .Select(role => role.StartsWith(prefix) ? role.Substring(prefix.Length) : role);

      // NHS Users
      return services.Where(service => roles.Contains(service.ServiceAdGroup))
      // PHE Users
        .Union(services.Where(service => roles.Contains(service.PHECAdGroup)))
        .ToList();
    }
  }
}