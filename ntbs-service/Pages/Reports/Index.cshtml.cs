using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;

namespace ntbs_service.Pages.Reports
{
    public class IndexModel : PageModel
    {
        public string ReportingPageExternalUrl { get; }

        public IndexModel(IReportingLinksService reportingLinksService)
        {
            ReportingPageExternalUrl = reportingLinksService.GetReportingPageUrl();
        }

        public IActionResult OnGet()
        {
            if (!string.IsNullOrEmpty(ReportingPageExternalUrl))
            {
                return Redirect(ReportingPageExternalUrl);
            }

            return Page();
        }
    }
}
