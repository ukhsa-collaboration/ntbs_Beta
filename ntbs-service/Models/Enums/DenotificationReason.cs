using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    // The display names are linked to the ETS denotification reasons and are used for mapping during import
    public enum DenotificationReason
    {
        [Display(Name = "Not selected")]
        NotSelected,
        [Display(Name = "Duplicate entry")]
        DuplicateEntry,
        [Display(Name = "Duplicate episode (episodes less than 12 months apart)")]
        DuplicateEpisode,
        [Display(Name = "Patient found not to have TB (atypical mycobacteria)")]
        NotTbAtypicalMyco,
        [Display(Name = "Patient found not to have TB (other)")]
        NotTbOther,
        [Display(Name = "Other")]
        Other
    }
}
