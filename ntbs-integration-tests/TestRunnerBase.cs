using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_integration_tests.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using ntbs_service;
using Xunit;
using AngleSharp;
using System.Linq;
using AngleSharp.Dom;
using System.Web;

namespace ntbs_integration_tests
{
    public abstract class TestRunnerBase : IClassFixture<NtbsWebApplicationFactory<Startup>>
    {
        protected readonly HttpClient client;
        protected readonly NtbsWebApplicationFactory<Startup> factory;
        protected virtual string PageRoute { get; }

        public TestRunnerBase(NtbsWebApplicationFactory<Startup> factory)
        {
            this.factory = factory;
            client = this.factory.CreateClient(new WebApplicationFactoryClientOptions
                {
                    AllowAutoRedirect = false
                });
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
            return $"Error:{validationMessage}";
        }

        protected async Task<HttpResponseMessage> SendFormWithData(IHtmlDocument document, Dictionary<string, string> formData)
        {
            var form = (IHtmlFormElement)document.QuerySelector("form");

            return await client.SendAsync(form, formData, PageRoute);
        }

        protected async Task<HttpResponseMessage> SendGetFormWithData(IHtmlDocument document, Dictionary<string, string> formData)
        {
            var form = (IHtmlFormElement)document.QuerySelector("form");

            return await client.SendGetAsync(form, formData, PageRoute);
        }

        protected string BuildValidationPath(Dictionary<string, string> formData, string subPath)
        {
            var queryString = string.Join("&", formData.Select(kvp =>
                            string.Format("{0}={1}", kvp.Key, kvp.Value)));
            return $"{PageRoute}/{subPath}?{queryString}";
        }
    }
}