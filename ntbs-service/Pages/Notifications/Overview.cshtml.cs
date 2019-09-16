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

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            Notification = await service.GetNotificationAsync(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationId = Notification.NotificationId;

            return Page();
        }
    }
}