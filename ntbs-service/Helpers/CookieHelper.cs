using Microsoft.AspNetCore.Http;
using Serilog;

namespace ntbs_service.Helpers
{
    public static class CookieHelper
    {
        public static string GetUserCookie(HttpRequest request)
        {
            var cookie = request.Cookies[".AspNetCore.Cookies"];
            return cookie ?? "defaultCookie"; 
        }
    }
}
