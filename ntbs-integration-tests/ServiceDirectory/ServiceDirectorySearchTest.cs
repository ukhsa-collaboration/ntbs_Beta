using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests.ServiceDirectory
{
    public class ServiceDirectorySearchTest : TestRunnerBase
    {
        public ServiceDirectorySearchTest(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public const string PageRoute = "ServiceDirectory";

        [Fact]
        public async Task GetSearch_ReturnsPageWithModelErrors_IfSearchNotValid()
        {
            // Arrange
            var initialPage = await Client.GetAsync(PageRoute);
            var pageContent = await GetDocumentAsync(initialPage);

            var formData = new Dictionary<string, string>
            {
                ["SearchKeyword"] = "!!Test User#",
            };

            // Act
            var result = await SendGetFormWithData(pageContent, formData, PageRoute);

            // Assert
            var resultDocument = await GetDocumentAsync(result);
            resultDocument.AssertErrorMessage("search-keyword", "Search keyword can only contain letters and the symbols ' - . ,");
        }


        [Fact]
        public async Task GetSearch_RedirectToSearchResults_IfSearchValid()
        {
            // Arrange
            var initialPage = await Client.GetAsync(PageRoute);
            var pageContent = await GetDocumentAsync(initialPage);
            var formData = new Dictionary<string, string>
            {
                ["SearchKeyword"] = "TestUser"
            };

            // Act
            var result = await SendGetFormWithData(pageContent, formData, PageRoute);

            // Assert
            Assert.Equal(HttpStatusCode.Redirect, result.StatusCode);
        }
    }
}
