using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ntbs_service.DataAccess;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.FilteredSelectLists;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;

namespace ntbs_service.Pages.Notifications.Edit.Items
{
    public class ManualTestResultPage : NotificationEditModelBase
    {
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IItemRepository<ManualTestResult> _testResultsRepository;

        public SelectList ManualTestTypes { get; set; }
        public SelectList SampleTypes { get; set; }

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
            IItemRepository<ManualTestResult> testResultsRepository)
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
            TestResultForEdit.SetValidationContext(Notification);
            TestResultForEdit.NotificationId = NotificationId;
            TestResultForEdit.Dob = Notification.PatientDetails.Dob;
            await SetRelatedEntitiesAsync();
            SetDate();

            if (TryValidateModel(TestResultForEdit, "TestResultForEdit"))
            {
                if (RowId == null)
                {
                    await _testResultsRepository.AddAsync(TestResultForEdit);
                }
                else
                {
                    TestResultForEdit.ManualTestResultId = RowId.Value;
                    await _testResultsRepository.UpdateAsync(Notification, TestResultForEdit);
                }
            }
        }

        public async Task<IActionResult> OnPostDeleteAsync()
        {
            Notification = await GetNotificationAsync(NotificationId);
            if (await AuthorizationService.GetPermissionLevelForNotificationAsync(User, Notification) != PermissionLevel.Edit)
            {
                return ForbiddenResult();
            }

            var testResult = Notification.TestData.ManualTestResults
                    .SingleOrDefault(r => r.ManualTestResultId == RowId.Value);
            if (testResult == null)
            {
                return NotFound();
            }

            await _testResultsRepository.DeleteAsync(testResult);

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

        private async Task SetRelatedEntitiesAsync()
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

        public async Task<ContentResult> OnGetValidateTestResultForEditDateAsync(string key, string day, string month, string year)
        {
            var notification = await NotificationRepository.GetNotificationAsync(NotificationId);
            return ValidationService.GetDateValidationResult<ManualTestResult>(key, day, month, year, notification.IsLegacy);
        }

        public async Task<JsonResult> OnGetFilteredSampleTypesForManualTestType(int value)
        {
            var filteredSampleTypes = await _referenceDataRepository.GetSampleTypesForManualTestType(value);

            return new JsonResult(
                new FilteredManualTestPageSelectLists
                {
                    SampleTypes = filteredSampleTypes.Select(n => new OptionValue
                    {
                        Value = n.SampleTypeId.ToString(),
                        Text = n.Description,
                        Group = n.Category
                    })
                });
        }
    }
}
