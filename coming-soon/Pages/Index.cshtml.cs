using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
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
