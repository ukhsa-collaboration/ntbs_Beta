using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    public class DraftEditPageTests : TestRunnerNotificationBase
    {
        public DraftEditPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public static IEnumerable<Notification> GetSeedingNotifications()
        {
            return new List<Notification>()
            {
                new Notification
                {
                    NotificationId = Utilities.DRAFT_NOTIFICATION_WITH_DRAFT_ALERT,
                    NotificationStatus = NotificationStatus.Draft
                }
            };
        }

        public static IEnumerable<Alert> GetSeedingAlerts()
        {
            return new List<Alert>
            {
                new DataQualityDraftAlert
                {
                    AlertId = Utilities.DRAFT_DATA_QUALITY_ALERT,
                    NotificationId = Utilities.DRAFT_NOTIFICATION_WITH_DRAFT_ALERT
                }
            };
        }

        [Fact]
        public async Task Get_ReturnsEditPageWithAlert_IfDraftHasDraftAlert()
        {
            // Arrange
            const int id = Utilities.DRAFT_NOTIFICATION_WITH_DRAFT_ALERT;
            var patientEditPageUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.EditPatientDetails);

            // Act
            var patientEditPage = await GetDocumentForUrlAsync(patientEditPageUrl);

            // Assert
            Assert.NotNull(patientEditPage.GetElementById("draft-alert-details"));
        }

        [Fact]
        public async Task Get_ReturnsRedirectToOverview_ForReadOnlyUser()
        {
            // Arrange
            const int id = Utilities.DRAFT_NOTIFICATION_WITH_DRAFT_ALERT;
            using (var client = Factory.WithUserAuth(TestUser.ReadOnlyUser)
                .CreateClientWithoutRedirects())
            {
                // Act
                var response = await client.GetAsync(RouteHelper.GetNotificationPath(id, NotificationSubPaths.EditPatientDetails));

                // Assert
                response.AssertRedirectTo($"/Notifications/{id}");
            }
        }

        [Fact]
        public async Task LastEditPage_HasNoSaveButton_ForDraftNonMDRRecord()
        {
            // Arrange
            const int id = Utilities.DRAFT_NOTIFICATION_WITH_DRAFT_ALERT;
            var lastEditPageUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.EditTreatmentEvents);

            // Act
            var lastEditPage = await GetDocumentForUrlAsync(lastEditPageUrl);

            // Assert
            Assert.Null(lastEditPage.GetElementById("save-button"));
        }
        
        [Fact]
        public async Task EditTreatmentEventsPage_HasContinueButton_WhenDraftRecordIsMDR()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var lastEditPageUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.EditTreatmentEvents);

            // Act
            var lastEditPage = await GetDocumentForUrlAsync(lastEditPageUrl);
            var button = lastEditPage.GetElementById("save-button");
            
            // Assert
            Assert.NotNull(button);
            Assert.Equal("Continue", button.TextContent.Trim());
        }
        
        [Fact]
        public async Task LastEditPage_HasSaveButton_WithNoContinue_WhenDraftRecordIsMDR()
        {
            // Arrange
            const int id = Utilities.DRAFT_ID;
            var lastEditPageUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.EditMDRDetails);

            // Act
            var lastEditPage = await GetDocumentForUrlAsync(lastEditPageUrl);
            var button = lastEditPage.GetElementById("save-button");
            
            // Assert
            Assert.NotNull(button);
            Assert.Equal("Save", button.TextContent.Trim());
        }
    }
}
