using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Interfaces;
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

        [ViewData]
        public Dictionary<string, string> EditPageErrorDictionary { get; set; }

        /*
        Post method accepts name of action specified by button clicked.
        Using handler appends handler to the url and would require awkward javascript logic
        to accommodate the URL changes for dynamic validation.
        */
        [BindProperty]
        public string ActionName { get; set; }

        public virtual async Task<IActionResult> OnGetAsync(bool isBeingSubmitted = false)
        {
            Notification = await GetNotificationAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }

            await AuthorizeAndSetBannerAsync();
            if (PermissionLevel != PermissionLevel.Edit)
            {
                return RedirectAfterSaveForNotified();
            }

            return await PrepareAndDisplayPageAsync(isBeingSubmitted);
        }

        public async Task<IActionResult> OnPostAsync(bool isBeingSubmitted)
        {
            await SetNotification(ActionName);

            if (Notification == null)
            {
                return NotFound();
            }
            if (await AuthorizationService.GetPermissionLevelForNotificationAsync(User, Notification) == PermissionLevel.Edit)
            {
                return ForbiddenResult();
            }

            await AuthorizeAndSetBannerAsync();
            var isValid = await TryValidateAndSave();

            if (!isValid)
            {
                EditPageErrorDictionary = EditPageValidationErrorGenerator.MapToDictionary(ModelState);
                return await PrepareAndDisplayPageAsync(isBeingSubmitted);
            }

            switch (ActionName)
            {
                case "Submit":
                    return await SubmitAsync();
                case "Create":
                    return RedirectToCreate();
                case "Save": // intentional fall-through: treating Save as the default case
                default:
                    return RedirectAfterSave(isBeingSubmitted);
            }
        }

        private async Task SetNotification(string actionName)
        {
            if (actionName == "Submit")
            {
                // When submitting, we need to validate the entire record, not just the currently edited page
                Notification = await NotificationRepository.GetNotificationWithAllInfoAsync(NotificationId);
            }
            else
            {
                Notification = await GetNotificationAsync(NotificationId);
            }
        }

        private async Task<IActionResult> SubmitAsync()
        {
            NotificationHelper.SetShouldValidateFull(Notification);

            if (!TryValidateModel(Notification))
            {
                NotifyErrorDictionary = NotificationValidationErrorGenerator.MapToDictionary(ModelState, NotificationId);
                return Partial("./NotificationErrorSummary", this);
            }

            await Service.SubmitNotificationAsync(Notification);

            return RedirectAfterSaveForNotified();
        }           

        private IActionResult RedirectAfterSave(bool isBeingSubmitted)
        {
            if (Notification.NotificationStatus != NotificationStatus.Draft)
            {
                return RedirectAfterSaveForNotified();
            }

            return RedirectAfterSaveForDraft(isBeingSubmitted);
        }

        // By default saving a notified record takes user to Overview page,
        // but this can be overriden for sub-entity pages such as TestResult
        protected virtual IActionResult RedirectAfterSaveForNotified()
        {
            return RedirectToPage("/Notifications/Overview", new { NotificationId });
        }

        protected async Task SetNotificationProperties<T>(bool isBeingSubmitted, T ownedModel) where T : ModelBase
        {
            await SetNotificationProperties(isBeingSubmitted);
            ownedModel.ShouldValidateFull = Notification.ShouldValidateFull;
        }

        protected async Task SetNotificationProperties(bool isBeingSubmitted)
        {
            Notification.SetFullValidation(Notification.NotificationStatus, isBeingSubmitted);
            await GetLinkedNotifications();
        }

        private async Task<bool> TryValidateAndSave()
        {
            Notification.SetFullValidation(Notification.NotificationStatus);
            await ValidateAndSave();
            return ModelState.IsValid;
        }

        public bool IsValid(string key)
        {
            return ValidationService.IsValid(key);
        }

        protected async Task FindAndSetPostcodeAsync<T>(IPostcodeService postcodeService, T model) where T : IHasPostcode
        {
            var foundPostcode = await postcodeService.FindPostcode(model.Postcode);
            model.PostcodeToLookup = foundPostcode?.Postcode;
        }

        public async Task<ContentResult> OnGetValidatePostcode<T>(IPostcodeService postcodeService, string postcode, bool shouldValidateFull) where T : ModelBase, IHasPostcode
        {
            var foundPostcode = await postcodeService.FindPostcode(postcode);
            var propertyValueTuples = new List<(string, object)>
            {
                ("PostcodeToLookup", foundPostcode?.Postcode),
                ("Postcode", postcode)
            };

            return ValidationService.GetMultiplePropertiesValidationResult<T>(propertyValueTuples, shouldValidateFull);
        }


        protected ContentResult CreateJsonResponse(object content)
        {
            return Content(JsonConvert.SerializeObject(content), "application/json");
        }

        protected abstract Task ValidateAndSave();
        protected abstract Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted);
        protected abstract IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted);

        protected virtual IActionResult RedirectToCreate()
        {
            throw new NotImplementedException();
        }
    }
}
