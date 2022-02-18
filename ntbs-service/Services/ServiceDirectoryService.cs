using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{
    public class ServiceDirectoryItemWrapper
    {
        public User user { get; set; }
        public PHEC region { get; set; }

        public ServiceDirectoryItemWrapper(User user, PHEC region)
        {
            this.user = user;
            this.region = region;
        }

        public bool IsUser()
        {
            return this.user != null;
        }

        public bool IsRegion()
        {
            return this.region != null;
        }
    }

    public interface IServiceDirectoryService
    {
        (IList<ServiceDirectoryItemWrapper>, int) GetPaginatedItems(
            IList<PHEC> regions, IList<User> users, PaginationParametersBase paginationParameters
            );
    }

    public class ServiceDirectoryService : IServiceDirectoryService
    {
        public (IList<ServiceDirectoryItemWrapper>, int) GetPaginatedItems(IList<PHEC> regions, IList<User> users, PaginationParametersBase paginationParameters)
        {
            var serviceDirectoryItems = regions.Select(r => new ServiceDirectoryItemWrapper(user: null, region: r))
                .Concat(
                    users.Select(u => new ServiceDirectoryItemWrapper(user: u, region: null))
                )
                .ToList();
            
            var paginatedServiceDirectoryItems = serviceDirectoryItems
                .Skip(paginationParameters.Offset ?? 0)
                .Take(paginationParameters.PageSize)
                .ToList();
            return (paginatedServiceDirectoryItems, serviceDirectoryItems.Count);
        }
    }
}
