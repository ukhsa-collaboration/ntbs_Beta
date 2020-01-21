using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models
{
    public class SpecimenPotentialMatchSelection
    {
        [Display(Name = "Potential Match")]
        [Required(ErrorMessage = ValidationMessages.RequiredSelect)]
        public int? NotificationId { get; set; }

        [Display(Name = "Notification Id")]
        [RequiredIf("NotificationIdIsManual",
            ErrorMessage = ValidationMessages.RequiredEnter)]
        [RegularExpression(pattern: ValidationRegexes.NumbersValidation,
            ErrorMessage = ValidationMessages.NumberFormat)]
        public string ManualNotificationId { get; set; }

        [NotMapped] public bool NotificationIdIsManual => NotificationId == Pages.LabResults.IndexModel.ManualNotificationIdValue;
    }
}
