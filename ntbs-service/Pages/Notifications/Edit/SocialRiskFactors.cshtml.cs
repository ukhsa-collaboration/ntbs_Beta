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
        private readonly IAuditService auditService;
        
        public List<Status> StatusList { get; set; }

        [BindProperty]
        public SocialRiskFactors SocialRiskFactors { get; set; }

        public SocialRiskFactorsModel(INotificationService service, NtbsContext context, IAuditService auditService) : base(service)
        {
            this.context = context;
            this.auditService = auditService;
        }

        public override async Task<IActionResult> OnGetAsync(int? id, bool isBeingSubmitted)
        {
            if (id == null)
            {
                return NotFound();
            }

            Notification = await service.GetNotificationWithSocialRisksAsync(id);
            NotificationId = Notification.NotificationId;
            NotificationStatus = Notification.NotificationStatus;
            Notification.SetFullValidation(NotificationStatus, isBeingSubmitted);
            SocialRiskFactors = Notification.SocialRiskFactors;

            StatusList = Enum.GetValues(typeof(Status)).Cast<Status>().ToList();

            if (SocialRiskFactors == null)
            {
                return NotFound();
            }

            await auditService.OnGetAuditAsync(Notification.NotificationId, SocialRiskFactors);
            return Page();
        }

        protected override async Task<bool> ValidateAndSave(int? NotificationId) {            
            if (!ModelState.IsValid)
            {
                return false;
            }

            var notification = await service.GetNotificationWithSocialRisksAsync(NotificationId);
            await service.UpdateSocialRiskFactorsAsync(notification, SocialRiskFactors);
            return true;
        }

        protected override IActionResult RedirectToNextPage(int? notificationId)
        {
            return RedirectToPage("./Travel", new {id = notificationId});
        } 

        public ContentResult OnGetValidateSocialRiskFactorsProperty(string key, bool pastFive, bool moreThanFive, bool isCurrent, string status)
        {
            var riskStatus = status == null ? null : (Status?) Enum.Parse(typeof(Status), status);
            RiskFactorBase riskFactor = new RiskFactorBase {
                MoreThanFiveYearsAgo = moreThanFive,
                InPastFiveYears = pastFive,
                IsCurrent = isCurrent,
                Status = riskStatus
            }; 
            
            return ValidateProperty(new SocialRiskFactors(), key, riskFactor);
        }
    }
}