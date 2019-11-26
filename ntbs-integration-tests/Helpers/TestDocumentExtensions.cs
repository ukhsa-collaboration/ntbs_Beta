using AngleSharp.Html.Dom;
using Xunit;

namespace ntbs_integration_tests.Helpers
{
    public static class HtmlDocumentExtensions
    {
        public static string GetError(this IHtmlDocument document, string input)
        {
            var errorSpan = document?.QuerySelector($"span[id='{input}-error']");
            if (errorSpan == null) return null;
            if (errorSpan.ClassList.Contains("hidden")) return null;
            return errorSpan.TextContent;
        }

        public static void AssertErrorMessage(this IHtmlDocument document, string inputName, string expectedMessage, string propertyName = "")
        {
            var expected = HtmlDocumentHelpers.FullErrorMessage(expectedMessage, propertyName);
            var actual = document.GetError(inputName);
            Assert.Equal(expected, actual);
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
