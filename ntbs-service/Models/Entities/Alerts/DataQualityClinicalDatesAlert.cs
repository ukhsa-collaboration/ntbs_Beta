using System;
using System.Linq.Expressions;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities.Alerts
{
    public class DataQualityClinicalDatesAlert : Alert
    {
        public static readonly Expression<Func<Notification, bool>> NotificationQualifiesExpression =
            n => n.ClinicalDetails.SymptomStartDate > n.ClinicalDetails.TreatmentStartDate
                 || n.ClinicalDetails.SymptomStartDate > n.ClinicalDetails.FirstPresentationDate
                 || n.ClinicalDetails.FirstPresentationDate > n.ClinicalDetails.TBServicePresentationDate
                 || n.ClinicalDetails.TBServicePresentationDate > n.ClinicalDetails.DiagnosisDate
                 || n.ClinicalDetails.DiagnosisDate > n.ClinicalDetails.TreatmentStartDate;

        public static readonly Func<Notification, bool> NotificationQualifies =
            NotificationQualifiesExpression.Compile();

        public override string Action =>
            "One or more of the clinical dates appears to be out of sequence, please review.";

        public override string ActionLink =>
            RouteHelper.GetNotificationOverviewPathWithSectionAnchor(
                NotificationId.GetValueOrDefault(),
                NotificationSubPaths.EditClinicalDetails);

        public DataQualityClinicalDatesAlert()
        {
            AlertType = AlertType.DataQualityClinicalDates;
        }
    }
}
