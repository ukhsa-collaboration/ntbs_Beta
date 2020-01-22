using System.Collections.Generic;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using ntbs_service.Models.Validations;
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
            // Arrange
            var initialPage = await Client.GetAsync(PageRoute);
            var pageContent = await GetDocumentAsync(initialPage);
            Assert.NotNull(pageContent.QuerySelector("#alert-20001"));

            // Act
            var result = await SendPostFormWithData(pageContent, null, DismissPageRoute);

            // Assert
            var reloadedPage = await Client.GetAsync(PageRoute);
            var reloadedPageContent = await GetDocumentAsync(reloadedPage);

            Assert.Null(reloadedPageContent.QuerySelector("#alert-20001"));
        }
    }
}
