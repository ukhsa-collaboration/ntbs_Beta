using System.Linq;
using AngleSharp.Dom;
using AngleSharp.Html.Dom;
using Xunit;

namespace ntbs_integration_tests.Helpers
{
    public static class HtmlDocumentExtensions
    {
        public static string GetError(this IParentNode document, string input)
        {
            var errorSpan = document?.QuerySelector(EscapeQuerySelector($"span#{input}-error"));
            Assert.NotNull(errorSpan);
            return errorSpan.ClassList.Contains("hidden") ? null : errorSpan.TextContent;
        }

        public static void AssertErrorMessage(this IParentNode document, string inputName, string expectedMessage)
        {
            var expected = HtmlDocumentHelpers.FullErrorMessage(expectedMessage);
            var actual = document.GetError(inputName);
            Assert.Equal(expected, actual);
        }

        public static void AssertErrorSummaryMessage(this IHtmlDocument document,
            string summaryInputName,
            string spanInputName,
            string expectedMessage)
        {
            // assert the error appears in the error summary
            var errorLink = (IHtmlAnchorElement)document?.QuerySelector(EscapeQuerySelector($"a#error-summary-{summaryInputName}"));
            Assert.NotNull(errorLink);
            Assert.Equal(expectedMessage, errorLink.TextContent);

            // assert the link contained within the error in the error summary works
            var errorParentId = errorLink.Href.Split("#").Last();
            var errorParent = document.QuerySelector(EscapeQuerySelector($"#{errorParentId}"));
            Assert.NotNull(errorParent);

            // In some places we don't link to a particular field, e.g. if the error relates to multiple fields together
            if (spanInputName != null)
            {
                // assert the error is found where linked to by the error message and the correct error message is present
                errorParent.AssertErrorMessage(spanInputName, expectedMessage);
            }
        }

        private static string EscapeQuerySelector(string query)
        {
            return query.Replace("[", "\\[").Replace("]", "\\]");
        }

        public static void AssertInputTextValue(this IHtmlDocument document, string inputId, string expectedValue)
        {
            var actual = ((IHtmlInputElement)document?.GetElementById(inputId))?.Value;
            Assert.Equal(expectedValue, actual);
        }

        public static void AssertTextAreaValue(this IHtmlDocument document, string textareaId, string expectedValue)
        {
            var actual = ((IHtmlTextAreaElement)document?.GetElementById(textareaId))?.Value;
            Assert.Equal(expectedValue, actual);
        }

        public static void AssertInputRadioValue(this IHtmlDocument document, string radioId, bool expectedValue)
        {
            var actual = ((IHtmlInputElement)document?.GetElementById(radioId))?.IsChecked;
            Assert.Equal(expectedValue, actual);
        }

        public static void AssertInputSelectValue(this IHtmlDocument document, string selectId, string expectedValue)
        {
            var actual = ((IHtmlSelectElement)document?.GetElementById(selectId))?.Value;
            Assert.Equal(expectedValue, actual);
        }
    }
}
