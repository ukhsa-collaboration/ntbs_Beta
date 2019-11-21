using System.Net;
using System.Net.Http;
using Xunit;

namespace ntbs_integration_tests.Helpers
{
    public static class TestResponseExtensions
    {
        public static void AssertRedirectTo(this HttpResponseMessage response, string expectedUrl)
        {
            Assert.Equal(HttpStatusCode.Redirect, response.StatusCode);

            var redirectUrl = response.Headers.Location.OriginalString;
            Assert.Contains(expectedUrl, redirectUrl);
        }
    }
}
