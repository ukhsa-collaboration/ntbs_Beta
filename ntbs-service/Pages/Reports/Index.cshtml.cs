using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ntbs_service
{
    /// <summary>
    /// Route intended to be both a dummy location that's navigable for
    /// when ExternalLinks__ReportingUri env variable is not set, and additional
    /// a centralised location for logic when routing out to external reporting.
    ///
    /// Intended for all external links to reporting to route through here, to then be
    /// redirected.
    /// </summary>
    public class IndexModel : PageModel
    {
        [BindProperty(SupportsGet = true)]
        public string ReportType { get; set; }

        public string ReportingUri { get; }

        public IndexModel(IConfiguration configuration)
        {
            ReportingUri = configuration.GetSection("ExternalLinks")?["ReportingUri"];
        }

        public IActionResult OnGet()
        {
            if (!string.IsNullOrEmpty(ReportingUri))
            {
                return Redirect(ReportingUri);
                // TODO: Add logic for routing to difference report types directly.
            }

            return Page();
        }
    }
}
