using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;

namespace ntbs_service.Pages
{
  public class LogoutModel : PageModel
    {
    private readonly string ReturnUrl;
    private readonly string BaseUrl;

    public LogoutModel(IOptionsMonitor<AdfsOptions> options)
    {
        // We just want to return to the homepage (which will trigger going to login again)
        ReturnUrl = options.CurrentValue.Wtrealm;
        BaseUrl = options.CurrentValue.AdfsUrl;
    }

    public RedirectResult OnGet()
        {
            // Erase the cookie ...
            HttpContext.SignOutAsync();

            // ... and sign out of adfs
            return Redirect($"{BaseUrl}/adfs/ls/?wa=wsignout1.0&wreply={ReturnUrl}");
        }
    }
}