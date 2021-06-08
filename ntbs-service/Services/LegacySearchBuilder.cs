using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using ntbs_service.DataAccess;
using ntbs_service.Models;

namespace ntbs_service.Services
{
    public interface ILegacySearchBuilder : ISearchBuilder
    {
        (string, dynamic) GetResult();
    }

    public class LegacySearchBuilder : ILegacySearchBuilder
    {
        private string sqlQuery;
        private readonly dynamic parameters;
        private readonly IReferenceDataRepository _referenceDataRepository;

        public LegacySearchBuilder(IReferenceDataRepository referenceDataRepository)
        {
            parameters = new ExpandoObject();
            _referenceDataRepository = referenceDataRepository;
        }

        public ISearchBuilder FilterById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                var condition =
                    "n.PrimaryNotificationId = @id OR " +
                    "n.SecondaryNotificationId = @id OR " +
                    "n.LtbrPatientId = @id OR " +
                    "dmg.NhsNumber = @id";
                var idNoWhitespace = id.Replace(" ", "");
                AppendCondition(condition);
                parameters.id = idNoWhitespace;
            }
            return this;
        }

        public ISearchBuilder FilterByFamilyName(string familyName)
        {
            if (!string.IsNullOrEmpty(familyName))
            {
                AppendCondition("dmg.FamilyName LIKE @familyName");
                var wildcardedFamilyName = '%' + familyName.Trim() + '%';
                parameters.familyName = wildcardedFamilyName;
            }
            return this;
        }

        public ISearchBuilder FilterByGivenName(string givenName)
        {
            if (!string.IsNullOrEmpty(givenName))
            {
                AppendCondition("dmg.GivenName LIKE @givenName");
                var wildcardedGivenName = '%' + givenName.Trim() + '%';
                parameters.givenName = wildcardedGivenName;
            }
            return this;
        }

        public ISearchBuilder FilterByPostcode(string postcode)
        {
            if (!string.IsNullOrEmpty(postcode))
            {
                var postcodeNoWhitespace = postcode.Replace(" ", "") + "%";
                AppendCondition("addrs.Postcode LIKE @postcode");
                parameters.postcode = postcodeNoWhitespace;
            }
            return this;
        }

        public ISearchBuilder FilterByPartialDob(PartialDate partialDob)
        {
            if (!(partialDob == null || partialDob.IsEmpty()))
            {
                partialDob.TryConvertToDateTimeRange(out var dateRangeStart, out var dateRangeEnd);
                AppendCondition("dmg.DateOfBirth >= @dobDateRangeStart AND dmg.DateOfBirth < @dobDateRangeEnd");
                parameters.dobDateRangeStart = dateRangeStart;
                parameters.dobDateRangeEnd = dateRangeEnd;
            }

            return this;
        }

        public ISearchBuilder FilterByPartialNotificationDate(PartialDate partialNotificationDate)
        {
            if (!(partialNotificationDate == null || partialNotificationDate.IsEmpty()))
            {
                partialNotificationDate.TryConvertToDateTimeRange(out var dateRangeStart, out var dateRangeEnd);
                AppendCondition("n.NotificationDate >= @notificationDateRangeStart AND n.NotificationDate < @notificationDateRangeEnd");
                parameters.notificationDateRangeStart = dateRangeStart;
                parameters.notificationDateRangeEnd = dateRangeEnd;
            }

            return this;
        }

        public ISearchBuilder FilterBySex(int? sexId)
        {
            if (sexId != null)
            {
                AppendCondition("dmg.NtbsSexId = @sexId");
                parameters.sexId = sexId;
            }
            return this;
        }

        public ISearchBuilder FilterByBirthCountry(int? countryId)
        {
            if (countryId != null)
            {
                AppendCondition("dmg.BirthCountryId = @countryId");
                parameters.countryId = countryId;
            }
            return this;
        }

        public ISearchBuilder FilterByTBService(string TBService)
        {
            if (!string.IsNullOrEmpty(TBService))
            {
                var hospitalGuids = _referenceDataRepository.GetHospitalsByTbServiceCodesAsync(new List<string> { TBService })
                    .Result
                    .Select(x => x.HospitalId);
                parameters.hospitals = hospitalGuids;
                AppendCondition("n.NtbsHospitalId IN @hospitals");
            }
            return this;
        }

        public (string, dynamic) GetResult()
        {
            return (sqlQuery, parameters);
        }

        private void AppendCondition(string condition)
        {
            sqlQuery += $@"
                AND ({condition})";
        }
    }
}
