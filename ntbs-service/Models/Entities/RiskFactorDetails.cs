using EFAuditer;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class RiskFactorDetails : IOwnedEntityForAuditing
    {
        public RiskFactorDetails() { }
        public RiskFactorDetails(RiskFactorType type)
        {
            Type = type;
        }

        // We already map different types to different tables; this is to distinguish them for auditing
        public RiskFactorType Type { get; set; }
        public Status? Status { get; set; }
        public bool? IsCurrent { get; set; }
        public bool? InPastFiveYears { get; set; }
        public bool? MoreThanFiveYearsAgo { get; set; }

        [NotMapped]
        public bool IsCurrentView
        {
            get
            {
                return IsCurrent ?? false;
            }
            set
            {
                IsCurrent = value;
            }
        }
        
        [NotMapped]
        public bool InPastFiveYearsView
        {
            get
            {
                return InPastFiveYears ?? false;
            }
            set
            {
                InPastFiveYears = value;
            }
        }
        
        [NotMapped]
        public bool MoreThanFiveYearsAgoView
        {
            get
            {
                return MoreThanFiveYearsAgo ?? false;
            }
            set
            {
                MoreThanFiveYearsAgo = value;
            }
        }

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
