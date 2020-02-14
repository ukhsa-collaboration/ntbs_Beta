using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class MbovisAlert : Alert
    {
        public override string Action => "M. bovis identified. Please complete enhanced surveillance questionnaire";
        // TODO MBovis provide correct anchor
        public override string ActionLink => RouteHelper.GetNotificationOverviewPathWithSectionAnchor(
            NotificationId.GetValueOrDefault(),
            NotificationSubPaths.EditClinicalDetails);

        public MbovisAlert()
        {
            AlertType = AlertType.EnhancedSurveillanceMbovis;
        }
    }
}
