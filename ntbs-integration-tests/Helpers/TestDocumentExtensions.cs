using System.Collections.Generic;
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
            var errorSpan = document?.QuerySelector($"span#{input}-error");
            Assert.NotNull(errorSpan);
            if (errorSpan.ClassList.Contains("hidden")) return null;
            return errorSpan.TextContent;
        }

        public static void AssertErrorMessage(this IParentNode document, string inputName, string expectedMessage)
        {
            var expected = HtmlDocumentHelpers.FullErrorMessage(expectedMessage);
            var actual = document.GetError(inputName);
            Assert.Equal(expected, actual);
        }

        public static void AssertErrorSummaryMessage(this IHtmlDocument document, string summaryInputName, string spanInputName, string expectedMessage)
        {
            // assert summary text
            var errorLink = (IHtmlAnchorElement) document?.QuerySelector($"a#error-summary-{summaryInputName}");
            Assert.NotNull(errorLink);
            Assert.Equal(expectedMessage, errorLink.TextContent);

            // assert link works
            var errorParentId = errorLink.Href.Split("#").Last();
            var errorParent = document.QuerySelector($"#{errorParentId}");
            Assert.NotNull(errorParent);

            // assert error text
            errorParent.AssertErrorMessage(spanInputName, expectedMessage);
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
