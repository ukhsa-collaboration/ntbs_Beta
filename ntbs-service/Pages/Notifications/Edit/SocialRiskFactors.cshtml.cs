using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Services;
using System.Linq;

namespace ntbs_service.Pages_Notifications
{
    public class SocialRiskFactorsModel : NotificationModelBase
    {
        private readonly NtbsContext context;
        
        public List<Status> StatusList { get; set; }

        [BindProperty]
        public SocialRiskFactors SocialRiskFactors { get; set; }

        public SocialRiskFactorsModel(INotificationService service, NtbsContext context) : base(service)
        {
            this.context = context;
        }

        public override async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted)
        {
            Notification = await service.GetNotificationWithSocialRisksAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);
            SocialRiskFactors = Notification.SocialRiskFactors;
            if (SocialRiskFactors == null)
            {
                return NotFound();
            }
            
            SetNotificationProperties<SocialRiskFactors>(isBeingSubmitted, SocialRiskFactors);

            StatusList = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();
            return Page();
        }

        protected override async Task<bool> ValidateAndSave() {
            SocialRiskFactors.SetFullValidation(Notification.NotificationStatus);   
            if (!TryValidateModel(SocialRiskFactors))
            {
                return false;
            }

            await service.UpdateSocialRiskFactorsAsync(Notification, SocialRiskFactors);
            return true;
        }

        protected override IActionResult RedirectToNextPage(int? notificationId)
        {
            return RedirectToPage("./Travel", new {id = notificationId});
        }
    }
}