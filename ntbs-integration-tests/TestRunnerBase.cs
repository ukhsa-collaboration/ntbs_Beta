using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using Xunit;

namespace ntbs_integration_tests
{
    public abstract class TestRunnerBase : IClassFixture<NtbsWebApplicationFactory<Startup>>
    {
        protected readonly HttpClient Client;
        protected readonly NtbsWebApplicationFactory<Startup> Factory;

        protected TestRunnerBase(NtbsWebApplicationFactory<Startup> factory)
        {
            Factory = factory;
            Factory.ConfigureTestClassName(GetType().Name);
            Factory.ConfigureLogger();
            Client = Factory.CreateClientWithoutRedirects();
        }

        protected async Task<IHtmlDocument> GetDocumentForUrlAsync(string url)
        {
            var initialPage = await Client.GetAsync(url);
            return await GetDocumentAsync(initialPage);
        }

        protected async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var document = await BrowsingContext.New().OpenAsync(req => req.Content(content));
            return (IHtmlDocument)document;
        }

        public async Task<HttpResponseMessage> AssertAndFollowRedirect(HttpResponseMessage response, string expectedUrl)
        {
            response.AssertRedirectTo(expectedUrl);
            return await Client.GetAsync(GetRedirectLocation(response));
        }

        protected string GetRedirectLocation(HttpResponseMessage response)
        {
            return response.Headers.Location.OriginalString;
        }

        protected string FullErrorMessage(string validationMessage)
        {
            return HtmlDocumentHelpers.FullErrorMessage(validationMessage);
        }

        protected async Task<HttpResponseMessage> SendGetFormWithData(
            IHtmlDocument document,
            Dictionary<string, string> formData,
            string pageRoute,
            string postRoute = null)
        {
            var form = (IHtmlFormElement)document.QuerySelector("form");

            var submissionRoute = pageRoute;
            if (!string.IsNullOrEmpty(postRoute))
            {
                submissionRoute += postRoute.StartsWith('/') ? postRoute : $"/{postRoute}";
            }

            return await Client.SendGetAsync(form, formData, submissionRoute);
        }
    }
}
