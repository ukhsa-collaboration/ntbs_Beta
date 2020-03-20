using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ntbs_service.Helpers;
using ntbs_service.Services;

namespace ntbs_service.Pages
{
    public class Heartbeat : PageModel
    {
        public JsonResult OnGetIsActive()
        {
            var isActive = SessionStateHelper.IsUpdatedRecently(HttpContext.Session);

            return new JsonResult(new 
            {
                IsActive = isActive
            });
        }

        public void OnGetUpdateActivity()
        {
            SessionStateHelper.UpdateSessionActivity(HttpContext.Session);
        }
    }
}
