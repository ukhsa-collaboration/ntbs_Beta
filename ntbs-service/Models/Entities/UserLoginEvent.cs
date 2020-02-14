using System;

namespace ntbs_service.Models.Entities
{
    public class UserLoginEvent
    {
        public int Id { get; set; }
        public string Username { get; set; }
        public DateTime LoginDate { get; set; }
        public string SystemName { get; set; }
    }
}
