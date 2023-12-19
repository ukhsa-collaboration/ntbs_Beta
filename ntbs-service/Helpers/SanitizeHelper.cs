using System;
using System.Text.RegularExpressions;

namespace ntbs_service.Helpers;

public static class SanitizeHelper
{
    public static string Sanitize(this string input)
    {
        if (!string.IsNullOrEmpty(input))
        {
            string SanitizeRegex = "[{}]";
            input = Regex.Replace(input, SanitizeRegex, "");
        }

        return input;
    }
}