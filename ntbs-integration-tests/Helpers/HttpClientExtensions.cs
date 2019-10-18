using System.Collections.Generic;
using System.IO;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using AngleSharp.Html.Dom;
using Xunit;

namespace ntbs_integration_tests.Helpers
{
    public static class HttpClientExtensions
    {
        private const string TOKEN = "__RequestVerificationToken";
        private static Regex TOKEN_REGEX = new Regex($"{TOKEN}=([^&]*)");

        public static Task<HttpResponseMessage> SendAsync(
            this HttpClient client,
            IHtmlFormElement form,
            Dictionary<string, string> formValues,
            string path,
            string submitType = "Save")
        {
            VerifyFormElementsExist(form, formValues);

            var token = GetAntiForgeryToken(form);
            formValues.Add("actionName", submitType);
            formValues.Add(TOKEN, token);

            var submission = new HttpRequestMessage(HttpMethod.Post, path)
            {
                Content = new FormUrlEncodedContent(formValues)
            };

            return client.SendAsync(submission);
        }

        public static Task<HttpResponseMessage> SendGetAsync(
            this HttpClient client,
            IHtmlFormElement form,
            Dictionary<string, string> formValues,
            string path,
            string submitType = "Save")
        {
            VerifyFormElementsExist(form, formValues);

            formValues.Add("actionName", submitType);

            var submission = new HttpRequestMessage(HttpMethod.Get, path)
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
            StreamReader reader = new StreamReader(form.GetSubmission().Body);
            string submitBody = reader.ReadToEnd();

            var tokenMatch = TOKEN_REGEX.Match(submitBody);
            var token = tokenMatch.Success ? tokenMatch.Groups[1].Captures[0].Value : null;
            Assert.NotNull(token);
            return token;
        }
    }
}