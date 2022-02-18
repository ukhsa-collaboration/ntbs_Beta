using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Services
{
    public interface IServiceDirectoryService
    {
        (IList<ServiceDirectoryItemWrapper>, int) GetPaginatedItems(
            IList<PHEC> regions, 
            IList<User> users, 
            IList<TBService> tbservices, 
            IList<Hospital> hospitals,
            PaginationParametersBase paginationParameters);
    }

    public class ServiceDirectoryService : IServiceDirectoryService
    {
        public (IList<ServiceDirectoryItemWrapper>, int) GetPaginatedItems(
            IList<PHEC> regions, 
            IList<User> users, 
            IList<TBService> tbservices,
            IList<Hospital> hospitals,
            PaginationParametersBase paginationParameters)
        {
            var serviceDirectoryItems = new List<ServiceDirectoryItemWrapper>();
            
            serviceDirectoryItems.AddRange(regions.Select(r  => new ServiceDirectoryItemWrapper(region: r)));
            serviceDirectoryItems.AddRange(tbservices.Select(t => new ServiceDirectoryItemWrapper(tbService: t)));
            serviceDirectoryItems.AddRange(hospitals.Select(h => new ServiceDirectoryItemWrapper(hospital: h)));
            serviceDirectoryItems.AddRange(users.Select(u => new ServiceDirectoryItemWrapper(user: u)));
            
            var paginatedServiceDirectoryItems = serviceDirectoryItems
                .Skip(paginationParameters.Offset ?? 0)
                .Take(paginationParameters.PageSize)
                .ToList();
            
            return (paginatedServiceDirectoryItems, serviceDirectoryItems.Count);
        }
    }
}
