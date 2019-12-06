namespace ntbs_service.Helpers
{
    public static class FormattingHelper
    {
        public static string TrueFalseToYesNo(bool? x)
        {
            if (x == null)
            {
                return string.Empty;
            }
            else
            {
                return x.Value ? "Yes" : "No";
            }
        }
    }
}
