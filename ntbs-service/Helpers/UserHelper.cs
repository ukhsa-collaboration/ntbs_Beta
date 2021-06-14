using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;
using ntbs_service.Properties;

namespace ntbs_service.Helpers
{
    public interface IUserHelper
    {
        bool CurrentUserMatchesUsernameOrIsAdmin(HttpContext context, string username);
        bool UserIsReadOnly(ClaimsPrincipal user);
    }
    
    public class UserHelper : IUserHelper
    {
        private readonly AdOptions _adOptions;
        
        public UserHelper(IOptionsMonitor<AdOptions> adOptions)
        {
            _adOptions = adOptions.CurrentValue;
        }
        
        public static string GetUsername(HttpContext context)
        {
            // Identity name is a fallback for if user doesn't have an email associated with them - as is the case with our test users
            return string.IsNullOrEmpty(context.User.Username()) ? context.User.Identity.Name : context.User.Username();
        }

        public bool CurrentUserMatchesUsernameOrIsAdmin(HttpContext context, string username)
        {
            return context.User.Username() == username || context.User.IsInRole(_adOptions.AdminUserGroup);
        }

        public bool UserIsReadOnly(ClaimsPrincipal user)
        {
            return user.IsInRole(_adOptions.ReadOnlyUserGroup);
        }
    }
}
