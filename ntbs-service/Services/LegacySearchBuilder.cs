using System;
using System.Dynamic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface ILegacySearchBuilder : ISearchBuilder
    {
        (string, dynamic) GetResult();
    }

    public class LegacySearchBuilder : ILegacySearchBuilder
    {
        string sqlQuery;
        dynamic parameters;
        
        public LegacySearchBuilder()
        {
            this.sqlQuery = "";
            this.parameters = new ExpandoObject();
        }

        public ISearchBuilder FilterById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                sqlQuery += "WHERE n.OldNotificationId = @id OR n.GroupId = @id AND n.Source = 'LTBR' OR dmg.NhsNumber = @id";
                parameters.id = id;
            }
            return this;
        }

        public ISearchBuilder FilterByFamilyName(string familyName)
        {
            return this;
        }

        public ISearchBuilder FilterByGivenName(string givenName)
        {
            return this;
        }

        public ISearchBuilder FilterByPostcode(string postcode)
        {
            return this;
        }

        public ISearchBuilder FilterByPartialDob(PartialDate partialDob) 
        {
            return this;
        }

        public ISearchBuilder FilterByPartialNotificationDate(PartialDate partialNotificationDate) 
        {
            return this;
        }

        public ISearchBuilder FilterBySex(int? sexId) 
        {
            return this;
        }

        public ISearchBuilder FilterByBirthCountry(int? countryId) 
        {
            return this;
        }

        public ISearchBuilder FilterByTBService(string TBService) 
        {
            return this;
        }

        public (string, dynamic) GetResult()
        {
            return (sqlQuery, parameters);
        }
    }
}
