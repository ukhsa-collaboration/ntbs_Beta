using System.Collections.Generic;

namespace ntbs_service.Models.FilteredSelectLists
{
    public class FilteredHospitalDetailsPageSelectLists
    {
        public IEnumerable<OptionValue> Hospitals { get; set; }
        public IEnumerable<OptionValue> CaseManagers { get; set; }
    }
}
