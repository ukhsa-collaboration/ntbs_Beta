using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EFAuditer;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface ISearchService
    {
        IQueryable<Notification> FilterById(IQueryable<Notification> IQ, string IdFilter);
        IQueryable<Notification> FilterBySex(IQueryable<Notification> IQ, int sexId);
        IQueryable<Notification> FilterByPartialDate(IQueryable<Notification> IQ, PartialDate partialDate);
        IQueryable<Notification> OrderQueryableByNotificationDate(IQueryable<Notification> query);
        Task<IList<T>> GetPaginatedItemsAsync<T>(IQueryable<T> source, PaginationParameters paginationParameters);
    }

    public class SearchService : ISearchService
    {

        public IQueryable<Notification> FilterById(IQueryable<Notification> IQ, string IdFilter) 
        {
            return IQ.Where(s => s.NotificationId.Equals(Int32.Parse(IdFilter)) 
                    || s.ETSID.Equals(IdFilter) || s.LTBRID.Equals(IdFilter) || s.PatientDetails.NhsNumber.Equals(IdFilter));
        }

        public IQueryable<Notification> FilterByPartialDate(IQueryable<Notification> IQ, PartialDate partialDate) 
        {
            partialDate.TryConvertToDateTimeRange(out DateTime? dateRangeStart, out DateTime? dateRangeEnd);
            return IQ.Where(s => s.PatientDetails.Dob >= dateRangeStart && s.PatientDetails.Dob < dateRangeEnd);
        }

        public IQueryable<Notification> FilterBySex(IQueryable<Notification> IQ, int sexId) 
        {
            return IQ.Where(s => s.PatientDetails.SexId.Equals(sexId));
        }

        public IQueryable<Notification> OrderQueryableByNotificationDate(IQueryable<Notification> query) 
        {
            return query.OrderByDescending(n => n.CreationDate)
                .OrderByDescending(n => n.SubmissionDate);
        }

        public async Task<IList<T>> GetPaginatedItemsAsync<T>(IQueryable<T> items, PaginationParameters paginationParameters)
        {
            return await items.Skip((paginationParameters.PageIndex - 1) * paginationParameters.PageSize)
                .Take(paginationParameters.PageSize).ToListAsync();
        }
    }
}