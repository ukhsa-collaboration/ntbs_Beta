using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
            IAlertRepository alertRepository,
            INotificationRepository notificationRepository) : base(service, authorizationService, alertRepository, notificationRepository)
        {
            ValidationService = new ValidationService(this);
        }

        [ViewData]
        public Dictionary<string, NotifyError> NotifyErrorDictionary { get; set; }
        [ViewData]
        public Dictionary<string, string> EditPageErrorDictionary { get; set; }
        

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

            await GetAlertsAsync();

            return await PreparePageForGet(NotificationId, isBeingSubmitted);
        }

        protected virtual async Task<Notification> GetNotification(int notificationId)
        {
            return await NotificationRepository.GetNotificationAsync(notificationId);
        }

        /*
        Post method accepts name of action specified by button clicked.
        Using handler adds handler onto url and therefore breaking javascript
        validation happening after form is submitted
        */
        public async Task<IActionResult> OnPostAsync(string actionName, bool isBeingSubmitted)
        {
            // Get Notifications with all owned properties to check for 
            Notification = await NotificationRepository.GetNotificationWithAllInfoAsync(NotificationId); 
            if (Notification == null)
            {
                return NotFound();
            }
            else if (!(await AuthorizationService.CanEdit(User, Notification)))
            {
                return ForbiddenResult();
            }

            await AuthorizeAndSetBannerAsync();

            switch (actionName) 
            {
                case "Save":
                    return await Save(isBeingSubmitted);
                case "Submit":
                    return await Submit();
                default:
                    return Page();
            }
        }

        public async Task<IActionResult> Submit()
        {
            await ValidateAndSave();
            if (!ModelState.IsValid) 
            {
                return await OnGetAsync(NotificationId);
            }

            // IsRequired fields on Models requires ShouldValidateFull flag
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

        public async Task<IActionResult> Save(bool isBeingSubmitted)
        {           
            Notification.SetFullValidation(Notification.NotificationStatus);
            await ValidateAndSave();

            if (!ModelState.IsValid) 
            {
                EditPageErrorDictionary = EditPageValidationErrorGenerator.MapToDictionary(ModelState);
                return await OnGetAsync(NotificationId);
            }

            if (Notification.NotificationStatus != NotificationStatus.Draft) 
            {
                return RedirectToOverview(NotificationId);
            }

            return RedirectToNextPage(NotificationId, isBeingSubmitted);
        }

        protected IActionResult RedirectToOverview(int id)
        {
            return RedirectToPage("../Overview", new {id});
        }

        protected async Task SetNotificationProperties<T>(bool isBeingSubmitted, T ownedModel) where T : ModelBase
        {
            Notification.SetFullValidation(Notification.NotificationStatus, isBeingSubmitted);
            ownedModel.ShouldValidateFull = Notification.ShouldValidateFull;

            await GetLinkedNotifications();
        }

        public bool IsValid(string key)
        {
            return ValidationService.IsValid(key);
        }

        protected ContentResult CreateJsonResponse(object content)
        {
            return Content(JsonConvert.SerializeObject(content), "application/json");
        }

        protected abstract Task ValidateAndSave();
        protected abstract Task<IActionResult> PreparePageForGet(int notificationId, bool isBeingSubmitted);
        protected abstract IActionResult RedirectToNextPage(int notificationId, bool isBeingSubmitted);
    }
}
