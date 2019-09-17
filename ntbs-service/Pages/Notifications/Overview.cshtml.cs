using System;
using System.Globalization;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Pages;
using ntbs_service.Services;

namespace ntbs_service.Pages_Notifications
{
    public class OverviewModel : ValidationModel
    {
        private readonly INotificationService service;
        private readonly NtbsContext context;

        public OverviewModel(INotificationService service, NtbsContext context)
        {
            this.service = service;
            this.context = context;
        }

        [BindProperty]
        public Notification Notification { get; set; }
        [BindProperty]
        public int NotificationId { get; set; }
        [BindProperty]
        public string DrugRiskFactorTimePeriods { get; set; }
        [BindProperty]
        public string HomelessRiskFactorTimePeriods { get; set; }
        [BindProperty]
        public string ImprisonmentRiskFactorTimePeriods { get; set; }
        [BindProperty]
        public string Postcode { get; set; }
        [BindProperty]
        public string SitesOfDisease { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Notification = await service.GetNotificationWithAllInfoAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationId = Notification.NotificationId;
            
            DrugRiskFactorTimePeriods = CreateTimePeriodsString(Notification.SocialRiskFactors.RiskFactorDrugs);
            HomelessRiskFactorTimePeriods = CreateTimePeriodsString(Notification.SocialRiskFactors.RiskFactorHomelessness);
            ImprisonmentRiskFactorTimePeriods = CreateTimePeriodsString(Notification.SocialRiskFactors.RiskFactorImprisonment);

            var postcodeNoWhiteSpace = Notification.PatientDetails.Postcode.Replace(" ", string.Empty);
            Postcode = postcodeNoWhiteSpace.Insert(postcodeNoWhiteSpace.Length - 3, " ");

            for(var i = 0; i < Notification.NotificationSites.Count; i++) {
                SitesOfDisease = SitesOfDisease + Notification.NotificationSites[i].Site.Description;
                if(i < Notification.NotificationSites.Count - 1) {
                    SitesOfDisease = SitesOfDisease + ", ";
                }
            }

            return Page();
        }

        public string CreateTimePeriodsString(RiskFactorBase riskFactor) {
            string timeString = "";
            if(riskFactor.IsCurrent) {
                timeString = timeString + "current";
            }
            if(riskFactor.InPastFiveYears) {
                timeString = timeString + (String.IsNullOrEmpty(timeString) ? "less than 5 years ago" : ", less than 5 years ago");
            }
            if(riskFactor.MoreThanFiveYearsAgo) {
                timeString = timeString + (String.IsNullOrEmpty(timeString) ? "more than 5 years ago" : ", more than 5 years ago");
            }
            return timeString;
        }
    }
}