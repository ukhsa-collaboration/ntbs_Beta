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
        private readonly IOptionsMonitor<AzureAdOptions> _azureAdOptions;
        private readonly IOptionsMonitor<AdfsOptions> _adfsOptions;

        private string IndexUrl => $"{Request.Scheme}://{Request.Host}/Index";

        public LogoutModel(IOptionsMonitor<AdfsOptions> adfsOptions, IOptionsMonitor<AzureAdOptions> azureAdOptions)
        {
            _azureAdOptions = azureAdOptions;
            _adfsOptions = adfsOptions;
        }

        public async Task<RedirectResult> OnGetAsync()
        {
            var redirectUrl = _azureAdOptions.CurrentValue.Enabled ? AzureAdRedirectUrl() : AdfsRedirectUrl();

            // Erase the cookie ...
            await HttpContext.SignOutAsync();

            // ... and sign out of the AD
            // We want to return to the homepage, which will trigger going to login again
            return Redirect(redirectUrl);
        }

        private string AzureAdRedirectUrl()
        {
            var baseUrl = _azureAdOptions.CurrentValue.Authority;
            var clientId = _azureAdOptions.CurrentValue.ClientId;
            return $"{baseUrl}/oauth2/logout?client_id={clientId}&post_logout_redirect_uri={IndexUrl}";
        }

        private string AdfsRedirectUrl()
        {
            var baseUrl = _adfsOptions.CurrentValue.AdfsUrl;
            return $"{baseUrl}/adfs/ls/?wa=wsignout1.0&wreply={IndexUrl}";
        }
    }
}
