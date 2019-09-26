using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Data.Legacy;
using ntbs_service.Services;
using ntbs_service.Models;

namespace ntbs_service.Pages_Search
{
    public class IndexModel : PageModel
    {
        public INotificationService service;
        public int Offset;
        public int PageSize;
        public SearchResults SearchResults;
        public IList<NotificationBannerModel> SearchResultsBannerDisplay;

        public IndexModel(INotificationService service)
        {
            this.service = service;
        }

        public async Task<IActionResult> OnGetAsync()
        {
            Offset = 0;
            PageSize = 50;
            SearchResults = await service.getSearchResultsAsync(Offset, PageSize);
            SearchResultsBannerDisplay = new List<NotificationBannerModel>();
            foreach(Notification result in SearchResults.notifications) {
                SearchResultsBannerDisplay.Add(new NotificationBannerModel(result, true));
            }

            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            return Page();
        }
    }
}
