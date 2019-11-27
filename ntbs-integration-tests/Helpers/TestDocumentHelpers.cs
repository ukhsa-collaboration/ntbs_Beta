using AngleSharp.Html.Dom;
using ntbs_service.Helpers;
using Xunit;

namespace ntbs_integration_tests.Helpers
{
    public static class HtmlDocumentHelpers
    {        
        public static string FullErrorMessage(string validationMessage)
        {
            return string.Format($"Error:{validationMessage}");
        }
    }
}