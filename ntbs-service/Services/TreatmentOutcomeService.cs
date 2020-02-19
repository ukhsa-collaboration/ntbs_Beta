using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;

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
                notification.TreatmentEvents.Where(t =>
                    t.EventDate < (notification.ClinicalDetails.TreatmentStartDate ?? notification.NotificationDate)?.AddYears(2)
                    && t.EventDate > (notification.ClinicalDetails.TreatmentStartDate ?? notification.NotificationDate)?.AddYears(1));
            
            if (yearsAfterTreatmentStartDate == 2)
            {
                
            }
            
            var treatmentEventsBetweenTwoAndThreeYears =
                notification.TreatmentEvents.Where(t =>
                    t.EventDate < (notification.ClinicalDetails.TreatmentStartDate ?? notification.NotificationDate)?.AddYears(3)
                    && t.EventDate > (notification.ClinicalDetails.TreatmentStartDate ?? notification.NotificationDate)?.AddYears(2));
            return false;
        }
    }
}
