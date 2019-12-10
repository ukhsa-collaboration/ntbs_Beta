using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataMigration;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace ntbs_service.Pages.Migration
{
    [Authorize(Policy = "AdminOnly")]
    public class IndexModel : PageModel
    {
        private readonly INotificationImportService _service;

        public IndexModel(INotificationImportService service)
        {
            _service = service;
            Results = new List<Notification>();
        }

        [BindProperty]
        public string NotificationId { get; set; }

        public IList<Notification> Results { get; set; }

        public IActionResult OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var requestId = HttpContext.TraceIdentifier;

            Results = await _service.ImportNotificationsAsync(requestId, NotificationId);
            return Page();
        }
    }
}
