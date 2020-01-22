using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.TransferPage
{
    public class RejectTransferPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.TransferDeclined;
        public RejectTransferPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        [Fact]
        public async Task DismissRejectedTransferAlert_DismissesAlertOnOverviewPage()
        {
            // Arrange
            const int id = Utilities.NOTIFIED_ID;
            var overviewUrl = RouteHelper.GetNotificationPath(id, NotificationSubPaths.Overview);
            var initialOverviewPage = await GetDocumentForUrl(overviewUrl);
            Assert.NotNull(initialOverviewPage.QuerySelector("#alert-20005"));
            var url = GetCurrentPathForId(id);
            var initialDocument = await GetDocumentForUrl(url);

            // Act
            await SendPostFormWithData(initialDocument, null, url);

            // Assert
            var resultOverviewPage = await GetDocumentForUrl(overviewUrl);
            Assert.Null(resultOverviewPage.QuerySelector("#alert-20005"));
        }
    }
}
