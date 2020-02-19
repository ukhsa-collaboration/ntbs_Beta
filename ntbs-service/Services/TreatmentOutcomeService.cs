using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface ITreatmentOutcomeService
    {
        bool IsTreatmentOutcomeNeeded(Notification notification, int yearsAfterTreatmentStartDate);
    }

    public class TreatmentOutcomeService : ITreatmentOutcomeService
    {
        private readonly IAlertRepository _alertRepository;

        public TreatmentOutcomeService(
            IAlertRepository alertRepository)
        {
            _alertRepository = alertRepository;
        }

        public bool IsTreatmentOutcomeNeeded(Notification notification, int yearsAfterTreatmentStartDate)
        {
            if (yearsAfterTreatmentStartDate == 1)
            {
                return true;
            }

            var treatmentEventsBetweenOneAndTwoYears =
                GetOrderedTreatmentEventsInWindowXtoXMinus1Years(notification, 2);
            
            if (yearsAfterTreatmentStartDate == 2)
            {
                return IsOutcomeNeededAt24Months(notification);
            }

            if (yearsAfterTreatmentStartDate == 3)
            {
                var treatmentEventsBetweenTwoAndThreeYears =
                    GetOrderedTreatmentEventsInWindowXtoXMinus1Years(notification, 3);
                if (treatmentEventsBetweenTwoAndThreeYears == null)
                {
                    return IsOutcomeNeededAt24Months(notification);
                }
            }

            return false;
        }

        public IEnumerable<TreatmentEvent> GetOrderedTreatmentEventsInWindowXtoXMinus1Years(Notification notification, int numberOfYears)
        {
            return notification.TreatmentEvents.Where(t =>
                t.EventDate < (notification.ClinicalDetails.TreatmentStartDate ?? notification.NotificationDate)?.AddYears(numberOfYears)
                && t.EventDate > (notification.ClinicalDetails.TreatmentStartDate ?? notification.NotificationDate)?.AddYears(numberOfYears - 1))
                .OrderByDescending(t => t.EventDate);
        }

        private bool IsOutcomeNeededAt24Months(Notification notification)
        {
            var treatmentEventsBetweenOneAndTwoYears =
                GetOrderedTreatmentEventsInWindowXtoXMinus1Years(notification, 2);
            if (treatmentEventsBetweenOneAndTwoYears == null)
            {
                var mostRecentTreatmentEventBetweenZeroAndOneYears =
                    GetOrderedTreatmentEventsInWindowXtoXMinus1Years(notification, 1).First();
                return mostRecentTreatmentEventBetweenZeroAndOneYears.TreatmentOutcome.TreatmentOutcomeType ==
                       TreatmentOutcomeType.NotEvaluated || !mostRecentTreatmentEventBetweenZeroAndOneYears.TreatmentEventTypeIsOutcome;
            }
            return treatmentEventsBetweenOneAndTwoYears.Last().TreatmentOutcome.TreatmentOutcomeType == TreatmentOutcomeType.NotEvaluated;
        }
    }
}
