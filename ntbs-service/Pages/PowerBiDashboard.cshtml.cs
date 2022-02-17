using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;
using ntbs_service.Properties;

namespace ntbs_service.Pages
{
    public class PowerBiDashboard : PageModel
    {
        public string DashboardUrl { get; }

        public PowerBiDashboard(IConfiguration configuration)
        {
            var links = new ExternalLinks();
            configuration.GetSection(Constants.ExternalLinks).Bind(links);

            DashboardUrl = links.EmbeddedPowerBiDashboard;
        }
    }
}
