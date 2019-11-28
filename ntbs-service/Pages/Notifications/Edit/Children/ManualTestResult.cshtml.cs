using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit
{
    public class ManualTestResultPage : NotificationEditModelBase
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly ITestResultsRepository _testResultsRepository;

        public SelectList ManualTestTypes { get; set; }
        public SelectList SampleTypes { get; set; }
        public SelectList ResultOptions { get; set; }

        [BindProperty(SupportsGet = true)]
        public int? RowId { get; set; }
        [BindProperty]
        public ManualTestResult TestResultForEdit { get; set; }
        [BindProperty]
        public FormattedDate FormattedTestDate { get; set; }

        public ManualTestResultPage(
            INotificationService notificationService,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            ITestResultsRepository testResultsRepository)
            : base(notificationService, authorizationService, notificationRepository)
        {
            _referenceDataRepository = referenceDataRepository;
            _testResultsRepository = testResultsRepository;
        }

        protected override async Task<IActionResult> PrepareAndDisplayPageAsync(bool isBeingSubmitted)
        {
            if (RowId != null)
            {
                TestResultForEdit = Notification.TestData.ManualTestResults
                    .SingleOrDefault(r => r.ManualTestResultId == RowId.Value);
                if (TestResultForEdit == null)
                {
                    return NotFound();
                }
                FormattedTestDate = TestResultForEdit.TestDate.ConvertToFormattedDate();
            }

            await SetDropdownsAsync();

            return Page();
        }

        protected override async Task ValidateAndSave()
        {
            TestResultForEdit.NotificationId = NotificationId;
            TestResultForEdit.Dob = Notification.PatientDetails.Dob;
            await SetRelatedEntities();
            SetDate();

            if (TryValidateModel(TestResultForEdit, "TestResultForEdit"))
            {
                if (RowId == null)
                {
                    await _testResultsRepository.AddTestResultAsync(TestResultForEdit);
                }
                else
                {
                    TestResultForEdit.ManualTestResultId = RowId.Value;
                    await _testResultsRepository.UpdateTestResultAsync(Notification, TestResultForEdit);
                }
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            Notification = await GetNotificationAsync(NotificationId);
            if (!(await AuthorizationService.CanEdit(User, Notification)))
            {
                return ForbiddenResult();
            }

            var testResult = Notification.TestData.ManualTestResults
                    .SingleOrDefault(r => r.ManualTestResultId == RowId.Value);
            if (testResult == null)
            {
                return NotFound();
            }

            await _testResultsRepository.DeleteTestAsync(testResult);

            return RedirectToPage("/Notifications/Edit/TestResults", new { NotificationId });
        }

        private async Task SetDropdownsAsync()
        {
            var manualTestTypes = await _referenceDataRepository.GetManualTestTypesAsync();
            ManualTestTypes = new SelectList(manualTestTypes, nameof(ManualTestType.ManualTestTypeId), nameof(ManualTestType.Description));

            var sampleTypes = await _referenceDataRepository.GetSampleTypesAsync();
            SampleTypes = new SelectList(
                items: sampleTypes,
                dataValueField: nameof(SampleType.SampleTypeId),
                dataTextField: nameof(SampleType.Description),
                selectedValue: null,
                dataGroupField: nameof(SampleType.Category));

            ResultOptions = new SelectList(ResultEnumHelper.GetAll());
        }

        protected override IActionResult RedirectAfterSaveForNotified()
        {
            return RedirectToPage("/Notifications/Edit/TestResults", new { NotificationId });
        }

        protected override IActionResult RedirectAfterSaveForDraft(bool isBeingSubmitted)
        {
            return RedirectToPage("/Notifications/Edit/TestResults", new { NotificationId, isBeingSubmitted });
        }

        protected override async Task<Notification> GetNotificationAsync(int notificationId)
        {
            return await NotificationRepository.GetNotificationWithTestsAsync(notificationId);
        }

        private async Task SetRelatedEntities()
        {
            if (TestResultForEdit.ManualTestTypeId != null)
            {
                TestResultForEdit.ManualTestType = await _referenceDataRepository.GetManualTestTypeAsync(TestResultForEdit.ManualTestTypeId.Value);
            }
            if (TestResultForEdit.SampleTypeId != null)
            {
                TestResultForEdit.SampleType = await _referenceDataRepository.GetSampleTypeAsync(TestResultForEdit.SampleTypeId.Value);
            }
        }

        private void SetDate()
        {
            // The required date will be marked as missing on the model, since we are setting it manually, rather than binding it
            ModelState.Remove("TestResultForEdit.TestDate");
            ValidationService.TrySetFormattedDate(
                TestResultForEdit,
                "TestResultForEdit",
                nameof(ManualTestResult.TestDate),
                FormattedTestDate);
        }
    }
}
