using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface INotificationSearchBuilder : ISearchBuilderParent
    {
        IQueryable<Notification> GetResult();
    }

    public class NotificationSearchBuilder : INotificationSearchBuilder
    {
        IQueryable<Notification> notificationIQ;
        
        public NotificationSearchBuilder(IQueryable<Notification> notificationIQ)
        {
            this.notificationIQ = notificationIQ;
        }

        public ISearchBuilderParent FilterById(string id)
        {
            if (!String.IsNullOrEmpty(id))
            {
                int.TryParse(id, out int parsedId);
                notificationIQ = notificationIQ.Where(s => s.NotificationId.Equals(parsedId) 
                    || (s.ETSID != null && s.ETSID.Equals(id)) || (s.LTBRID != null && s.LTBRID.Equals(id)) || s.PatientDetails.NhsNumber.Equals(id));
            }
            return this;
        }

        public ISearchBuilderParent FilterByFamilyName(string familyName)
        {
            if (!String.IsNullOrEmpty(familyName))
            {
                notificationIQ = notificationIQ.Where(s => EF.Functions.Like(s.PatientDetails.FamilyName, $"%{familyName}%"));
            }
            return this;
        }

        public ISearchBuilderParent FilterByGivenName(string givenName)
        {
            if (!String.IsNullOrEmpty(givenName))
            {
                notificationIQ = notificationIQ.Where(s => EF.Functions.Like(s.PatientDetails.GivenName, $"%{givenName}%"));
            }
            return this;
        }

        public ISearchBuilderParent FilterByPostcode(string postcode)
        {
            if (!String.IsNullOrEmpty(postcode))
            {
                var postcodeNoWhitespace = postcode.Replace(" ", "");
                notificationIQ = notificationIQ.Where(s => EF.Functions.Like(s.PatientDetails.PostcodeToLookup, $"{postcodeNoWhitespace}%"));
            }
            return this;
        }

        public ISearchBuilderParent FilterByPartialDob(PartialDate partialDob) 
        {
            if(!(partialDob == null || partialDob.IsEmpty())) {
                partialDob.TryConvertToDateTimeRange(out DateTime? dateRangeStart, out DateTime? dateRangeEnd);
                notificationIQ = notificationIQ.Where(s => s.PatientDetails.Dob >= dateRangeStart && s.PatientDetails.Dob < dateRangeEnd);
            }
            
            return this;
        }

        public ISearchBuilderParent FilterByPartialNotificationDate(PartialDate partialNotificationDate) 
        {
            if(!(partialNotificationDate == null || partialNotificationDate.IsEmpty())) {
                partialNotificationDate.TryConvertToDateTimeRange(out DateTime? dateRangeStart, out DateTime? dateRangeEnd);
                notificationIQ = notificationIQ.Where(s => s.SubmissionDate >= dateRangeStart && s.SubmissionDate < dateRangeEnd);
            }
            
            return this;
        }

        public ISearchBuilderParent FilterBySex(int? sexId) 
        {
            if(sexId != null) {
                notificationIQ = notificationIQ.Where(s => s.PatientDetails.SexId.Equals(sexId));
            }
            return this;
        }

        public ISearchBuilderParent FilterByBirthCountry(int? countryId) 
        {
            if(countryId != null) {
                notificationIQ = notificationIQ.Where(s => s.PatientDetails.CountryId.Equals(countryId));
            }
            return this;
        }

        public ISearchBuilderParent FilterByTBService(string TBService) 
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
