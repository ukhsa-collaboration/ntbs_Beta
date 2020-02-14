using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.TransferPage
{
    public class TransferPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.TransferRequest;
        public TransferPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task CreateTransferAlert_ReturnsPageWithModelErrors_IfAlertNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_TBSERVICE;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["TransferAlert.NotificationId"] = id.ToString(),
                ["TransferAlert.TbServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["TransferAlert.TransferReason"] = nameof(TransferReason.Relocation),
                ["TransferAlert.OtherReasonDescription"] = "|||",
                ["TransferAlert.TransferRequestNote"] = "|||"
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url, "Confirm");

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("description", "Other description can only contain letters, numbers and the symbols ' - . , /");
            resultDocument.AssertErrorMessage("optional-note", "Optional note can only contain letters, numbers and the symbols ' - . , /");
        }

        [Fact]
        public async Task CreateTransferAlert_RedirectsToOverviewPage_IfAlertValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_TBSERVICE;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["TransferAlert.NotificationId"] = id.ToString(),
                ["TransferAlert.TbServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["TransferAlert.TransferReason"] = nameof(TransferReason.Relocation)
            };

            // Act
            var result = await Client.SendPostFormWithData(initialDocument, formData, url, "Confirm");

            // Assert
            result.AssertRedirectTo($"/Notifications/{id}");
        }

        [Fact]
        public async Task NavigatingToPendingTransfer_ReturnsReadOnlyPartial_WhenTransferAlertAlreadyExists()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            Assert.NotNull(initialDocument.QuerySelector("#cancel-transfer-button"));
        }
    }
}
