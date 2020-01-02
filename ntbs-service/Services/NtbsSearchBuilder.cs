using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface INtbsSearchBuilder : ISearchBuilder
    {
        IQueryable<Notification> GetResult();
    }

    public class NtbsSearchBuilder : INtbsSearchBuilder
    {
        IQueryable<Notification> notificationIQ;

        public NtbsSearchBuilder(IQueryable<Notification> notificationIQ)
        {
            this.notificationIQ = notificationIQ;
        }

        public ISearchBuilder FilterById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                int.TryParse(id, out int parsedId);
                notificationIQ = notificationIQ.Where(s => s.NotificationId.Equals(parsedId)
                    || (s.ETSID != null && s.ETSID.Equals(id)) || (s.LTBRID != null && s.LTBRID.Equals(id)) || s.PatientDetails.NhsNumber.Equals(id) || s.LTBRPatientId.Equals(id));
            }
            return this;
        }

        public ISearchBuilder FilterByFamilyName(string familyName)
        {
            if (!string.IsNullOrEmpty(familyName))
            {
                notificationIQ = notificationIQ.Where(s => EF.Functions.Like(s.PatientDetails.FamilyName, $"%{familyName}%"));
            }
            return this;
        }

        public ISearchBuilder FilterByGivenName(string givenName)
        {
            if (!string.IsNullOrEmpty(givenName))
            {
                notificationIQ = notificationIQ.Where(s => EF.Functions.Like(s.PatientDetails.GivenName, $"%{givenName}%"));
            }
            return this;
        }

        public ISearchBuilder FilterByPostcode(string postcode)
        {
            if (!string.IsNullOrEmpty(postcode))
            {
                var postcodeNoWhitespace = postcode.Replace(" ", "");
                notificationIQ = notificationIQ.Where(s => EF.Functions.Like(s.PatientDetails.PostcodeToLookup, $"{postcodeNoWhitespace}%"));
            }
            return this;
        }

        public ISearchBuilder FilterByPartialDob(PartialDate partialDob)
        {
            if (!(partialDob == null || partialDob.IsEmpty()))
            {
                partialDob.TryConvertToDateTimeRange(out DateTime? dateRangeStart, out DateTime? dateRangeEnd);
                notificationIQ = notificationIQ.Where(s => s.PatientDetails.Dob >= dateRangeStart && s.PatientDetails.Dob < dateRangeEnd);
            }

            return this;
        }

        public ISearchBuilder FilterByPartialNotificationDate(PartialDate partialNotificationDate)
        {
            if (!(partialNotificationDate == null || partialNotificationDate.IsEmpty()))
            {
                partialNotificationDate.TryConvertToDateTimeRange(out DateTime? dateRangeStart, out DateTime? dateRangeEnd);
                notificationIQ = notificationIQ.Where(s => s.NotificationDate >= dateRangeStart && s.NotificationDate < dateRangeEnd);
            }

            return this;
        }

        public ISearchBuilder FilterBySex(int? sexId)
        {
            if (sexId != null)
            {
                notificationIQ = notificationIQ.Where(s => s.PatientDetails.SexId.Equals(sexId));
            }
            return this;
        }

        public ISearchBuilder FilterByBirthCountry(int? countryId)
        {
            if (countryId != null)
            {
                notificationIQ = notificationIQ.Where(s => s.PatientDetails.CountryId.Equals(countryId));
            }
            return this;
        }

        public ISearchBuilder FilterByTBService(string TBService)
        {
            if (TBService != null)
            {
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
