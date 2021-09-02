using Microsoft.EntityFrameworkCore;

namespace ntbs_service.Models.QueryEntities
{
    [Keyless]
    public class NotificationAndDuplicateIds
    {
        public int NotificationId { get; set; }
        public int DuplicateId { get; set; }
        public string NhsNumber { get; set; }
        public string DuplicateNhsNumber { get; set; }
    }
}
