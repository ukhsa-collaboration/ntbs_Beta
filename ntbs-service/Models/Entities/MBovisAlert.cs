using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class MBovisAlert : Alert
    {
        public override string Action => "M. bovis identified. Please complete enhanced surveillance questionnaire";
        // TODO NTBS-371 MBovis provide correct anchor (use NotificationSubPaths.EditMBovisExposureToKnownCases)
        public override string ActionLink => RouteHelper.GetNotificationOverviewPathWithSectionAnchor(
            NotificationId.GetValueOrDefault(),
            "overview-mbovis-exposure-details");

        public MBovisAlert()
        {
            AlertType = AlertType.EnhancedSurveillanceMBovis;
        }
    }
}
