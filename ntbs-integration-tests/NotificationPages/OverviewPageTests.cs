using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Helpers;
using ntbs_service.Pages;
using Xunit;

namespace ntbs_integration_tests.NotificationPages
{
    // TODO: Complete tests for this page
    public class OverviewPageTests : TestRunnerNotificationBase
    {
        protected override string NotificationSubPath => NotificationSubPaths.Overview;

        public OverviewPageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public static IEnumerable<object[]> OverviewRoutes()
        {
            yield return new object[] { Utilities.DRAFT_ID, HttpStatusCode.Redirect };
            yield return new object[] { Utilities.NOTIFIED_ID, HttpStatusCode.OK };
            yield return new object[] { Utilities.DENOTIFIED_ID, HttpStatusCode.OK };
            yield return new object[] { Utilities.NEW_ID, HttpStatusCode.NotFound };
        }

        [Theory, MemberData(nameof(OverviewRoutes))]
        public async Task GetOverviewPage_ReturnsCorrectStatusCode_DependentOnId(int id, HttpStatusCode code)
        {
            // Act
            var response = await Client.GetAsync(GetCurrentPathForId(id));

            // Assert
            Assert.Equal(code, response.StatusCode);

            if (response.StatusCode == HttpStatusCode.Redirect)
            {
                Assert.Contains(GetPathForId(NotificationSubPaths.EditPatientDetails, id), GetRedirectLocation(response));
            }
        }

        [Fact]
        public async Task Get_ReturnsOverviewPage_ForUserWithPermission()
        {
            // Arrange
            using (var client = Factory.WithMockUserService<NhsUserService>()
                                        .WithNotificationAndTbServiceConnected(Utilities.NOTIFIED_ID, Utilities.PERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {

                //Act
                var response = await client.GetAsync(GetCurrentPathForId(Utilities.NOTIFIED_ID));

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var document = await GetDocumentAsync(response);
                Assert.True(document.GetElementsByClassName("notification-overview-type-and-edit-container").Length > 0);
            }
        }

        [Fact]
        public async Task Get_ShowsWarning_ForUserWithoutPermission()
        {
            // Arrange
            using (var client = Factory.WithMockUserService<NhsUserService>()
                                        .WithNotificationAndTbServiceConnected(Utilities.NOTIFIED_ID, Utilities.UNPERMITTED_SERVICE_CODE)
                                        .CreateClientWithoutRedirects())
            {

                //Act
                var response = await client.GetAsync(GetCurrentPathForId(Utilities.NOTIFIED_ID));

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var document = await GetDocumentAsync(response);
                Assert.Contains(Messages.UnauthorizedWarning, document.GetElementById("unauthorized-warning").TextContent);
            }
        }

        [Fact]
        public async Task DismissAlert_CorrectlyDismissesAlertAndReturnsOverviewPage()
        {
            // Arrange
            var url = GetCurrentPathForId(Utilities.NOTIFIED_ID);
            var document = await GetDocumentForUrl(url);
            var dismissPageRoute = "/Alerts/1/Dismiss?Page=Overview";
            Assert.NotNull(document.QuerySelector("#alert-1"));

            // Act
            var result = await SendPostFormWithData(document, null, dismissPageRoute);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
            Assert.Contains(GetRedirectLocation(result), url);
            var reloadedDocument = await GetDocumentForUrl(GetRedirectLocation(result));
            Assert.Null(reloadedDocument.QuerySelector("#alert-1"));
        }
    }
}
