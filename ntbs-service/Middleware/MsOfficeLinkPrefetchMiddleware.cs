using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;

namespace ntbs_service.Middleware
{
    // This middleware is to deal with an issue following hyperlinks from Microsoft Office products, which send an
    // initial request to website from within the program to check the site, before sending a link to the browser to
    // open. This initial request in MS Office doesn't have access to the authentication cookies in browser, so the
    // request is redirected to the Azure AD login page, and this redirect is opened in a browser. The
    // anti-CSRF correlation cookie set before the redirect is lost, and when the user is redirected back from Azure AD
    // this cookie is missing, causing an app exception and a 500 response.

    // Because the initial redirected request gets a successful response (302), MS Office then chooses to open the site
    // properly in the browser. This leads to two tabs being opened, and one of which with the app working normally, and
    // the other is an error 500 page.

    // This issue has been present for over a decade. Discussion of this with regards to OIDC in ASP.NET is here:
    // https://github.com/aspnet/Security/issues/1252
    // And Microsoft's explanation for the behaviour is here:
    // https://docs.microsoft.com/en-US/office/troubleshoot/office-suite-issues/click-hyperlink-to-sso-website
    // Both parties suggest that other should fix the problem, so it seems unlikely that it will be fixed soon.
    // This behaviour in MS Office can be turned off by editing a registry value, but we can't ask all our users to do
    // this.

    // To work around this, this middleware intercepts requests that come from MS Office, and sends back an empty
    // 200 responses. When a user follows a hyperlink in MS Office, the initial MS Office request gets intercepted
    // here, so no authorisation redirects are sent back (which would lead the second 500 tab). MS Office then asks the
    // web browser to open the same URL, and it works how the user expected. The requests are intercepted based on the
    // user-agent header.

    // This class is taken from this StackOverflow answer for this problem
    // https://stackoverflow.com/a/52478746
    public class MsOfficeLinkPrefetchMiddleware
    {
        readonly RequestDelegate _next;

        public MsOfficeLinkPrefetchMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            if (RequestMethodMatches(context, HttpMethod.Get, HttpMethod.Head) && IsMsOffice(context))
            {
                // Mitigate by preempting auth challenges to MS Office apps' preflight requests and
                // let the real browser start at the original URL and handle all redirects and cookies.

                // Success response indicates to Office that the link is OK.
                context.Response.StatusCode = (int)HttpStatusCode.OK;
                context.Response.Headers["Cache-Control"] = "no-cache, no-store, must-revalidate";
                context.Response.Headers["Pragma"] = "no-cache";
                context.Response.Headers["Expires"] = "0";
            }
            else
            {
                await _next.Invoke(context);
            }
        }

        private static bool RequestMethodMatches(HttpContext context, params HttpMethod[] methods)
        {
            var requestMethod = context.Request.Method;
            return methods.Any(method => StringComparer.OrdinalIgnoreCase.Equals(requestMethod, method.Method));
        }

        private static readonly Regex MsOfficeUserAgent = new Regex(
            @"(^Microsoft Office\b)|([\(;]\s*ms-office\s*[;\)])",
            RegexOptions.CultureInvariant | RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);

        private static bool IsMsOffice(HttpContext context)
        {
            var headers = context.Request.Headers;

            var userAgent = headers["User-Agent"].FirstOrDefault();

            return (!string.IsNullOrWhiteSpace(userAgent) && MsOfficeUserAgent.IsMatch(userAgent))
                   || !string.IsNullOrWhiteSpace(headers["X-Office-Major-Version"]);
        }
    }
}
