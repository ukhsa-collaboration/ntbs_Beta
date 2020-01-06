using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class TestResultsModel : NotificationEditModelBase
    {
        private readonly ICultureAndResistanceService _cultureAndResistanceService;
        private readonly ISpecimenService _specimenService;

        public TestResultsModel(
            INotificationService notificationService,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            ICultureAndResistanceService cultureAndResistanceService,
            ISpecimenService specimenService)
            : base(notificationService,
                   authorizationService,
                   notificationRepository)
        {
            _cultureAndResistanceService = cultureAndResistanceService;
            _specimenService = specimenService;
        }

        [BindProperty]
        public TestData TestData { get; set; }

        [BindProperty]
        public CultureAndResistance CultureAndResistance { get; set; }

        [BindProperty]
        public IEnumerable<Specimen> Specimens { get; set; }
        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            TestData = Notification.TestData;
            CultureAndResistance = await _cultureAndResistanceService.GetCultureAndResistanceDetailsAsync(NotificationId);
            Specimens = await _specimenService.GetSpecimenDetailsAsync(NotificationId);

            await SetNotificationProperties(isBeingSubmitted, TestData);

            if (TestData.ShouldValidateFull)
            {
                TryValidateModel(this);
            }

            return Page();
        }

        protected override IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./ContactTracing", new { NotificationId, isBeingSubmitted });
        }

        protected override IActionResult RedirectToCreate()
        {
            return RedirectToPage("./Items/NewManualTestResult", new { NotificationId });
        }

        protected override async Task ValidateAndSave()
        {
            // Set the collection so it can be included in the validation
            TestData.ManualTestResults = Notification.TestData.ManualTestResults;
            TestData.ProceedingToAdd = ActionName == "Create";
            TestData.SetFullValidation(Notification.NotificationStatus);
            if (TryValidateModel(TestData, nameof(TestData)))
            {
                await Service.UpdateTestDataAsync(Notification, TestData);
            }
        }

        public ContentResult OnGetValidateTestDataProperty(string key, string value, bool shouldValidateFull)
        {
            return ValidationService.GetPropertyValidationResult<TestData>(key, value, shouldValidateFull);
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithTestsAsync(notificationId);
        }
    }
}
