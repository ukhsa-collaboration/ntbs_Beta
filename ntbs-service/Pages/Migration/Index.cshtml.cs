using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataMigration;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System.IO;
using ntbs_service.Services;
using System.ComponentModel.DataAnnotations;
using System;
using Hangfire;

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
            ValidationService = new ValidationService(this);
        }

        [BindProperty]
        public string NotificationId { get; set; }

        [BindProperty]
        public IFormFile UploadedFile { get; set; }

        [BindProperty]
        [Display(Name = "Cutoff Notification Date")]
        public PartialDate CutoffNotificationDate { get; set; }

        public IList<Notification> Results { get; set; }
        public ValidationService ValidationService { get; }

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

            var requestId = HttpContext.TraceIdentifier;

            if (UploadedFile != null)
            {
                var notificationIds = await GetIdListFromFile(UploadedFile);
                var IdGroups = splitList(notificationIds, 10);

                foreach (var idGroup in IdGroups)
                {
                    BackgroundJob.Enqueue<INotificationImportService>(x => x.ImportNotificationsByIdAsync(requestId, idGroup));
                }
            }
            else if (CutoffNotificationDate != null)
            {
                CutoffNotificationDate.TryConvertToDateTimeRange(out DateTime? dateRangeStart, out DateTime? dateRangeEnd);
                Results = await _service.ImportNotificationsByDateAsync(requestId, (DateTime) dateRangeStart);
            }

            return Page();
        }

        private async Task<List<string>> GetIdListFromFile(IFormFile file)
        {
            var ids = new List<string>();
            using (var reader = new StreamReader(file.OpenReadStream()))
            {
                while (reader.Peek() >= 0)
                    ids.Add(await reader.ReadLineAsync()); 
            }
            return ids;
        }

        public static IEnumerable<List<T>> splitList<T>(List<T> ids, int nSize=1000)  
        {        
            for (int index = 0; index < ids.Count; index += nSize) 
            { 
                yield return ids.GetRange(index, Math.Min(nSize, ids.Count - index)); 
            }  
        } 
    }
}
