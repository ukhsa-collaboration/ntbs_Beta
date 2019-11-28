using System;

namespace ntbs_service.Data.Legacy
{
    public class SearchResult
    {
        public string Source { get; set; }
        public string NotificationId { get; set; }
        public string NhsNumber { get; set; }
        public DateTime NotificationDate { get; set; }
    }
}
