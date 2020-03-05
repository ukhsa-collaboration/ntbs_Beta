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

    public LogoutModel(IOptionsMonitor<AdfsOptions> options)
    {
        // We just want to return to the homepage (which will trigger going to login again)
        ReturnUrl = $"{options.CurrentValue.Wtrealm}Index";
        BaseUrl = options.CurrentValue.AdfsUrl;
    }

    public async Task<RedirectResult> OnGetAsync()
        {
            // Erase the cookie ...
            await HttpContext.SignOutAsync();

            // ... and sign out of adfs
            return Redirect($"{BaseUrl}/adfs/ls/?wa=wsignout1.0&wreply={ReturnUrl}");
        }
    }
}
