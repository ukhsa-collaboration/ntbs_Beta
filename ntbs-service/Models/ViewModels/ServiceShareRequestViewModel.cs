using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.ViewModels;

public class ServiceShareRequestViewModel
{
    [Display(Name = "TB Service to share record with")]
    [Required(ErrorMessage = ValidationMessages.Mandatory)]
    [AssertThat("SharingTBServiceCode != NotificationTBServiceCode", ErrorMessage = ValidationMessages.ShareDestinationCannotBeCurrentTbService)]
    public string SharingTBServiceCode { get; set; }

    [Display(Name = "Reason for TB Service share")]
    [MaxLength(1000, ErrorMessage = ValidationMessages.MaximumTextLength)]
    [ContainsNoTabs]
    [RegularExpression(ValidationRegexes.CharacterValidationAsciiBasicWithNewline,
        ErrorMessage = ValidationMessages.InvalidCharacter)]
    public string ReasonForTBServiceShare { get; set; }
    
    public string NotificationTBServiceCode { get; set; }
}
