using System;
using System.Collections.Generic;

namespace ntbs_service.Models
{
    public partial class CohortReview
    {
        public int CohortReviewId { get; set; }
        public int NotificationId { get; set; }
        public int? CscidentifiedAdult { get; set; }
        public int? CscidentifiedChild { get; set; }
        public int? CscassessedAdult { get; set; }
        public int? CscassessedChild { get; set; }
        public int? CscunderInvestigationAdult { get; set; }
        public int? CscunderInvestigationChild { get; set; }
        public int? CscnumWithActiveDiseaseAdult { get; set; }
        public int? CscnumWithActiveDiseaseChild { get; set; }
        public int? CscnumWithLtbiadult { get; set; }
        public int? CscnumWithLtbichild { get; set; }
        public int? CscnumStartedLtbitreatmentAdult { get; set; }
        public int? CscnumStartedLtbitreatmentChild { get; set; }
        public int? CscnumCompletedLtbitreamentAdult { get; set; }
        public int? CscnumCompletedLtbitreamentChild { get; set; }
        public int? CscdiscontinuedLtbiadverseReactionAdult { get; set; }
        public int? CscdiscontinuedLtbiadverseReactionChild { get; set; }
        public int? CscdiscontinuedLtbideathAdult { get; set; }
        public int? CscdiscontinuedLtbideathChild { get; set; }
        public int? CscdiscontinuedLtbimovedAdult { get; set; }
        public int? CscdiscontinuedLtbimovedChild { get; set; }
        public int? CscdiscontinuedLtbirefusedAdult { get; set; }
        public int? CscdiscontinuedLtbirefusedChild { get; set; }

        public virtual Notification Notification { get; set; }
    }
}
