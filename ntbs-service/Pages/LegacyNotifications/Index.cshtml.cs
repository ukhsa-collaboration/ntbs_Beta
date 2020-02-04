using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataMigration;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages.LegacyNotifications
{
    public class Index : PageModel
    {
        private readonly ILegacySearchService _legacySearchService;
        private readonly INotificationImportService _notificationImportService;
        
        public NotificationBannerModel NotificationBanner { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string LegacyNotificationId { get; set; }
        
        public string RequestId { get; set; }
        public ImportResult LegacyImportResult { get; set; }

        public Index(ILegacySearchService legacySearchService, INotificationImportService notificationImportService)
        {
            _legacySearchService = legacySearchService;
            _notificationImportService = notificationImportService;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            return await GetPage();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            RequestId = HttpContext.TraceIdentifier;
            var idsList = new List<string> {LegacyNotificationId};
            LegacyImportResult = (await _notificationImportService.ImportByLegacyIdsAsync(null, RequestId, idsList)).FirstOrDefault();

            if (LegacyImportResult != null && LegacyImportResult.IsValid)
            {
                var notificationId = LegacyImportResult.NtbsIds[LegacyNotificationId];
                return RedirectToPage("/Notifications/Overview", new { NotificationId = notificationId });
            }

            return await GetPage();
        }

        private async Task<IActionResult> GetPage()
        {
            NotificationBanner = await _legacySearchService.GetByIdAsync(LegacyNotificationId);
            if (NotificationBanner == null)
            {
                return NotFound();
            }
            
            NotificationBanner.ShowLink = false;
            return Page(); 
        }
    }
}
