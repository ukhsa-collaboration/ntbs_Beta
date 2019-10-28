using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.Models.Enums;
using ntbs_service.Models;
using ntbs_service.Services;
using ntbs_service.Helpers;

namespace ntbs_service.Pages_Notifications
{
    public class NotifyError {
        public string Url { get; set; }
        public List<string> ErrorMessages { get; set; }
    }

    // Needed by all Notification edit pages
    public abstract class NotificationEditModelBase : NotificationModelBase
    {
        protected ValidationService validationService;

        public NotificationEditModelBase(INotificationService service, IAuthorizationService authorizationService) : base(service, authorizationService)
        {
            validationService = new ValidationService(this);
        }

        [ViewData]
        public Dictionary<string, NotifyError> NotifyErrorDictionary { get; set; }

        public virtual async Task<IActionResult> OnGetAsync(int notificationId, bool isBeingSubmitted = false)
        {
            Notification = await GetNotification(notificationId);

            if (Notification == null)
            {
                return NotFound();
            }

            await AuthorizeAndSetBannerAsync();
            if (!HasEditPermission)
            {
                return RedirectToOverview(notificationId);
            }

            return await PreparePageForGet(notificationId, isBeingSubmitted);
        }

        protected virtual async Task<Notification> GetNotification(int notificationId)
        {
            return await service.GetNotificationAsync(notificationId);
        }

        /*
        Post method accepts name of action specified by button clicked.
        Using handler adds handler onto url and therefore breaking javascript
        validation hapening after form is submitted
        */
        public async Task<IActionResult> OnPostAsync(string actionName, bool isBeingSubmitted)
        {
            // Get Notifications with all owned properties to check for 
            Notification = await service.GetNotificationWithAllInfoAsync(NotificationId); 
            if (Notification == null)
            {
                return NotFound();
            }
            else if (!(await authorizationService.CanEdit(User, Notification)))
            {
                return ForbiddenResult();
            }

            NotificationBannerModel = new NotificationBannerModel(Notification);

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
            // First Validate and Save current page details
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

            await service.SubmitNotification(Notification);
            
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
            NotificationId = Notification.NotificationId;
            Notification.SetFullValidation(Notification.NotificationStatus, isBeingSubmitted);
            ownedModel.ShouldValidateFull = Notification.ShouldValidateFull;

            await GetLinkedNotifications();
        }

        public bool IsValid(string key)
        {
            return validationService.IsValid(key);
        }

        protected abstract Task ValidateAndSave();
        protected abstract Task<IActionResult> PreparePageForGet(int notificationId, bool isBeingSubmitted);
        protected abstract IActionResult RedirectToNextPage(int? notificationId, bool isBeingSubmitted);
    }
}
