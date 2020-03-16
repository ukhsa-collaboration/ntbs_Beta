
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Helpers;
using ntbs_service.Services;

namespace ntbs_service.Pages
{
    public class Heartbeat : PageModel
    {
        private readonly ISessionStateService _sessionStateService;

        public Heartbeat(ISessionStateService sessionStateService)
        {
            _sessionStateService = sessionStateService;
        }

        public IActionResult OnGet()
        {
            return Page();
            
        }
        
        public ContentResult OnGetIsActive()
        { 
            var cookie = CookieHelper.GetUserCookie(HttpContext.Request);
            return new ContentResult
            {
                Content = _sessionStateService.IsUpdatedRecently(cookie).ToString()
            };
        }

        public void OnGetUpdateActivity()
        {
            var cookie = CookieHelper.GetUserCookie(HttpContext.Request);
            _sessionStateService.UpdateSessionStateService(cookie);
        }
    }
}
