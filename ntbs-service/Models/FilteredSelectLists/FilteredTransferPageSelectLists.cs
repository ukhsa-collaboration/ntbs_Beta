using System.Collections.Generic;

namespace ntbs_service.Models.FilteredSelectLists
{
    public class FilteredTransferPageSelectLists
    {
        public IEnumerable<OptionValue> TbServices { get; set; }
        public IEnumerable<OptionValue> CaseManagers { get; set; }
    }
}
