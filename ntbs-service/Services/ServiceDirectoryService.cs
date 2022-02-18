using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{
    public class ServiceDirectoryItemWrapper
    {
        public User User { get; }
        public PHEC Region { get; }
        public TBService TBService { get; }

        public ServiceDirectoryItemWrapper(User user = null, PHEC region = null, TBService tbService = null)
        {
            this.User = user;
            this.Region = region;
            this.TBService = tbService;
        }

        public bool IsUser()
        {
            return this.User is not null;
        }

        public bool IsRegion()
        {
            return this.Region is not null;
        }

        public bool IsTBService()
        {
            return this.TBService is not null;
        }
    }

    public interface IServiceDirectoryService
    {
        (IList<ServiceDirectoryItemWrapper>, int) GetPaginatedItems(
            IList<PHEC> regions, IList<User> users, IList<TBService> tbservices, PaginationParametersBase paginationParameters
            );
    }

    public class ServiceDirectoryService : IServiceDirectoryService
    {
        public (IList<ServiceDirectoryItemWrapper>, int) GetPaginatedItems(
            IList<PHEC> regions, IList<User> users, IList<TBService> tbservices, PaginationParametersBase paginationParameters
            )
        {
            var serviceDirectoryItems = new List<ServiceDirectoryItemWrapper>();
            
            serviceDirectoryItems.AddRange(regions.Select(r  => new ServiceDirectoryItemWrapper(region: r)));
            serviceDirectoryItems.AddRange(tbservices.Select(t => new ServiceDirectoryItemWrapper(tbService: t)));
            serviceDirectoryItems.AddRange(users.Select(u => new ServiceDirectoryItemWrapper(user: u)));

            
            var paginatedServiceDirectoryItems = serviceDirectoryItems
                .Skip(paginationParameters.Offset ?? 0)
                .Take(paginationParameters.PageSize)
                .ToList();
            
            return (paginatedServiceDirectoryItems, serviceDirectoryItems.Count);
        }
    }
}
