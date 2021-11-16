using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Validations;
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
            ISpecimenService specimenService,
            IUserHelper userHelper)
            : base(notificationService, authorizationService, notificationRepository, alertRepository, userHelper)
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
                TryValidateTestData(TestData, nameof(TestData));
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
            if (TryValidateTestData(TestData, nameof(TestData)))
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

        public ContentResult OnPostValidateTestDataProperty([FromBody]InputValidationModel validationData)
        {
            return ValidationService.GetPropertyValidationResult<TestData>(validationData.Key, validationData.Value, validationData.ShouldValidateFull);
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithTestsAsync(notificationId);
        }

        private bool TryValidateTestData(TestData testData, string name)
        {
            TryValidateModel(testData, name);
            // The manual test results have already been validated/saved on the manual test result page. Here we need to include
            // them in the TestData so that the HasTestCarriedOut property can be validated correctly, but we don't care if there
            // are validation errors in the tests themselves. (It is possible to have errors on test results which have been
            // imported from ETS where the test type and sample type combination is invalid).
            ModelState.RemoveAll<TestResultsModel>(t => t.TestData.ManualTestResults);
            return ModelState.IsValid;
        }
    }
}
