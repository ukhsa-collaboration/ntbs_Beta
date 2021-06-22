using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Pages.Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.LegacyNotifications
{
    public class Index : PageModel, IWithNotificationBanner
    {
        private readonly ILegacySearchService _legacySearchService;
        private readonly INotificationImportService _notificationImportService;
        private readonly INotificationImportRepository _notificationImportRepository;
        private readonly INotificationRepository _notificationRepository;
        private readonly IUserHelper _userHelper;

        public NotificationBannerModel NotificationBannerModel { get; set; }

        [BindProperty(SupportsGet = true)]
        public string LegacyNotificationId { get; set; }

        public int RunId { get; set; }
        public ImportResult LegacyImportResult { get; set; }

        public Index(ILegacySearchService legacySearchService,
            INotificationImportService notificationImportService,
            INotificationImportRepository notificationImportRepository,
            INotificationRepository notificationRepository,
            IUserHelper userHelper)
        {
            _legacySearchService = legacySearchService;
            _notificationImportService = notificationImportService;
            _notificationImportRepository = notificationImportRepository;
            _notificationRepository = notificationRepository;
            _userHelper = userHelper;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            if (_userHelper.UserIsReadOnly(User))
            {
                return RedirectToPage(RouteHelper.AccessDeniedPath);
            }
            ViewData["Breadcrumbs"] = new List<Breadcrumb>
            {
                HttpContext.Session.GetTopLevelBreadcrumb(),
            };

            return await GetPageAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var idsList = new List<string> { LegacyNotificationId };
            var migrationRun = await _notificationImportRepository.CreateLegacyImportMigrationRun(idsList);
            RunId = migrationRun.LegacyImportMigrationRunId;
            LegacyImportResult =
                (await _notificationImportService.ImportByLegacyIdsAsync(null, RunId, idsList)).FirstOrDefault();

            if (LegacyImportResult != null && LegacyImportResult.IsValid)
            {
                var notificationId = LegacyImportResult.NtbsIds[LegacyNotificationId];
                return RedirectToPage("/Notifications/Overview", new { NotificationId = notificationId });
            }

            return await GetPageAsync();
        }

        private async Task<IActionResult> GetPageAsync()
        {
            NotificationBannerModel = await _legacySearchService.GetByIdAsync(LegacyNotificationId);
            if (NotificationBannerModel == null)
            {
                var notificationId = await _notificationRepository.GetNotificationIdByLegacyIdAsync(LegacyNotificationId);
                if (notificationId != 0)
                {
                    return RedirectToPage("/Notifications/Overview", new { NotificationId = notificationId });
                }

                return NotFound();
            }

            NotificationBannerModel.ShowLink = false;
            return Page();
        }
    }
}
