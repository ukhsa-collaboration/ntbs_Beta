using System.ComponentModel.DataAnnotations;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities;

public class ServiceShareRequestModel
{
    [Display(Name = "TB Service record is shared with")]
    [Required(ErrorMessage = ValidationMessages.Mandatory)]
    [AssertThat("SecondaryTBServiceCode != TBServiceCode", ErrorMessage = ValidationMessages.ShareDestinationCannotBeCurrentTbService)]
    public string SecondaryTBServiceCode { get; set; }

    [Display(Name = "Reason for TB Service share")]
    public string ReasonForTBServiceShare { get; set; }
    
    public string TBServiceCode { get; set; }
}
