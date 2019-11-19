using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public abstract class NotificationEditModelBase : NotificationModelBase
    {
        protected ValidationService ValidationService;

        protected NotificationEditModelBase(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(service, authorizationService, notificationRepository)
        {
            ValidationService = new ValidationService(this);
        }

        [ViewData]
        public Dictionary<string, NotifyError> NotifyErrorDictionary { get; set; }

        public virtual async Task<IActionResult> OnGetAsync(int id, bool isBeingSubmitted = false)
        {
            Notification = await GetNotification(id);
            if (Notification == null)
            {
                return NotFound();
            }

            NotificationId = Notification.NotificationId;

            await AuthorizeAndSetBannerAsync();
            if (!HasEditPermission)
            {
                return RedirectToOverview(NotificationId);
            }

            return await PreparePageForGet(NotificationId, isBeingSubmitted);
        }

        /*
        Post method accepts name of action specified by button clicked.
        Using handler appends handler to the url and would require awkward javascript logic
        to accomodate the URL changes for dynamic validation.
        */
        public async Task<IActionResult> OnPostAsync(string actionName, bool isBeingSubmitted)
        {
            Notification = await NotificationRepository.GetNotificationWithAllInfoAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }
            if (!(await AuthorizationService.CanEdit(User, Notification)))
            {
                return ForbiddenResult();
            }

            await AuthorizeAndSetBannerAsync();
            var isValid = await TryValidateAndSave();

            if (!isValid)
            {
                return await OnGetAsync(NotificationId);
            }

            switch (actionName)
            {
                case "Save":
                    return RedirectAfterSave(isBeingSubmitted);
                case "Submit":
                    return await Submit();
                case "Create":
                    return RedirectToCreate(NotificationId);
                default:
                    return BadRequest();
            }
        }


        public async Task<IActionResult> Submit()
        {
            SetShouldValidateFull();

            if (!TryValidateModel(Notification))
            {
                NotifyErrorDictionary = NotificationValidationErrorGenerator.MapToDictionary(ModelState, NotificationId);
                return Partial("./NotificationErrorSummary", this);
            }

            await Service.SubmitNotificationAsync(Notification);

            return RedirectToOverview(NotificationId);
        }

        private void SetShouldValidateFull()
        {
            Notification.ShouldValidateFull = true;
            foreach (var property in Notification.GetType().GetProperties())
            {
                if (property.PropertyType.IsSubclassOf(typeof(ModelBase)))
                {
                    var ownedModel = property.GetValue(Notification);
                    ownedModel.GetType().GetProperty("ShouldValidateFull").SetValue(ownedModel, true);
                }
            }
            Notification.NotificationSites.ForEach(x => x.ShouldValidateFull = Notification.ShouldValidateFull);
        }

        private IActionResult RedirectAfterSave(bool isBeingSubmitted)
        {
            if (Notification.NotificationStatus != NotificationStatus.Draft)
            {
                return RedirectToOverview(NotificationId);
            }

            return RedirectToNextPage(NotificationId, isBeingSubmitted);
        }

        protected IActionResult RedirectToOverview(int id)
        {
            return RedirectToPage("../Overview", new { id });
        }

        protected async Task SetNotificationProperties<T>(bool isBeingSubmitted, T ownedModel) where T : ModelBase
        {
            Notification.SetFullValidation(Notification.NotificationStatus, isBeingSubmitted);
            ownedModel.ShouldValidateFull = Notification.ShouldValidateFull;

            await GetLinkedNotifications();
        }

        private async Task<bool> TryValidateAndSave()
        {
            Notification.SetFullValidation(Notification.NotificationStatus);
            await ValidateAndSave();
            var modelStateIsValid = ModelState.IsValid;
            return modelStateIsValid;
        }

        public bool IsValid(string key)
        {
            return ValidationService.IsValid(key);
        }

        protected abstract Task ValidateAndSave();
        protected abstract Task<IActionResult> PreparePageForGet(int notificationId, bool isBeingSubmitted);
        protected abstract IActionResult RedirectToNextPage(int notificationId, bool isBeingSubmitted);

        protected virtual IActionResult RedirectToCreate(int notificationId)
        {
            throw new NotImplementedException();
        }
    }
}
