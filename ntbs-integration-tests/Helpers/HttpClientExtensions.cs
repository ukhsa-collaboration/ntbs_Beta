using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using ntbs_service.Models;

namespace ntbs_integration_tests.Helpers
{
    public static class HttpClientExtensions
    {
        private const string TOKEN = "__RequestVerificationToken";
        private static readonly Regex TOKEN_REGEX = new Regex($"{TOKEN}=([^&]*)");


        public static Task<HttpResponseMessage> SendGetAsync(
            this HttpClient client,
            IHtmlFormElement form,
            Dictionary<string, string> formValues,
            string path)
        {
            return SendAsync(client, form, formValues, path, HttpMethod.Get, null);
        }

        public static Task<HttpResponseMessage> SendPostAsync(
            this HttpClient client,
            IHtmlFormElement form,
            Dictionary<string, string> formValues,
            string path,
            string submitType = ActionNameString.Save)
        {
            return SendAsync(client, form, formValues, path, HttpMethod.Post, submitType);
        }
        
        public static async Task<HttpResponseMessage> SendPostFormWithData(
            this HttpClient client,
            IHtmlDocument document,
            Dictionary<string, string> formData,
            string pageRoute,
            string postRoute = null,
            string submitType = null)
        {
            var data = formData ?? new Dictionary<string, string>();
            var form = (IHtmlFormElement)document.QuerySelector("form");

            var submissionRoute = pageRoute;
            if (!string.IsNullOrEmpty(postRoute))
            {
                submissionRoute += postRoute.StartsWith('/') ? postRoute : $"/{postRoute}";
            }

            return await client.SendPostAsync(form, data, submissionRoute, submitType);
        }

        private static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            IHtmlFormElement form,
            Dictionary<string, string> formValues,
            string path,
            HttpMethod httpMethod,
            string submitType)
        {
            VerifyFormElementsExist(form, formValues);

            var token = GetAntiForgeryToken(form);
            if(token != null)
            {
                formValues.Add(TOKEN, token);
            }
            if(submitType != null)
            {
                formValues.Add("actionName", submitType);
            }

            var submission = new HttpRequestMessage(httpMethod, path)
            {
                Content = new FormUrlEncodedContent(formValues)
            };

            return client.SendAsync(submission);
        }

        private static void VerifyFormElementsExist(IHtmlFormElement form, Dictionary<string, string> formValues)
        {
            foreach (var kvp in formValues)
            {
                if (form[kvp.Key] == null)
                {
                    throw new HtmlElementParseException($"Cannot find element {kvp.Key} on form");
                }
            }
        }

        private static string GetAntiForgeryToken(IHtmlFormElement form)
        {
            using (var reader = new StreamReader(form.GetSubmission().Body))
            {
                var submitBody = reader.ReadToEnd();
                var tokenMatch = TOKEN_REGEX.Match(submitBody);
                var token = tokenMatch.Success ? tokenMatch.Groups[1].Captures[0].Value : null;
                return token;
            }
        }
    }
}
