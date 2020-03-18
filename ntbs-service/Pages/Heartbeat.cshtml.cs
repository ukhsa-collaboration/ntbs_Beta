using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
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
        
        public JsonResult OnGetIsActive()
        {
            var isActive = _sessionStateService.IsUpdatedRecently(HttpContext.Session);

            return new JsonResult(new 
            {
                IsActive = isActive
            });
        }

        public void OnGetUpdateActivity()
        {
            _sessionStateService.UpdateSessionActivity(HttpContext.Session);
        }
    }
}
