using System.ComponentModel.DataAnnotations;

namespace ntbs_service.Models.Enums
{
    public enum DenotificationReason
    {
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
