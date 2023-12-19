using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Entities.Alerts;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.TransferPages
{
    public class RejectTransferPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.TransferDeclined;
        public RejectTransferPageTests(NtbsWebApplicationFactory<EntryPoint> factory) : base(factory) { }
        public static IList<Notification> GetSeedingNotifications()
        {
            return new List<Notification>
            {
                new Notification
                {
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_CURLY_BRACKETS_IN_ALERT,
                    NotificationStatus = NotificationStatus.Notified,
                }
            };
        }
        public static IList<Alert> GetSeedingAlerts()
        {
            return new List<Alert>
            {
                new TransferRejectedAlert
                {
                    AlertId = Utilities.TRANSFER_ALERT_DECLINED,
                    AlertStatus = AlertStatus.Open,
                    AlertType = AlertType.TransferRejected,
                    CreationDate = DateTime.Now,
                    RejectionReason = "{{abc}}",
                    NotificationId = Utilities.NOTIFICATION_ID_WITH_CURLY_BRACKETS_IN_ALERT
                }
            };
        }

        [Fact]
        public async Task DismissRejectedTransferAlert_DismissesAlertOnOverviewPage()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var overviewUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.Overview);
            var initialOverviewPage = await GetDocumentForUrlAsync(overviewUrl);
            Assert.NotNull(initialOverviewPage.QuerySelector("#alert-20005"));

            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            // Act
            await Client.SendPostFormWithData(initialDocument, null, url);

            // Assert
            var resultOverviewPage = await GetDocumentForUrlAsync(overviewUrl);
            Assert.Null(resultOverviewPage.QuerySelector("#alert-20005"));
        }
        
        [Fact]
        public async Task GetTransferDeclined_RemovesCurlyBrackets()
        {
            // Arrange
            const int id = Utilities.NOTIFICATION_ID_WITH_CURLY_BRACKETS_IN_ALERT;
            var url = GetCurrentPathForId(id);

            // Act
            var document = await GetDocumentForUrlAsync(url);

            // Assert
            var rejectionNote = document.GetElementsByClassName("rejection-note").Single().TextContent;

            Assert.DoesNotContain("{", rejectionNote);
            Assert.DoesNotContain("}", rejectionNote);
            Assert.Contains("abc", rejectionNote);
        }
    }
}
