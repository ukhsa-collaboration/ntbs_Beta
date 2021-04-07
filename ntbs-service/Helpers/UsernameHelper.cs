using Microsoft.AspNetCore.Http;

namespace ntbs_service.Helpers
{
    public class UsernameHelper
    {
        public static string GetUsername(HttpContext context)
        {
            // Identity name is a fallback for if user doesn't have an email associated with them - as is the case with our test users
            return context.User.Username() ?? context.User.Identity.Name;
        }
    }
}
