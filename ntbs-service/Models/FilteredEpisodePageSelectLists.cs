using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace ntbs_service.Models
{
    [NotMapped]
    public class FilteredEpisodePageSelectLists
    {
        public IEnumerable<ListEntry> Hospitals { get; set; }
        public IEnumerable<ListEntry> CaseManagers { get; set; }
    }

    public class ListEntry
    {
        public string Value { get; set; }
        public string Text { get; set; }
    }
}
