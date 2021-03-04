using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests.HomePage
{
    public class HomePageTests : TestRunnerBase
    {
        public HomePageTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public const string PageRoute = "/Index";
        public const string DismissPageRoute = "/Alerts/20001/Dismiss";

        [Fact]
        public async Task DismissAlert_CorrectlyDismissesAlertAndReturnsHomePage()
        {
            using (var client = Factory.WithUserAuth(TestUser.NhsUserForAbingdonAndPermitted)
                .WithNotificationAndTbServiceConnected(Utilities.NOTIFIED_ID, Utilities.PERMITTED_SERVICE_CODE)
                .CreateClientWithoutRedirects())
            {

                // Arrange
                var initialPage = await client.GetAsync(PageRoute);
                var pageContent = await GetDocumentAsync(initialPage);
                Assert.NotNull(pageContent.QuerySelector("#alert-20001"));

                // Act
                var result = await client.SendPostFormWithData(pageContent, null, DismissPageRoute);

                // Assert
                var reloadedPage = await client.GetAsync(PageRoute);
                var reloadedPageContent = await GetDocumentAsync(reloadedPage);

                Assert.Null(reloadedPageContent.QuerySelector("#alert-20001"));
            }
        }

        [Fact]
        public async Task ShowingHomepageKpis_WhenUserIsNationalUser()
        {
            // Arrange
            var initialPage = await Client.GetAsync(PageRoute);
            var pageContent = await GetDocumentAsync(initialPage);

            // Assert
            Assert.NotNull(pageContent.QuerySelector("#homepage-kpi-details"));
        }

        [Fact]
        public async Task ShowingHomepageKpis_WhenUserIsNhsUser()
        {
            using (var client = Factory.WithUserAuth(TestUser.NhsUserForAbingdonAndPermitted)
                                        .CreateClientWithoutRedirects())
            {
                // Arrange
                var initialPage = await client.GetAsync(PageRoute);
                var pageContent = await GetDocumentAsync(initialPage);

                // Assert
                Assert.NotNull(pageContent.QuerySelector("#homepage-kpi-details"));
            }
        }

        [Fact]
        public async Task ShowingHomepageKpis_WhenUserIsPheUser()
        {
            using (var client = Factory.WithUserAuth(TestUser.PheUserWithPermittedPhecCode)
                                        .CreateClientWithoutRedirects())
            {
                // Arrange
                var initialPage = await client.GetAsync(PageRoute);
                var pageContent = await GetDocumentAsync(initialPage);

                // Assert
                Assert.NotNull(pageContent.QuerySelector("#homepage-kpi-details"));
            }
        }
    }
}
