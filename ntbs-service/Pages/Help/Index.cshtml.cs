using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;

namespace ntbs_service.Pages.Help
{
    public class Index : PageModel
    {
        private readonly IFaqRepository _faqRepository;

        public IEnumerable<FrequentlyAskedQuestion> FAQs;

        public Index(IFaqRepository faqRepository)
        {
            _faqRepository = faqRepository;
        }

        public IActionResult OnGet()
        {
            FAQs = _faqRepository.GetAll();
            return Page();
        }
    }
}
