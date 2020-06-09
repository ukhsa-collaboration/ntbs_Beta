using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public partial class PreviousTbHistory
    {
        public string PreviouslyHadTbYesNo => PreviouslyHadTb.ToString();

    }
}
