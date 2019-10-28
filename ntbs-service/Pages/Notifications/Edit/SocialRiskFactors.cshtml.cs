using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Pages_Notifications;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class SocialRiskFactorsModel : NotificationEditModelBase
    {
        [BindProperty]
        public SocialRiskFactors SocialRiskFactors { get; set; }

        public SocialRiskFactorsModel(INotificationService service) : base(service) {}

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);
            SocialRiskFactors = Notification.SocialRiskFactors;
            await SetNotificationProperties(isBeingSubmitted, SocialRiskFactors);

            return Page();
        }

        protected override async Task ValidateAndSave() 
        {
            SocialRiskFactors.SetFullValidation(Notification.NotificationStatus);   
            if (TryValidateModel(SocialRiskFactors))
            {
                await service.UpdateSocialRiskFactorsAsync(Notification, SocialRiskFactors);
            }
        }

        protected override IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./Travel", new { id = notificationId, isBeingSubmitted });
        }
    }
}