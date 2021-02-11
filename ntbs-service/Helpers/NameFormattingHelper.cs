using System.Text.RegularExpressions;

namespace ntbs_service.Helpers
{
    public static class NameFormattingHelper
    {
        public static string FormatDisplayName(string displayName)
        {
            // Extract the name "John Smith" from the display name "   John Smith (NHS Trust Name)"

            if (displayName == null)
            {
                return null;
            }

            var splitName = displayName.Split('(');
            return splitName.Length == 0 ? "" : splitName[0].Trim();
        }
    }
}
