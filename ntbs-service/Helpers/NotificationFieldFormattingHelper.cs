namespace ntbs_service.Helpers
{
    public static class NotificationFieldFormattingHelper
    {
        public static string FormatNHSNumberForDisplay(string nhsNumber)
        {
            if (string.IsNullOrEmpty(nhsNumber))
            {
                return string.Empty;
            }

            if (nhsNumber.Length != 10)
            {
                return nhsNumber;
            }

            return string.Join(" ",
                nhsNumber.Substring(0, 3),
                nhsNumber.Substring(3, 3),
                nhsNumber.Substring(6, 4)
            );
        }

        public static string FormatNhsNumberForModel(string rawNhsNumber)
        {
            return string.IsNullOrEmpty(rawNhsNumber) ? null : rawNhsNumber.Replace(" ", "");
        }
    }
}
