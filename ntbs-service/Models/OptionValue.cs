using System;
using ntbs_service.Helpers;

namespace ntbs_service.Models
{
    public class OptionValue
    {
        public string Value { get; set; }
        public string Text { get; set; }
        public string Group { get; set; }

        public static OptionValue FromEnum(Enum e)
        {
            return new OptionValue
            {
                Value = (Convert.ToInt32(e)).ToString(),
                Text = e.GetDisplayName()
            };
        }
    }
}
