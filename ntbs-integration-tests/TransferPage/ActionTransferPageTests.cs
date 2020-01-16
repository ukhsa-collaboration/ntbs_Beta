using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.TransferPage
{
    public class ActionTransferPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.ActionTransferRequest;
        public ActionTransferPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task DeclineTransferAlert_ReturnsPageWithModelErrors_IfReasonNotValid()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            var formData = new Dictionary<string, string>
            {
                ["AcceptTransfer"] = "false",
                ["DeclineTransferReason"] = "|||",
            };

            // Act
            var result = await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("comment", "Explanatory comment can only contain letters, numbers and the symbols ' - . , /");
        }

        [Fact]
        public async Task ActionTransferAlertPage_ReturnsPageWithModelErrors_IfNoChoiceMade()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            // Act
            var result = await SendPostFormWithData(initialDocument, null, url);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("action", "Please accept or decline the transfer");
        }

        [Fact]
        public async Task AcceptTransferAlert_SuccessfullyChangesTbServiceOfNotificationAndDismissesAlert()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID_WITH_NOTIFICATION_DATE;
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            Assert.Equal("  ", initialDocument.QuerySelector("#banner-tb-service").TextContent);
            Assert.Equal("  ", initialDocument.QuerySelector("#banner-case-manager").TextContent);

            var formData = new Dictionary<string, string>
            {
                ["AcceptTransfer"] = "true"
            };

            // Act
            await SendPostFormWithData(initialDocument, formData, url);

            // Assert
            var overviewUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.Overview);
            var overviewPage = await GetDocumentForUrl(overviewUrl);
            Assert.Contains("Abingdon Community Hospital", overviewPage.QuerySelector("#banner-tb-service").TextContent);
            Assert.Contains("TestCase TestManager", overviewPage.QuerySelector("#banner-case-manager").TextContent);
            Assert.Null(overviewPage.QuerySelector("#alert-3"));
        }
    }
}