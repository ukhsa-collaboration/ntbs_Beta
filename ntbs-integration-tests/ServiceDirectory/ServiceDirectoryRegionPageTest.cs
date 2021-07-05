using System.Linq;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests.ServiceDirectory
{
    public class ServiceDirectoryRegionPageTest : TestRunnerBase
    {
        public ServiceDirectoryRegionPageTest(NtbsWebApplicationFactory<Startup> factory) : base(factory) { }

        public const string PageRoute = "ServiceDirectory/Region/E45000009";

        [Fact]
        public async Task GetRegionPage_IncludesOnlyActiveUsers()
        {
            // Arrange
            var initialPage = await Client.GetAsync(PageRoute);
            var pageContent = await GetDocumentAsync(initialPage);

            // Act
            var gatesheadSection = pageContent.GetElementsByClassName("govuk-accordion__section")
                .Single(elem => elem.TextContent.Contains("Gateshead"));

            // Assert
            Assert.Contains(Utilities.CASEMANAGER_GATESHEAD_DISPLAY_NAME1, gatesheadSection.TextContent);
            Assert.Contains(Utilities.CASEMANAGER_GATESHEAD_DISPLAY_NAME2, gatesheadSection.TextContent);
            Assert.DoesNotContain(Utilities.CASEMANAGER_GATESHEAD_INACTIVE_DISPLAY_NAME, gatesheadSection.TextContent);
        }
    }
}
