using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models
{
    public class Occupation
    {
        public int OccupationId { get; set; }
        public string Sector { get; set; }
        public string Role { get; set; }
        public bool HasFreeTextField { get; set; }
    }
}
