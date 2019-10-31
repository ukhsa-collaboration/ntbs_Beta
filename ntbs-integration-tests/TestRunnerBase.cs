using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using ntbs_service;
using Xunit;
using AngleSharp;
using System.Linq;

namespace ntbs_integration_tests
{
    public abstract class TestRunnerBase : IClassFixture<NtbsWebApplicationFactory<Startup>>
    {
        protected readonly HttpClient Client;
        protected readonly NtbsWebApplicationFactory<Startup> Factory;
        protected virtual string PageRoute { get; }

        protected TestRunnerBase(NtbsWebApplicationFactory<Startup> factory)
        {
            Factory = factory;
            Client = Factory.CreateClientWithoutRedirects();
        }

        protected async Task<IHtmlDocument> GetDocumentAsync(HttpResponseMessage response)
        {
            var content = await response.Content.ReadAsStringAsync();
            var document = await BrowsingContext.New().OpenAsync(req => req.Content(content));
            return (IHtmlDocument)document;
        }

        protected string GetPageRouteForId(int id)
        {
            return $"{PageRoute}?id={id}";
        }

        protected string BuildRoute(string route, int id)
        {
            return $"{route}?id={id}";
        }

        protected string BuildEditRoute(string route, int id, bool isBeingSubmitted = false)
        {
            return $"{BuildRoute(route, id)}&isBeingSubmitted={isBeingSubmitted}";
        }

        protected string GetRedirectLocation(HttpResponseMessage response)
        {
            return response.Headers.Location.OriginalString;
        }

        protected string FullErrorMessage(string validationMessage)
        {
            return HtmlDocumentHelpers.FullErrorMessage(validationMessage);
        }

        protected async Task<HttpResponseMessage> SendPostFormWithData(
            IHtmlDocument document,
            Dictionary<string, string> formData, 
            string postRoute = null)
        {
            var form = (IHtmlFormElement)document.QuerySelector("form");

            var submissionRoute = PageRoute;
            if (!string.IsNullOrEmpty(postRoute))
            {
                submissionRoute += postRoute.StartsWith('/') ? postRoute : $"/{postRoute}";
            }

            return await Client.SendPostAsync(form, formData, submissionRoute);
        }

        protected async Task<HttpResponseMessage> SendGetFormWithData(
            IHtmlDocument document, 
            Dictionary<string, string> formData, 
            string postRoute = null)
        {
            var form = (IHtmlFormElement)document.QuerySelector("form");

            var submissionRoute = PageRoute;
            if (!string.IsNullOrEmpty(postRoute))
            {
                submissionRoute += postRoute.StartsWith('/') ? postRoute : $"/{postRoute}";
            }

            return await Client.SendGetAsync(form, formData, submissionRoute);
        }

        protected string BuildValidationPath(Dictionary<string, string> formData, string subPath)
        {
            var queryString = string.Join("&", formData.Select(kvp => $"{kvp.Key}={kvp.Value}"));
            return $"{PageRoute}/{subPath}?{queryString}";
        }
    }
}
