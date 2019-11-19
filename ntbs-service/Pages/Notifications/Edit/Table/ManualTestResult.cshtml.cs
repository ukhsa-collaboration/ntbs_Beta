using System;
using System.Collections.Generic;
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
    public class ManualTestResult : NotificationModelBase
    {
        private readonly IReferenceDataRepository _referenceDataRepository;

        public IList<Models.ManualTestResult> AllTestResults { get; set; }
        public SelectList ManualTestTypes { get; set; }
        public SelectList SampleTypes { get; set; }
        public SelectList ResultOptions { get; set; }

        [BindProperty]
        public int? RowId { get; set; }
        [BindProperty]
        public Models.ManualTestResult TestResultForEdit { get; set; }
        [BindProperty]
        public FormattedDate FormattedTestDate { get; set; }

        public ManualTestResult(
            INotificationService service,
            IAuthorizationService authorizationService,
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository) : base(service, authorizationService, notificationRepository)
        {
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<IActionResult> OnGetAsync(int notificationId, int? rowId)
        {
            Notification = await GetNotification(notificationId);
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

            return await PreparePageForGet(NotificationId, rowId);
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Notification = await GetNotification(NotificationId);
            if (Notification == null)
            {
                return NotFound();
            }

            if (!(await AuthorizationService.CanEdit(User, Notification)))
            {
                return ForbiddenResult();
            }

            if (!FormattedTestDate.TryConvertToDateTime(out DateTime? testDate))
            {
                TestResultForEdit.TestDate = testDate.Value;
            }

            // Validate
            // Save/Delete
            // Redirect, maybe to same page depending on whether optional implementation of 'Save and add another is used'

            return RedirectToEditPage(NotificationId);
        }

        public async Task<IActionResult> PreparePageForGet(int notificationId, int? rowId)
        {
            AllTestResults = await NotificationRepository.GetManualTestResultsForNotificationAsync(notificationId);

            if (rowId != null)
            {
                TestResultForEdit = AllTestResults.SingleOrDefault(r => r.ManualTestResultId == rowId.Value);
                if (TestResultForEdit != null)
                {
                    FormattedTestDate = TestResultForEdit.TestDate.ConvertToFormattedDate();
                }
            }

            await SetDropdownsAsync();

            return Page();
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

        protected IActionResult RedirectToOverview(int notificationId)
        {
            return RedirectToPage("/Notifications/Overview", new { id = notificationId });
        }

        protected IActionResult RedirectToEditPage(int notificationId)
        {
            return RedirectToPage("/Notifications/Edit/TestResults", new { id = notificationId });
        }
    }
}
