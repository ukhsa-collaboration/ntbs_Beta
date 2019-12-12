using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models;
using ntbs_service.Models.Entities;

namespace ntbs_service.Services
{
    public interface ILegacySearchBuilder : ISearchBuilderParent
    {
        string GetResult();
    }

    public class LegacySearchBuilder : ILegacySearchBuilder
    {
        string sqlSearchBuilder;
        object parameters;
        
        public LegacySearchBuilder()
        {
            this.sqlSearchBuilder = "";
            this.parameters = new object();
        }

        public ISearchBuilderParent FilterById(string id)
        {
            if (!string.IsNullOrEmpty(id))
            {
                sqlSearchBuilder += "WHERE n.OldNotificationId = @id OR n.GroupId = @id AND n.Source = 'LTBR' OR dmg.NhsNumber = @IdFilter";
                parameters.id = id;
            }
            return this;
        }

        public ISearchBuilderParent FilterByFamilyName(string familyName)
        {
            return this;
        }

        public ISearchBuilderParent FilterByGivenName(string givenName)
        {
            return this;
        }

        public ISearchBuilderParent FilterByPostcode(string postcode)
        {
            return this;
        }

        public ISearchBuilderParent FilterByPartialDob(PartialDate partialDob) 
        {
            return this;
        }

        public ISearchBuilderParent FilterByPartialNotificationDate(PartialDate partialNotificationDate) 
        {
            return this;
        }

        public ISearchBuilderParent FilterBySex(int? sexId) 
        {
            return this;
        }

        public ISearchBuilderParent FilterByBirthCountry(int? countryId) 
        {
            return this;
        }

        public ISearchBuilderParent FilterByTBService(string TBService) 
        {
            return this;
        }

        public string GetResult()
        {
            return (sqlSearchBuilder, params);
        }
    }
}
