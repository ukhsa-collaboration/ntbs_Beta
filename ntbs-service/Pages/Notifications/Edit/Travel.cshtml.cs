using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    // TODO - To complete when spec is ready for this page
    public class TravelModel : NotificationEditModelBase
    {
        public TravelModel(INotificationService service) : base(service) {}

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null) 
            {
                return NotFound();
            }
            
            NotificationId = id;

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId)
        {
            return RedirectToPage("./Comorbidities", new { id = notificationId });
        }

        protected override async Task<bool> ValidateAndSave()
        {
            return true;
        }
    }
}