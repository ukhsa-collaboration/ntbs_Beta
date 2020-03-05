using System;
using System.Linq.Expressions;
using ntbs_service.Helpers;
using ntbs_service.Models.Enums;
using ntbs_service.Services;

namespace ntbs_service.Models.Entities
{
    public class DataQualityTreatmentOutcome24 : Alert
    {
        public static readonly Expression<Func<Notification, bool>> NotificationInQualifyingDateRangeExpression =
            n => (n.ClinicalDetails.TreatmentStartDate ?? n.NotificationDate) < DateTime.Today.AddYears(-2);

        public static readonly Func<Notification, bool> NotificationInRangeQualifies =
            n => TreatmentOutcomesHelper.IsTreatmentOutcomeMissingAtXYears(n, 2);

        public static readonly Func<Notification, bool> NotificationQualifies = n =>
            NotificationInQualifyingDateRangeExpression.Compile()(n) && NotificationInRangeQualifies(n);

        public override string Action => 
            "No treatment outcome at 24 months can be found, please provide treatment outcome with appropriate date";

        public override string ActionLink =>
            RouteHelper.GetNotificationOverviewPathWithSectionAnchor(
                // ReSharper disable once PossibleInvalidOperationException
                NotificationId.Value,
                NotificationSubPaths.EditTreatmentEvents);

        public DataQualityTreatmentOutcome24()
        {
            AlertType = AlertType.DataQualityTreatmentOutcome24;
        }
    }
}
