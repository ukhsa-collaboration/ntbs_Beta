using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class TestResultsModel : NotificationEditModelBase
    {
        public TestResultsModel(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository) : base(service, authorizationService,
            notificationRepository)
        {
        }

        [BindProperty]
        public TestData TestData { get; set; }

        protected override async Task<IActionResult> PreparePageForGet(int id, bool isBeingSubmitted)
        {
            TestData = Notification.TestData;
            await SetNotificationProperties(isBeingSubmitted, TestData);

            if (TestData.ShouldValidateFull)
            {
                TryValidateModel(this);
            }

            return Page();
        }

        protected override IActionResult RedirectToNextPage(int notificationId, bool isBeingSubmitted)
        {
            return RedirectToPage("./ContactTracing", new { id = notificationId, isBeingSubmitted });
        }

        protected override IActionResult RedirectToCreate(int notificationId)
        {
            return RedirectToPage("./Table/ManualTestResult", new { notificationId = notificationId });
        }

        protected override async Task ValidateAndSave()
        {
            TestData.SetFullValidation(Notification.NotificationStatus);
            if (TryValidateModel(TestData, nameof(TestData)))
            {
                await Service.UpdateTestDataAsync(Notification, TestData);
            }
        }

        public ContentResult OnGetValidateTestDataProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.ValidateModelProperty<TestData>(key, value, shouldValidateFull);
        }
    }
}
