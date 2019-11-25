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

        public static void AssertValidationErrorResponse(this HttpResponseMessage response)
        {
            // When there's an input validation error, we  respond to the POST
            // request with an OK status code the html page including errors
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }
    }
}
