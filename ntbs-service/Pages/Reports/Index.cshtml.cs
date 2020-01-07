using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace ntbs_service
{
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
