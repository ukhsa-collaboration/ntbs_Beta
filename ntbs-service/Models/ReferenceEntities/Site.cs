using System.Collections.Generic;
using ntbs_service.Models.Entities;

namespace ntbs_service.Models.ReferenceEntities
{
    public class Site
    {
        public int SiteId { get; set; }
        public string Description { get; set; }
        public virtual List<NotificationSite> NotificationSites { get; set; }
    }

    public enum SiteId {
        PULMONARY = 1,
        BONE_SPINE = 2,
        BONE_OTHER = 3,
        CNS_MENINGITIS = 4,
        CNS_OTHER = 5,
        OCULAR = 6,
        CRYPTIC = 7,
        GASTROINTESTINAL = 8,
        GENITOURINARY = 9,
        LYMPH_INTRA = 10,
        LYMPH_EXTRA = 11,
        LARYNGEAL = 12,
        MILIARY = 13,
        PLEURAL = 14,
        PERICARDIAL = 15,
        SKIN = 16,
        OTHER = 17
    }
}
