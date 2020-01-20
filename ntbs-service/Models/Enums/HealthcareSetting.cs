using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum HealthcareSetting
    {
        [Display(Name = "A&E")]
        AccidentAndEmergency,
        [Display(Name = "Contact Tracing")]
        ContactTracing,
        [Display(Name = "Find and Trace")]
        FindAndTrace,
        [Display(Name = "GP")]
        GP,
        [Display(Name = "Other")]
        Other,
        [Display(Name = "Unknown")]
        Unknown
    }
}
