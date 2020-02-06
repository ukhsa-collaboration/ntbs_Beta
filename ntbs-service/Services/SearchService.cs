using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface ISearchService
    {
        Task<(IList<int> notificationIds, int count)> OrderAndPaginateQueryableAsync(INtbsSearchBuilder firstQueryable,
            PaginationParameters paginationParameters, ClaimsPrincipal user);
    }

    public class SearchService : ISearchService
    {
        private readonly IUserService _userService;

        public SearchService(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<(IList<int> notificationIds, int count)> OrderAndPaginateQueryableAsync(
            INtbsSearchBuilder searchBuilder,
            PaginationParameters paginationParameters,
            ClaimsPrincipal user)
        {
            
            var notificationIdsQueryable = await OrderQueryableByEditPermissionThenNotificationDateAsync(searchBuilder.GetResult(), user);
            var notificationIds = await GetPaginatedItemsAsync(notificationIdsQueryable.Select(n => n.NotificationId), paginationParameters);
            var count = await notificationIdsQueryable.CountAsync();
            return (notificationIds, count);
        }

        private async Task<IQueryable<Notification>> OrderQueryableByEditPermissionThenNotificationDateAsync(IQueryable<Notification> query, ClaimsPrincipal user) 
        {
            var permittedTbServiceCodes = (await _userService.GetTbServicesAsync(user)).Select(s => s.Code);
            return query
                .OrderByDescending(n => permittedTbServiceCodes.Contains(n.HospitalDetails.TBServiceCode))
                .ThenByDescending(n => n.NotificationStatus == NotificationStatus.Draft)
                .ThenByDescending(n => n.NotificationDate ?? n.CreationDate)
                // For notifications with the same NotificationDate order by NotificationId so that each search returns the same order each time
                .ThenByDescending(n => n.NotificationId);
        }

        private async Task<IList<T>> GetPaginatedItemsAsync<T>(IQueryable<T> items, PaginationParameters paginationParameters)
        {
            return await items.Skip(paginationParameters.NtbsOffset ?? 0)
                .Take(paginationParameters.NumberOfNtbsNotificationsToFetch).ToListAsync();
        }
    }
}
