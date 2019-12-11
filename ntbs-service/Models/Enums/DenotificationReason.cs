using System.ComponentModel;

namespace ntbs_service.Models.Enums
{
    public enum DenotificationReason
    {
        NotSelected,
        [DisplayName("Duplicate entry")]
        DuplicateEntry,
        [DisplayName("Duplicate episode (episodes less than 12 months apart)")]
        DuplicateEpisode,
        [DisplayName("Patient found not to have TB (atypical mycobacteria)")]
        NotTbAtypicalMyco,
        [DisplayName("Patient found not to have TB (other)")]
        NotTbOther,
        [DisplayName("Other")]
        Other
    }
}
