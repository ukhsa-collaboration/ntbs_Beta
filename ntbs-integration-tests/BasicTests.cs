using System.Net;
using System.Threading.Tasks;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests
{
    public class BasicTests : TestRunnerBase
    {
        public BasicTests(NtbsWebApplicationFactory<Startup> factory) : base(factory) {}

        [Theory]
        [InlineData(Routes.HomePage)]
        [InlineData(Routes.SearchPage)]
        public async Task TestPagesExist(string route)
        {
            // Act
            var response = await Client.GetAsync($"{route}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
