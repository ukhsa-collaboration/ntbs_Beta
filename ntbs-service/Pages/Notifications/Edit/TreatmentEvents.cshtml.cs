using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class TreatmentEventsModel : NotificationEditModelBase
    {
        public IEnumerable<TreatmentEvent> TreatmentEvents { get; set; }

        public TreatmentEventsModel(
            INotificationService notificationService,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IAlertRepository alertRepository) : base(notificationService, authorizationService, notificationRepository, alertRepository)
        {
            CurrentPage = NotificationSubPaths.EditTreatmentEvents;
        }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            TreatmentEvents = Notification.TreatmentEvents.OrderForEpisodes();
            await SetNotificationProperties(isBeingSubmitted);

            return Page();
        }

        protected override IActionResult RedirectToCreate()
        {
            return RedirectToPage("./Items/NewTreatmentEvent", new { NotificationId });
        }

#pragma warning disable 1998
        protected override async Task ValidateAndSave()
        {
            // No validation or saving happening on list
        }
#pragma warning restore 1998


        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithTreatmentEventsAsync(notificationId);
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            string nextPage;
            if (Notification.IsMdr)
            {
                nextPage = "./MDRDetails";
            }
            else if (Notification.IsMBovis)
            {
                nextPage = "./MBovisExposureToKnownCases";
            }
            else
            {
                nextPage = "./TreatmentEvents";
            }
            return RedirectToPage(nextPage, new { NotificationId, isBeingSubmitted });
        }
    }
}
