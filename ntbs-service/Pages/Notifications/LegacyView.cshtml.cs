using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataMigration;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public class LegacyView : PageModel
    {
        private readonly ILegacySearchService _legacySearchService;
        private readonly INotificationImportService _notificationImportService;

        [BindProperty] 
        public NotificationBannerModel NotificationBanner { get; set; }
        
        [BindProperty(SupportsGet = true)]
        public string LegacyNotificationId { get; set; }
        
        public string RequestId { get; set; }
        public ImportResult LegacyImportResult { get; set; }

        public LegacyView(ILegacySearchService legacySearchService, INotificationImportService notificationImportService)
        {
            _legacySearchService = legacySearchService;
            _notificationImportService = notificationImportService;
        }
        
        public async Task<IActionResult> OnGetAsync()
        {
            await GetLegacyNotificationDetailsForBanner();
            return Page();
        }

        private async Task GetLegacyNotificationDetailsForBanner()
        {
            NotificationBanner = await _legacySearchService.SearchByIdAsync(LegacyNotificationId);
            NotificationBanner.ShowLink = false;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            RequestId = HttpContext.TraceIdentifier;
            var idsList = new List<string> {LegacyNotificationId};
            ImportResult importResult = (await _notificationImportService.ImportByLegacyIdsAsync(null, RequestId, idsList)).FirstOrDefault();

            if (importResult != null && importResult.IsValid)
            {
                var notificationId = importResult.NtbsIds[LegacyNotificationId];
                return RedirectToPage("/Notifications/Overview", new { NotificationId = notificationId });
            }
                
            LegacyImportResult = importResult;

            await GetLegacyNotificationDetailsForBanner();
            return Page();
        }
    }
}
