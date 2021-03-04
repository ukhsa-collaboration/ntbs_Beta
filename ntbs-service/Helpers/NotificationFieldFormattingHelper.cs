namespace ntbs_service.Helpers
{
    public static class NotificationFieldFormattingHelper
    {
        public static string FormatNHSNumber(string nhsNumber)
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
    }
}
