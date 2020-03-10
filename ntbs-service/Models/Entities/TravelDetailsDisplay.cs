using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public partial class TravelDetails
    {
        public string HasRecentTravel => HasTravel.FormatYesNo();
    }
}
