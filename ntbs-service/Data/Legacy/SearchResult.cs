using System;

namespace ntbs_service.Data.Legacy
{
    public class SearchResult
    {
        public string Source { get; set; }
        public string ID { get; set; }
        public string NhsNumber { get; set; }
        public string GivenNames { get; set; }
        public string FamilyNames { get; set; }
        public DateTime DateOfBirth { get; set; }
        public DateTime DateOfNotification { get; set; }
    }
}
