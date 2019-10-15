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
        INotificationSearchBuilder FilterByPartialDate(PartialDate partialDate);
        INotificationSearchBuilder FilterBySex(int? sexId);
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

        public INotificationSearchBuilder FilterByPartialDate(PartialDate partialDate) 
        {
            if(!(partialDate == null || partialDate.IsEmpty())) {
                partialDate.TryConvertToDateTimeRange(out DateTime? dateRangeStart, out DateTime? dateRangeEnd);
                notificationIQ = notificationIQ.Where(s => s.PatientDetails.Dob >= dateRangeStart && s.PatientDetails.Dob < dateRangeEnd);
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

        public IQueryable<Notification> GetResult()
        {
            return notificationIQ;
        }
    }
}
