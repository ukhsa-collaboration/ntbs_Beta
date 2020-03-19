using System;
using System.Linq.Expressions;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public class DataQualityDotVotAlert : Alert
    {
        public static readonly Expression<Func<Notification, bool>> NotificationQualifiesExpression =
            n =>
                // record has one or more social risk factors but DOT is set to No
                ((
                    n.SocialRiskFactors.AlcoholMisuseStatus == Status.Yes ||
                    n.SocialRiskFactors.RiskFactorDrugs.Status == Status.Yes ||
                    n.SocialRiskFactors.RiskFactorHomelessness.Status == Status.Yes ||
                    n.SocialRiskFactors.RiskFactorImprisonment.Status == Status.Yes ||
                    n.SocialRiskFactors.MentalHealthStatus == Status.Yes ||
                    n.SocialRiskFactors.SmokingStatus == Status.Yes ||
                    n.SocialRiskFactors.AsylumSeekerStatus == Status.Yes ||
                    n.SocialRiskFactors.ImmigrationDetaineeStatus == Status.Yes
                ) && n.ClinicalDetails.IsDotOffered == false)
                || // OR - record has no social risk factors but DOT set to Yes
                (!(
                    n.SocialRiskFactors.AlcoholMisuseStatus == Status.Yes ||
                    n.SocialRiskFactors.RiskFactorDrugs.Status == Status.Yes ||
                    n.SocialRiskFactors.RiskFactorHomelessness.Status == Status.Yes ||
                    n.SocialRiskFactors.RiskFactorImprisonment.Status == Status.Yes ||
                    n.SocialRiskFactors.MentalHealthStatus == Status.Yes ||
                    n.SocialRiskFactors.SmokingStatus == Status.Yes ||
                    n.SocialRiskFactors.AsylumSeekerStatus == Status.Yes ||
                    n.SocialRiskFactors.ImmigrationDetaineeStatus == Status.Yes
                ) && n.ClinicalDetails.IsDotOffered == true);

        public static readonly Func<Notification, bool> NotificationQualifies =
            NotificationQualifiesExpression.Compile();
        
        public override string Action => "Please review whether value given for DOT is correct.";

        public override string ActionLink =>
            RouteHelper.GetNotificationOverviewPathWithSectionAnchor(
                NotificationId.GetValueOrDefault(),
                NotificationSubPaths.EditClinicalDetails);

        public DataQualityDotVotAlert()
        {
            AlertType = AlertType.DataQualityDotVotAlert;
        }
    }
}
