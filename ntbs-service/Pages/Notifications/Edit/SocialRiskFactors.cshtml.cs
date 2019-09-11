using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Pages;
using ntbs_service.Services;
using System.Linq;

namespace ntbs_service.Pages_Notifications
{
    public class SocialRiskFactorsModel : ValidationModel
    {
        private readonly INotificationService service;
        private readonly NtbsContext context;
        
        public List<Status> StatusList { get; set; }

        [BindProperty]
        public SocialRiskFactors SocialRiskFactors { get; set; }

        [BindProperty]
        public int NotificationId { get; set; }
        

        public SocialRiskFactorsModel(INotificationService service, NtbsContext context)
        {
            this.service = service;
            this.context = context;
        }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var notification = await service.GetNotificationWithSocialRisksAsync(id);
            NotificationId = notification.NotificationId;
            SocialRiskFactors = notification.SocialRiskFactors;

            StatusList = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            if (SocialRiskFactors == null)
            {
                return NotFound();
            }

            return Page();
        }

        public IActionResult OnGetPartial() => Partial("_RiskFactorPartial");

        public async Task<IActionResult> OnPostPreviousPageAsync(int? NotificationId)
        {
            var notification = await service.GetNotificationWithSocialRisksAsync(NotificationId);
            
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(NotificationId);
            }

            await service.UpdateSocialRiskFactorsAsync(notification, SocialRiskFactors);

            return RedirectToPage("./ClinicalTimeline", new {id = NotificationId});
        }

        public async Task<IActionResult> OnPostNextPageAsync(int? NotificationId)
        {
            var notification = await service.GetNotificationWithSocialRisksAsync(NotificationId);
            
            if (!ModelState.IsValid)
            {
                return await OnGetAsync(NotificationId);
            }

            await service.UpdateSocialRiskFactorsAsync(notification, SocialRiskFactors);

            return RedirectToPage("./PreviousHistory", new {id = NotificationId});
        }

        public ContentResult OnPostValidateSocialRiskFactorsProperty(string key, bool pastFive, bool moreThanFive, bool isCurrent, string status)
        {
            var riskStatus = status == null ? null : (Status?) Enum.Parse(typeof(Status), status);
            RiskFactorBase riskFactor = new RiskFactorBase {
                MoreThanFiveYearsAgo = moreThanFive,
                InPastFiveYears = pastFive,
                IsCurrent = isCurrent,
                Status = riskStatus
            }; 
            
            return OnPostValidateProperty(SocialRiskFactors, key, riskFactor);
      
        }
    }
}