using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public abstract partial class SocialContextBase
    {
        public string DateRange
        {
            get
            {
                if (DateFrom == null && DateTo == null)
                {
                    return "Unspecified time range";
                }

                return $"{FormattedDateFrom} to {FormattedDateTo}";
            }
        }

        private string FormattedDateFrom => DateFrom.ConvertToString() ?? "Prior";
        private string FormattedDateTo => DateTo.ConvertToString() ?? "Present";
    }
}
