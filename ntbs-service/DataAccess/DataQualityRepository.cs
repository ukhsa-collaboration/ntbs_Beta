using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.Core.Internal;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface IDataQualityRepository
    {
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityDraftAlerts();
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityBirthCountryAlerts();
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityClinicalDatesAlerts();
        Task<IList<Notification>> GetNotificationsEligibleForDataQualityClusterAlerts();
    }

    public class DataQualityRepository : IDataQualityRepository
    {
        private readonly NtbsContext _context;
        
        public DataQualityRepository(NtbsContext context)
        {
            _context = context;
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityDraftAlerts()
        {
            return await _context.Notification
                .Where(n => n.NotificationStatus == NotificationStatus.Draft)
                .Where(n => n.CreationDate < DateTime.Now.AddDays(-90))
                .ToListAsync();
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityBirthCountryAlerts()
        {
            return await _context.Notification
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .Where(n => n.CreationDate < DateTime.Now.AddDays(-45))
                .Where(n => n.PatientDetails.CountryId == Countries.UnknownId)
                .ToListAsync();
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityClinicalDatesAlerts()
        {
            return await _context.Notification
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .Where(n => n.CreationDate < DateTime.Now.AddDays(-45))
                .Where(n => n.ClinicalDetails.SymptomStartDate > n.ClinicalDetails.TreatmentStartDate
                            || n.ClinicalDetails.SymptomStartDate > n.ClinicalDetails.FirstPresentationDate
                            || n.ClinicalDetails.FirstPresentationDate > n.ClinicalDetails.TBServicePresentationDate
                            || n.ClinicalDetails.TBServicePresentationDate > n.ClinicalDetails.DiagnosisDate
                            || n.ClinicalDetails.DiagnosisDate > n.ClinicalDetails.TreatmentStartDate)
                .ToListAsync();
        }
        
        public async Task<IList<Notification>> GetNotificationsEligibleForDataQualityClusterAlerts()
        {
            return await _context.Notification
                .Where(n => n.NotificationStatus == NotificationStatus.Notified)
                .Where(n => n.CreationDate < DateTime.Now.AddDays(-45))
                .Where(n => n.ClusterId != null 
                            && n.SocialContextAddresses.IsNullOrEmpty() 
                            && n.SocialContextVenues.IsNullOrEmpty())
                .ToListAsync();
        }
    }
}
