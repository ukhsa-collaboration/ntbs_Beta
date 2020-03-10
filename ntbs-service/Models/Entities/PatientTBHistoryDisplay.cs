using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public partial class PatientTBHistory
    {
        public string PreviouslyHadTBYesNo => (PreviouslyHadTB).FormatYesNo();

    }
}
