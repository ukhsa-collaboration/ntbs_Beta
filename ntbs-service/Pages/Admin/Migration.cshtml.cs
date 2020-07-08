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

        public MigrationModel(IOptions<MigrationConfig> config)
        {
            _config = config.Value;
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

        public bool Triggered { get; private set; }
        public ValidationService ValidationService { get; }

        private string RequestId =>  HttpContext.TraceIdentifier;


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
                TriggerImportFromIdsField();
            }
            else if (UploadedFile != null)
            {
                await TriggerImportFromIdsFile();
            }
            else if (NotificationDateRangeStart != null)
            {
                TriggerImportFromDateRange();
            }

            Triggered = true;

            return Page();
        }

        private void TriggerImportFromIdsField()
        {
            var legacyIds = NotificationIds.Split(',').Select(id => id.Trim()).ToList();
            BackgroundJob.Enqueue<INotificationImportService>(x =>
                x.ImportByLegacyIdsAsync(null, RequestId, legacyIds));
        }

        private async Task TriggerImportFromIdsFile()
        {
            var notificationIds = await GetIdListFromFile(UploadedFile);
            var idBatches = SplitList(notificationIds);

            foreach (var idBatch in idBatches)
            {
                BackgroundJob.Enqueue<INotificationImportService>(x => x.ImportByLegacyIdsAsync(null, RequestId, idBatch));
            }
        }

        private void TriggerImportFromDateRange()
        {
            NotificationDateRangeStart.TryConvertToDateTimeRange(out DateTime? notificationDateRangeStart, out _);
            NotificationDateRangeEnd.TryConvertToDateTimeRange(out DateTime? notificationDateRangeEnd, out _);
            if (notificationDateRangeStart == null)
            {
                throw new ArgumentException("Could not parse the start date");
            }

            var rangeEnd = notificationDateRangeEnd ?? DateTime.Now.AddDays(1);

            for (var batchStart = (DateTime) notificationDateRangeStart;
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
                    x.ImportByDateAsync(null, RequestId, start, end));
            }
        }

        private static async Task<List<string>> GetIdListFromFile(IFormFile file)
        {
            var legacyIds = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    legacyIds.Add(await reader.ReadLineAsync());
            }
            return legacyIds;
        }

        private static IEnumerable<List<T>> SplitList<T>(List<T> legacyIds, int nSize = 1000)
        {
            for (int index = 0; index < legacyIds.Count; index += nSize)
            {
                yield return legacyIds.GetRange(index, Math.Min(nSize, legacyIds.Count - index));
            }
        }
    }
}
