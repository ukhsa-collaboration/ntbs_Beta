using System.Security.Claims;

namespace ntbs_service.Helpers
{
    internal static class ClaimsPrincipalExtensionMethods
    {
        public static string Username(this ClaimsPrincipal user)
        {
            return user.FindFirstValue(ClaimTypes.Upn);
        }
    }
}
