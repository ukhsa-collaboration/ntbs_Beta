using Microsoft.EntityFrameworkCore;
using MoreLinq.Extensions;
using ntbs_service.DataAccess;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface INotificationCloningService
    {
        Notification Clone(Notification notification);
    }

    public class NotificationCloningService : INotificationCloningService
    {
        private readonly NtbsContext _context;

        public NotificationCloningService(NtbsContext context)
        {
            _context = context;
        }

        public Notification Clone(Notification notification)
        {
            // Every primary record needs to go through the process of:
            // 1. detaching (including owned entities)
            // 2. wiping the id (setting to 0)
            // 3. being added to the context anew
            
            _context.Entry(notification).State = EntityState.Detached;
            _context.Entry(notification.ClinicalDetails).State = EntityState.Detached;
            _context.Entry(notification.ComorbidityDetails).State = EntityState.Detached;
            _context.Entry(notification.ContactTracing).State = EntityState.Detached;
            _context.Entry(notification.DrugResistanceProfile).State = EntityState.Detached;
            _context.Entry(notification.HospitalDetails).State = EntityState.Detached;
            _context.Entry(notification.ImmunosuppressionDetails).State = EntityState.Detached;
            _context.Entry(notification.MDRDetails).State = EntityState.Detached;
            _context.Entry(notification.MBovisDetails).State = EntityState.Detached;
            _context.Entry(notification.PatientDetails).State = EntityState.Detached;
            _context.Entry(notification.PatientTBHistory).State = EntityState.Detached;
            _context.Entry(notification.SocialRiskFactors).State = EntityState.Detached;
            _context.Entry(notification.SocialRiskFactors.RiskFactorDrugs).State = EntityState.Detached;
            _context.Entry(notification.SocialRiskFactors.RiskFactorHomelessness).State = EntityState.Detached;
            _context.Entry(notification.SocialRiskFactors.RiskFactorImprisonment).State = EntityState.Detached;
            _context.Entry(notification.SocialRiskFactors.RiskFactorSmoking).State = EntityState.Detached;
            _context.Entry(notification.TravelDetails).State = EntityState.Detached;
            _context.Entry(notification.VisitorDetails).State = EntityState.Detached;
            notification.NotificationId = 0;
            
            notification.NotificationSites.ForEach(site => _context.Entry(site).State = EntityState.Detached);
            
            _context.Entry(notification.TestData).State = EntityState.Detached;
            notification.TestData.NotificationId = 0;
            notification.TestData.ManualTestResults.ForEach(result =>
            {
                _context.Entry(result).State = EntityState.Detached;
                result.ManualTestResultId = 0;
                _context.ManualTestResult.Add(result);
            });
            
            notification.TreatmentEvents.ForEach(treatmentEvent =>
            {
                _context.Entry(treatmentEvent).State = EntityState.Detached;
                treatmentEvent.TreatmentEventId = 0;
                _context.TreatmentEvent.Add(treatmentEvent);
            });

            notification.SocialContextAddresses.ForEach(address =>
            {
                _context.Entry(address).State = EntityState.Detached;
                address.SocialContextAddressId = 0;
                _context.SocialContextAddress.Add(address);
            });
            notification.SocialContextVenues.ForEach(venue =>
            {
                _context.Entry(venue).State = EntityState.Detached;
                venue.SocialContextVenueId = 0;
                _context.SocialContextVenue.Add(venue);
            });
            
            notification.MBovisDetails.MBovisAnimalExposures.ForEach(answer =>
            {
                _context.Entry(answer).State = EntityState.Detached;
                answer.MBovisAnimalExposureId = 0;
                _context.MBovisAnimalExposure.Add(answer);
            });
            notification.MBovisDetails.MBovisOccupationExposures.ForEach(answer =>
            {
                _context.Entry(answer).State = EntityState.Detached;
                answer.MBovisOccupationExposureId = 0;
                _context.MBovisOccupationExposures.Add(answer);
            });
            notification.MBovisDetails.MBovisUnpasteurisedMilkConsumptions.ForEach(answer =>
            {
                _context.Entry(answer).State = EntityState.Detached;
                answer.MBovisUnpasteurisedMilkConsumptionId = 0;
                _context.MBovisUnpasteurisedMilkConsumption.Add(answer);
            });
            notification.MBovisDetails.MBovisExposureToKnownCases.ForEach(answer =>
            {
                _context.Entry(answer).State = EntityState.Detached;
                answer.MBovisExposureToKnownCaseId = 0;
                _context.MBovisExposureToKnownCase.Add(answer);
            });
            
            _context.Notification.Add(notification);
            
            return notification;
        }
    }
}
