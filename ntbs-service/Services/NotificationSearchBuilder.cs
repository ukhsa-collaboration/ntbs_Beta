using System;
using System.Linq;
using ntbs_service.Models;
using Microsoft.EntityFrameworkCore;

namespace ntbs_service.Services
{
    public interface INotificationSearchBuilder
    {
        INotificationSearchBuilder FilterById(string id);
        INotificationSearchBuilder FilterByFamilyName(string familyName);
        INotificationSearchBuilder FilterByGivenName(string givenName);
        INotificationSearchBuilder FilterByPartialDob(PartialDate partialDob);
        INotificationSearchBuilder FilterByPartialNotificationDate(PartialDate partialNotificationDate);
        INotificationSearchBuilder FilterBySex(int? sexId);
        INotificationSearchBuilder FilterByPostcode(string postcode);
        INotificationSearchBuilder FilterByBirthCountry(int? countryId);
        INotificationSearchBuilder FilterByTBService(string TBService);
        IQueryable<Notification> GetResult();
    }

    public class NotificationSearchBuilder : INotificationSearchBuilder
    {
        IQueryable<Notification> notificationIQ;
        
        public NotificationSearchBuilder(IQueryable<Notification> notificationIQ)
        {
            this.notificationIQ = notificationIQ;
        }

        public INotificationSearchBuilder FilterById(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                int.TryParse(id, out int parsedId);
                notificationIQ = notificationIQ.Where(s => s.NotificationId.Equals(parsedId) 
                    || s.ETSID.Equals(id) || s.LTBRID.Equals(id) || s.PatientDetails.NhsNumber.Equals(id));
            }
            return this;
        }

        public INotificationSearchBuilder FilterByFamilyName(string familyName)
        {
            if (!String.IsNullOrEmpty(familyName))
            {
                notificationIQ = notificationIQ.Where(s => EF.Functions.Like(s.PatientDetails.FamilyName, $"%{familyName}%"));
            }
            return this;
        }

        public INotificationSearchBuilder FilterByGivenName(string givenName)
        {
            if (!String.IsNullOrEmpty(givenName))
            {
                notificationIQ = notificationIQ.Where(s => EF.Functions.Like(s.PatientDetails.GivenName, $"%{givenName}%"));
            }
            return this;
        }

        public INotificationSearchBuilder FilterByPostcode(string postcode)
        {
            if (!String.IsNullOrEmpty(postcode))
            {
                var postcodeNoWhitespace = postcode.Replace(" ", "");
                notificationIQ = notificationIQ.Where(s => EF.Functions.Like(s.PatientDetails.PostcodeToLookup, $"{postcodeNoWhitespace}%"));
            }
            return this;
        }

        public INotificationSearchBuilder FilterByPartialDob(PartialDate partialDob) 
        {
            if(!(partialDob == null || partialDob.IsEmpty())) {
                partialDob.TryConvertToDateTimeRange(out DateTime? dateRangeStart, out DateTime? dateRangeEnd);
                notificationIQ = notificationIQ.Where(s => s.PatientDetails.Dob >= dateRangeStart && s.PatientDetails.Dob < dateRangeEnd);
            }
            
            return this;
        }

        public INotificationSearchBuilder FilterByPartialNotificationDate(PartialDate partialNotificationDate) 
        {
            if(!(partialNotificationDate == null || partialNotificationDate.IsEmpty())) {
                partialNotificationDate.TryConvertToDateTimeRange(out DateTime? dateRangeStart, out DateTime? dateRangeEnd);
                notificationIQ = notificationIQ.Where(s => s.SubmissionDate >= dateRangeStart && s.SubmissionDate < dateRangeEnd);
            }
            
            return this;
        }

        public INotificationSearchBuilder FilterBySex(int? sexId) 
        {
            if(sexId != null) {
                notificationIQ = notificationIQ.Where(s => s.PatientDetails.SexId.Equals(sexId));
            }
            return this;
        }

        public INotificationSearchBuilder FilterByBirthCountry(int? countryId) 
        {
            if(countryId != null) {
                notificationIQ = notificationIQ.Where(s => s.PatientDetails.CountryId.Equals(countryId));
            }
            return this;
        }

        public INotificationSearchBuilder FilterByTBService(string TBService) 
        {
            if(TBService != null) {
                notificationIQ = notificationIQ.Where(s => s.Episode.TBServiceCode.Equals(TBService));
            }
            return this;
        }

        public IQueryable<Notification> GetResult()
        {
            return notificationIQ;
        }
    }
}
