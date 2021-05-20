using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hangfire;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Options;
using ntbs_service.DataMigration;
using ntbs_service.Models;
using ntbs_service.Properties;
using ntbs_service.Services;

namespace ntbs_service.Pages.Admin
{
    [Authorize(Policy = "AdminOnly")]
    public class MigrationModel : PageModel
    {
        private readonly MigrationConfig _config;
        private readonly INotificationImportRepository _notificationImportRepository;

        public MigrationModel(IOptions<MigrationConfig> config,
            INotificationImportRepository notificationImportRepository)
        {
            _config = config.Value;
            _notificationImportRepository = notificationImportRepository;
            ValidationService = new ValidationService(this);
        }

        [BindProperty]
        public string NotificationIds { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        [BindProperty]
        [Display(Name = "Start Notification Date")]
        public PartialDate NotificationDateRangeStart { get; set; }


        [BindProperty]
        [Display(Name = "End Notification Date")]
        public PartialDate NotificationDateRangeEnd { get; set; }

        [BindProperty]
        public string Description { get; set; }

        public bool Triggered { get; private set; }
        public ValidationService ValidationService { get; }

        public int RunId { get; private set; }


        public IActionResult OnGetAsync()
        {
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (NotificationIds != null)
            {
                await TriggerImportFromIdsField();
            }
            else if (UploadedFile != null)
            {
                await TriggerImportFromIdsFile();
            }
            else if (NotificationDateRangeStart != null)
            {
                await TriggerImportFromDateRange();
            }

            Triggered = true;

            return Page();
        }

        private async Task TriggerImportFromIdsField()
        {
            var legacyIds = NotificationIds.Split(',').Select(id => id.Trim()).ToList();
            var migrationRun =
                await _notificationImportRepository.CreateLegacyImportMigrationRun(legacyIds, description: Description);
            RunId = migrationRun.LegacyImportMigrationRunId;

            foreach (var idBatch in SplitList(legacyIds))
            {
                BackgroundJob.Enqueue<INotificationImportService>(x =>
                    x.BulkImportByLegacyIdsAsync(null, RunId, idBatch));
            }
        }

        private async Task TriggerImportFromIdsFile()
        {
            var notificationIds = await GetIdListFromFile(UploadedFile);
            var migrationRun = await _notificationImportRepository.CreateLegacyImportMigrationRun(
                notificationIds,
                UploadedFile.FileName,
                Description);
            RunId = migrationRun.LegacyImportMigrationRunId;

            foreach (var idBatch in SplitList(notificationIds))
            {
                BackgroundJob.Enqueue<INotificationImportService>(x =>
                    x.BulkImportByLegacyIdsAsync(null, RunId, idBatch));
            }
        }

        private async Task TriggerImportFromDateRange()
        {
            NotificationDateRangeStart.TryConvertToDateTimeRange(out var notificationDateRangeStart, out _);
            NotificationDateRangeEnd.TryConvertToDateTimeRange(out var notificationDateRangeEnd, out _);
            if (notificationDateRangeStart == null)
            {
                throw new ArgumentException("Could not parse the start date");
            }

            var rangeStart = notificationDateRangeStart.Value;

            var rangeEnd = notificationDateRangeEnd ?? DateTime.Now.AddDays(1);

            var migrationRun =
                await _notificationImportRepository.CreateLegacyImportMigrationRun(rangeStart, rangeEnd, Description);
            RunId = migrationRun.LegacyImportMigrationRunId;

            for (var batchStart = rangeStart;
                batchStart < rangeEnd;
                batchStart = batchStart.AddDays(_config.DateRangeJobIntervalInDays))
            {
                var start = batchStart; // We need to capture the variable locally!
                var end = batchStart.AddDays(_config.DateRangeJobIntervalInDays);
                if (end > rangeEnd)
                {
                    end = rangeEnd;
                }

                BackgroundJob.Enqueue<INotificationImportService>(x =>
                    x.BulkImportByDateAsync(null, RunId, start, end));
            }
        }

        private static async Task<List<string>> GetIdListFromFile(IFormFile file)
        {
            var legacyIds = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                {
                    legacyIds.Add((await reader.ReadLineAsync()).Trim());
                }
            }
            return legacyIds;
        }

        private IEnumerable<List<T>> SplitList<T>(List<T> legacyIds)
        {
            var size = _config.ByIdsJobBatchSize;
            for (var index = 0; index < legacyIds.Count; index += size)
            {
                yield return legacyIds.GetRange(index, Math.Min(size, legacyIds.Count - index));
            }
        }
    }
}
