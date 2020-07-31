using System.Collections.Generic;

namespace ntbs_service.Models.FilteredSelectLists
{
    public class FilteredManualTestPageSelectLists
    {
        public IEnumerable<OptionValue> SampleTypes { get; set; }
        public IEnumerable<OptionValue> Results { get; set; }
    }
}
