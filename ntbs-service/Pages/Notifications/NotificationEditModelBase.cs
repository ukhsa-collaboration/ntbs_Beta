using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Interfaces;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications
{
    public abstract class NotificationEditModelBase : NotificationModelBase
    {
        protected readonly ValidationService ValidationService;
        private readonly IAlertRepository _alertRepository;

        protected NotificationEditModelBase(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IAlertRepository alertRepository)
            : base(service, authorizationService, notificationRepository)
        {
            ValidationService = new ValidationService(this);
            _alertRepository = alertRepository;
        }

        public Dictionary<NotificationSection, List<string>> NotifyErrorDictionary { get; private set; }

        [ViewData]
        public Dictionary<string, string> EditPageErrorDictionary { get; set; }

        /*
        Post method accepts name of action specified by button clicked.
        Using handler appends handler to the url and would require awkward javascript logic
        to accommodate the URL changes for dynamic validation.
        */
        [BindProperty]
        public string ActionName { get; set; }

        [ViewData]
        public string CurrentPage { get; set; }

        public virtual async Task<IActionResult> OnGetAsync(bool isBeingSubmitted = false)
        {
            PrepareBreadcrumbs();
            Notification = await GetNotificationAsync(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }

            await AuthorizeAndSetBannerAsync();
            if (PermissionLevel == PermissionLevel.ReadOnly)
            {
                return Notification.NotificationStatus == NotificationStatus.Draft
                    ? RedirectToPage("/Index")
                    : RedirectToPage("/Notifications/Overview", new {NotificationId});
            }
            
            if (PermissionLevel != PermissionLevel.Edit)
            {
                return RedirectForNotified();
            }

            if (Notification.NotificationStatus == NotificationStatus.Draft)
            {
                DraftAlert = await GetDraftAlertIfItExistsAsync();
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

            var (permissionLevel, _) = await _authorizationService.GetPermissionLevelAsync(User, Notification);
            if (permissionLevel != PermissionLevel.Edit)
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
                case ActionNameString.Submit:
                    return await SubmitAsync();
                case ActionNameString.Create:
                    return RedirectToCreate();
                default:
                    return RedirectAfterSave(isBeingSubmitted);
            }
        }

        private async Task SetNotification(string actionName)
        {
            if (actionName == ActionNameString.Submit)
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
                NotifyErrorDictionary = NotificationValidationErrorGenerator.MapToDictionary(ModelState);
                return Partial("./NotificationErrorSummary", this);
            }

            await Service.SubmitNotificationAsync(Notification);

            return RedirectToPage("/Notifications/Overview", new { NotificationId });
        }

        private IActionResult RedirectAfterSave(bool isBeingSubmitted)
        {
            if (Notification.NotificationStatus != NotificationStatus.Draft)
            {
                return RedirectForNotified();
            }

            return RedirectForDraft(isBeingSubmitted);
        }

        // By default saving a notified record takes user to Overview page,
        // but this can be overriden for sub-entity pages such as TestResult
        protected virtual IActionResult RedirectForNotified()
        {
            var overviewAnchorId = OverviewSubPathToAnchorMap.GetOverviewAnchorId(CurrentPage);
            return RedirectToPage(
                pageName: "/Notifications/Overview",
                pageHandler: null,
                routeValues: new { NotificationId },
                fragment: overviewAnchorId);
        }

        protected async Task SetNotificationProperties<T>(bool isBeingSubmitted, T subModel) where T : ModelBase
        {
            await SetNotificationProperties(isBeingSubmitted);
            subModel.SetValidationContext(Notification, isBeingSubmitted);
        }

        protected async Task SetNotificationProperties(bool isBeingSubmitted)
        {
            Notification.SetValidationContext(Notification, isBeingSubmitted);
            await GetLinkedNotificationsAsync();
        }

        private async Task<bool> TryValidateAndSave()
        {
            Notification.SetValidationContext(Notification);
            await ValidateAndSave();
            return ModelState.IsValid;
        }

        public bool IsValid(string key)
        {
            return ValidationService.IsValid(key);
        }

        protected async Task FindAndSetPostcodeAsync<T>(IPostcodeService postcodeService, T model) where T : IHasPostcode
        {
            var foundPostcode = await postcodeService.FindPostcodeAsync(model.Postcode);
            model.PostcodeToLookup = foundPostcode?.Postcode;
        }

        protected ContentResult CreateJsonResponse(object content)
        {
            return Content(JsonConvert.SerializeObject(content), "application/json");
        }

        protected abstract Task ValidateAndSave();
        protected abstract Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted);
        protected abstract IActionResult RedirectForDraft(bool isBeingSubmitted);

        protected virtual IActionResult RedirectToCreate()
        {
            throw new NotImplementedException();
        }

        private async Task<DataQualityDraftAlert> GetDraftAlertIfItExistsAsync()
        {
            return await _alertRepository.GetOpenDraftAlertForNotificationAsync(NotificationId);
        }
    }
}
