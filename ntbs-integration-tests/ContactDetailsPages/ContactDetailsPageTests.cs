using System.Linq;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_integration_tests.TestServices;
using ntbs_service;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.ContactDetailsPages
{
    public class ContactDetailsPageTests : TestRunnerBase
    {
        public ContactDetailsPageTests(NtbsWebApplicationFactory<Startup> factory)
            : base(factory) { }

        [Fact]
        public async Task CorrectBreadcrumbsForCaseManagerPresent()
        {
            var user = TestUser.AbingdonCaseManager;
            var pageRoute = (RouteHelper.GetContactDetailsSubPath(user.Id, null));
            using (var client = Factory.WithUserAuth(user).CreateClientWithoutRedirects())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(UserAuthentication.SchemeName);
                // Arrange
                var initialPage = await client.GetAsync(pageRoute);
                var pageContent = await GetDocumentAsync(initialPage);

                // Assert
                var breadcrumbs = pageContent.GetElementsByClassName("nhsuk-breadcrumb__list").Single().TextContent;
                Assert.Contains("Service Directory", breadcrumbs);
                Assert.Contains("South East", breadcrumbs);
                Assert.Contains("TestCase TestManager", breadcrumbs);
            }
        }

        [Fact]
        public async Task BreadcrumbForRegionPresentForRegionalUser()
        {
            var user = TestUser.PheUserWithPermittedPhecCode;
            var pageRoute = (RouteHelper.GetContactDetailsSubPath(user.Id, null));
            using (var client = Factory.WithUserAuth(user).CreateClientWithoutRedirects())
            {
                client.DefaultRequestHeaders.Authorization =
                    new AuthenticationHeaderValue(UserAuthentication.SchemeName);
                // Arrange
                var initialPage = await client.GetAsync(pageRoute);
                var pageContent = await GetDocumentAsync(initialPage);

                // Assert
                var breadcrumbs = pageContent.GetElementsByClassName("nhsuk-breadcrumb__list").Single().TextContent;
                Assert.Contains("Service Directory", breadcrumbs);
                Assert.Contains("South East", breadcrumbs);
                Assert.Contains("Permitted Phec", breadcrumbs);
            }
        }
    }
}

