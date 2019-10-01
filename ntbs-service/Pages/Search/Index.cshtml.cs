using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Services;
using ntbs_service.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using ntbs_service.Models.Validations;
using ntbs_service.Pages;

namespace ntbs_service.Pages_Search
{
    public class IndexModel : ValidationModel
    {
        public INotificationService service;
        public int PageIndex;
        public PaginatedList<Notification> Notifications { get; set; }
        public IList<NotificationBannerModel> SearchResultsBannerDisplay;

        [RegularExpression(@"[0-9]+", ErrorMessage = "This can only contain digits 0-9")]
        [BindProperty(SupportsGet = true)]
        public string IdFilter { get; set; }
        public bool? DisplayCreateNotification { get; set; }

        public IndexModel(INotificationService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> OnGetAsync(int? pageIndex)
        {
            if(!ModelState.IsValid) 
            {
                return Page();
            }

            var pageSize = 50;

            PageIndex = pageIndex ?? 1;

            IQueryable<Notification> notificationsIQ = service.GetBaseNotificationIQueryable();
            
            if (!String.IsNullOrEmpty(IdFilter))
            {
                DisplayCreateNotification = true;
                notificationsIQ = notificationsIQ.Where(s => s.NotificationId.Equals(Int32.Parse(IdFilter)) 
                    || s.ETSID.Equals(IdFilter) || s.LTBRID.Equals(IdFilter) || s.PatientDetails.NhsNumber.Equals(IdFilter));
            }

            Notifications = await PaginatedList<Notification>.CreateAsync(
                notificationsIQ.AsNoTracking(), pageIndex ?? 1, pageSize);

            SearchResultsBannerDisplay = new List<NotificationBannerModel>();
            foreach(Notification result in Notifications) {
                SearchResultsBannerDisplay.Add(new NotificationBannerModel(result, true));
            }

            return Page();
        }

        public void OnGetSearch(int? pageIndex)
        {
            DisplayCreateNotification = true;
        }

        public ContentResult OnGetValidateSearchProperty(string key, string value)
        {
            return ValidateProperty(new IndexModel(service), key, value);
        }
    }
}
