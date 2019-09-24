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
    public class OverviewModel : NotificationModelBase
    {

        public OverviewModel(INotificationService service) : base(service)
        {
            this.service = service;
        }

        public override async Task<IActionResult> OnGetAsync(int? id)
        {
            Notification = await service.GetNotificationWithAllInfoAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationId = Notification.NotificationId;

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int? notificationId)
        {
            // This is not needed on the overview page. We should think about restructuring the
            // inheritance to accommodate pages like this one without this hack
            throw new NotImplementedException();
        }

        protected override Task<bool> ValidateAndSave(int? notificationId)
        {
            // This is not needed on the overview page. We should think about restructuring the
            // inheritance to accommodate pages like this one without this hack
            throw new NotImplementedException();
        }
    }
}