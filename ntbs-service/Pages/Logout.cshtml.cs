using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using ntbs_service.Properties;

namespace ntbs_service.Pages
{
    public class LogoutModel : PageModel
    {
        private readonly string ReturnUrl;
        private readonly string BaseUrl;

        private readonly string RedirectUrl;

        public LogoutModel(IOptionsMonitor<AdfsOptions> options, IOptionsMonitor<AzureAdOptions> azureAdOptions)
        {
            // We just want to return to the homepage (which will trigger going to login again)
            // Check to see if Azure Ad Auth is enabled.
            if (azureAdOptions.CurrentValue.Enabled)
            {
                BaseUrl = azureAdOptions.CurrentValue.Authority;
                RedirectUrl =
                    $"{BaseUrl}/oauth2/logout?client_id={azureAdOptions.CurrentValue.ClientId}&post_logout_redirect_uri={options.CurrentValue.Wtrealm}";
            }
            else
            {
                ReturnUrl = $"{options.CurrentValue.Wtrealm}Index";
                BaseUrl = options.CurrentValue.AdfsUrl;
                RedirectUrl = $"{BaseUrl}/adfs/ls/?wa=wsignout1.0&wreply={ReturnUrl}";
            }
        }

        public async Task<RedirectResult> OnGetAsync()
        {
            // Erase the cookie ...
            await HttpContext.SignOutAsync();

            // ... and sign out of adfs
            return Redirect(RedirectUrl);
        }
    }
}
