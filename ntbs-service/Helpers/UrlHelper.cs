using System;
using Castle.Core.Internal;

namespace ntbs_service.Helpers
{
    public static class UrlHelper
    {
        public static string SanitiseHttpsUrl(string url)
        {
            bool isHttps = Uri.TryCreate(url, UriKind.Absolute, out var uriResult)
                           && uriResult.Scheme == Uri.UriSchemeHttps;

            return isHttps ? uriResult.AbsoluteUri : "/";
        }
    }
}
