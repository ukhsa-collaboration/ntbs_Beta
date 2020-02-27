using System;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ntbs_service.Pages
{
    public class Health : PageModel
    {
        public Health()
        {
            Release = Environment.GetEnvironmentVariable("RELEASE");
        }

        public string Release { get; }
    }
}
