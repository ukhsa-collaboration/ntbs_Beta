using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using Xunit;

namespace ntbs_integration_tests.TransferPages
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
                ["TransferRequest.TbServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["TransferRequest.TransferReason"] = nameof(TransferReason.Relocation),
                ["TransferRequest.OtherReasonDescription"] = "|||",
                ["TransferRequest.TransferRequestNote"] = "|||"
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
            const int id = Utilities.NOTIFIED_ID_2;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            var formData = new Dictionary<string, string>
            {
                ["TransferRequest.TbServiceCode"] = Utilities.TBSERVICE_ABINGDON_COMMUNITY_HOSPITAL_ID,
                ["TransferRequest.TransferReason"] = nameof(TransferReason.Relocation)
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
            const int id = Utilities.NOTIFIED_ID_2;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);

            Assert.NotNull(initialDocument.QuerySelector("#cancel-transfer-button"));
        }

        [Fact]
        public async Task TransferPageDisplaysServiceDirectoryLink()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_TBSERVICE;
            var url = GetCurrentPathForId(id);
            var document = await GetDocumentForUrlAsync(url);
            var directoryLinkText = document.QuerySelectorAll("h3")
                .FirstOrDefault(e => e.InnerHtml.Contains("You can search for TB services and case managers"));
            
            // Assert
            Assert.NotNull(directoryLinkText);
        }

        [Fact]
        public async Task NTBSServiceDirectoryLink_LinksToServiceDirectoryPage()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_WITH_TBSERVICE;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrlAsync(url);
            var directoryLink = initialDocument.QuerySelectorAll("h3 > a")
                .SingleOrDefault(
                    e => e.Attributes.GetNamedItem("href")?.Value == "/ServiceDirectory" 
                         && e.InnerHtml.Contains("NTBS service directory"));
            
            //Assert
            Assert.NotNull(directoryLink);
        }
    }
}
