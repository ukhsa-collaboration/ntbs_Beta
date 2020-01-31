using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface ISearchBuilder
    {
        ISearchBuilder FilterById(string id);
        ISearchBuilder FilterByFamilyName(string familyName);
        ISearchBuilder FilterByGivenName(string givenName);
        ISearchBuilder FilterByPartialDob(PartialDate partialDob);
        ISearchBuilder FilterByPartialNotificationDate(PartialDate partialNotificationDate);
        ISearchBuilder FilterBySex(int? sexId);
        ISearchBuilder FilterByPostcode(string postcode);
        ISearchBuilder FilterByBirthCountry(int? countryId);
        ISearchBuilder FilterByTBService(string TBService);
        ISearchBuilder OrderByEditPermission(List<string> TbServices);
    }
}
