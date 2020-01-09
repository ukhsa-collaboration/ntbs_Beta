using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.TransferPage
{
    public class TransferPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.TransferRequest;
        public TransferPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public static IList<Alert> GetSeedingAlerts()
        {
            return new List<Alert>
            {
                new TransferAlert 
                {
                    AlertType = AlertType.TransferRequest,
                    AlertId = Utilities.TRANSFER_ALERT_ID,
                    NotificationId = Utilities.NOTIFIED_ID,
                    TbServiceCode = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                    CaseManagerEmail = Utilities.CASEMANAGER_ABINGDON_EMAIL,
                    AlertStatus = AlertStatus.Open
                }
            };
        }

        [Fact]
        public async Task CreateTransferAlert_ReturnsPageWithModelErrors_IfAlertNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID_WITH_NOTIFICATION_DATE;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["TransferAlert.NotificationId"] = id.ToString(),
                ["TransferAlert.TbServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["TransferAlert.TransferReason"] = nameof(TransferReason.Relocation),
                ["TransferAlert.OtherReasonDescription"] = "|||",
                ["TransferAlert.TransferRequestNote"] = "|||"
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url, "Confirm");

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("description", "Other description can only contain letters, numbers and the symbols ' - . , /");
            resultDocument.AssertErrorMessage("optional-note", "Optional note can only contain letters, numbers and the symbols ' - . , /");
        }

        [Fact]
        public async Task CreateTransferAlert_RedirectsToOverviewPage_IfAlertValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID_WITH_NOTIFICATION_DATE;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["TransferAlert.NotificationId"] = id.ToString(),
                ["TransferAlert.TbServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["TransferAlert.TransferReason"] = nameof(TransferReason.Relocation)
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url, "Confirm");

            // Assert
            result.AssertRedirectTo("/Notifications/4");
        }

        [Fact]
        public async Task NavigatingToPendingTransfer_ReturnsReadOnlyPartial_WhenTransferAlertAlreadyExists()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            Assert.NotNull(initialDocument.QuerySelector("#cancel-transfer-button"));
        }
    }
}