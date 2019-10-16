using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Data.Legacy;

namespace ntbs_service.Pages_SearchOld
{
    public class IndexModel : PageModel
    {
        private readonly ISearchServiceLegacy _searcher;

        public IndexModel(ISearchServiceLegacy searcher)
        {
            _searcher = searcher;
            Results = new List<SearchResult>();
        }

        [BindProperty]
        public SearchRequest SearchRequest { get; set; }

        public IList<SearchResult> Results { get; set; }

        public IActionResult OnGetAsync()
        {
            return Page();
        }

        public IActionResult OnPost()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Results = _searcher.Search(SearchRequest).ToList();
            return Page();
        }
    }
}
