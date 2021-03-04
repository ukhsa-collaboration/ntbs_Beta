using System;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Configuration;

namespace coming_soon.Pages
{
    public class IndexModel : PageModel
    {
        public string ReportsUrl { get; set; }

        public IndexModel(IConfiguration Configuration)
        {
            ReportsUrl = Configuration.GetSection("App").GetValue<String>("ReportsUrl");
        }

        public void OnGet()
        {

        }
    }
}
