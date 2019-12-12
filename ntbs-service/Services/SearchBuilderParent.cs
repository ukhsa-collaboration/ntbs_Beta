using System.Linq;
using ntbs_service.Models;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface ISearchBuilderParent
    {
        ISearchBuilderParent FilterById(string id);
        ISearchBuilderParent FilterByFamilyName(string familyName);
        ISearchBuilderParent FilterByGivenName(string givenName);
        ISearchBuilderParent FilterByPartialDob(PartialDate partialDob);
        ISearchBuilderParent FilterByPartialNotificationDate(PartialDate partialNotificationDate);
        ISearchBuilderParent FilterBySex(int? sexId);
        ISearchBuilderParent FilterByPostcode(string postcode);
        ISearchBuilderParent FilterByBirthCountry(int? countryId);
        ISearchBuilderParent FilterByTBService(string TBService);
    }
}