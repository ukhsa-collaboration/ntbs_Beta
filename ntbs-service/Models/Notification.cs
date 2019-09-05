namespace ntbs_service.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }

        public virtual PatientDetails PatientDetails { get; set; }
        public virtual ClinicalTimeline ClinicalTimeline { get; set; }
        public virtual Episode Episode { get; set; }
        public virtual PatientTBHistory PatientTBHistory { get; set; }
    }
}
