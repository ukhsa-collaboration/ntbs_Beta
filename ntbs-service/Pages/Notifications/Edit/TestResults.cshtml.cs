using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
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
            IAlertRepository alertRepository,
            ICultureAndResistanceService cultureAndResistanceService,
            ISpecimenService specimenService) : base(notificationService, authorizationService, notificationRepository, alertRepository)
        {
            _cultureAndResistanceService = cultureAndResistanceService;
            _specimenService = specimenService;
            
            CurrentPage = NotificationSubPaths.EditTestResults;
        }

        [BindProperty]
        public TestData TestData { get; set; }
        public CultureAndResistance CultureAndResistance { get; set; }
        public IEnumerable<MatchedSpecimen> Specimens { get; set; }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            TestData = Notification.TestData;
            CultureAndResistance = await _cultureAndResistanceService.GetCultureAndResistanceDetailsAsync(NotificationId);
            Specimens = await _specimenService.GetMatchedSpecimenDetailsForNotificationAsync(NotificationId);

            await SetNotificationProperties(isBeingSubmitted, TestData);

            if (TestData.ShouldValidateFull)
            {
                TryValidateModel(this);
            }

            return Page();
        }

        protected override IActionResult RedirectForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("./ContactTracing", new { NotificationId, isBeingSubmitted });
        }

        protected override IActionResult RedirectToCreate()
        {
            return RedirectToPage("./Items/NewManualTestResult", new { NotificationId });
        }

        protected override async Task ValidateAndSave()
        {
            if (ActionName == ActionNameString.Create)
            {
                TestData.HasTestCarriedOut = true;
            }
            // Set the collection so it can be included in the validation
            TestData.ManualTestResults = Notification.TestData.ManualTestResults;
            TestData.ProceedingToAdd = ActionName == ActionNameString.Create;
            TestData.SetValidationContext(Notification);
            if (TryValidateModel(TestData, nameof(TestData)))
            {
                await Service.UpdateTestDataAsync(Notification, TestData);
            }
        }

        public async Task<IActionResult> OnPostUnmatch(string labReferenceNumber)
        {
            var userName = User.Username();
            await _specimenService.UnmatchSpecimenAsync(NotificationId, labReferenceNumber, userName);
            return RedirectToPage("/Notifications/Edit/TestResults", new { NotificationId });
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
