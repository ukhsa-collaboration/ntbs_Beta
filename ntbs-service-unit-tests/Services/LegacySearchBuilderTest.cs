using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Moq;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class LegacySearchBuilderTest
    {
        private readonly LegacySearchBuilder builder;
        private readonly Mock<IReferenceDataRepository> mockReferenceDataRepository;

        public LegacySearchBuilderTest()
        {
            mockReferenceDataRepository = new Mock<IReferenceDataRepository>();
            builder = new LegacySearchBuilder(mockReferenceDataRepository.Object);
        }

        [Fact]
        public void SearchById_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterById("1")).GetResult();

            Assert.Contains("AND (n.PrimaryNotificationId = @id OR n.SecondaryNotificationId = @id OR (n.GroupId = @id AND n.PrimarySource = 'LTBR') OR dmg.NhsNumber = @id)", sqlQuery);
            Assert.Equal("1", parameters.id);
        }

        [Fact]
        public void SearchByFamilyName_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByFamilyName("Smith")).GetResult();

            Assert.Contains("AND (dmg.FamilyName LIKE @familyName)", sqlQuery);
            Assert.Equal("%Smith%", parameters.familyName);
        }

        [Fact]
        public void SearchByFamilyNameAndId_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterById("1").FilterByFamilyName("Smith")).GetResult();

            Assert.Contains("AND (n.PrimaryNotificationId = @id OR n.SecondaryNotificationId = @id OR (n.GroupId = @id AND n.PrimarySource = 'LTBR') OR dmg.NhsNumber = @id)", sqlQuery);
            Assert.Contains("AND (dmg.FamilyName LIKE @familyName)", sqlQuery);
            Assert.Equal("1", parameters.id);
            Assert.Equal("%Smith%", parameters.familyName);
        }

        [Fact]
        public void SearchByGivenName_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByGivenName("Bob")).GetResult();

            Assert.Contains("AND (dmg.GivenName LIKE @givenName)", sqlQuery);
            Assert.Equal("%Bob%", parameters.givenName);
        }

        [Fact]
        public void SearchBySex_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterBySex(2)).GetResult();

            Assert.Contains("AND (dmg.NtbsSexId = @sexId)", sqlQuery);
            Assert.Equal(2, parameters.sexId);
        }

        [Fact]
        public void SearchByCountryId_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByBirthCountry(2)).GetResult();

            Assert.Contains("AND (dmg.BirthCountryId = @countryId)", sqlQuery);
            Assert.Equal(2, parameters.countryId);
        }

        [Fact]
        public void SearchByPostcode_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByPostcode("AWX2N")).GetResult();

            Assert.Contains("AND (addrs.Postcode LIKE @postcode)", sqlQuery);
            Assert.Equal("AWX2N%", parameters.postcode);
        }

        [Fact]
        public void SearchByPartialNotificationDate_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByPartialNotificationDate(new PartialDate() { Day = "1", Month = "1", Year = "2000" })).GetResult();

            Assert.Contains("AND (n.NotificationDate >= @notificationDateRangeStart AND n.NotificationDate < @notificationDateRangeEnd)", sqlQuery);
            Assert.Equal(new DateTime(2000, 1, 1), parameters.notificationDateRangeStart);
            Assert.Equal(new DateTime(2000, 1, 2), parameters.notificationDateRangeEnd);
        }

        [Fact]
        public void SearchByPartialDob_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByPartialDob(new PartialDate() { Day = "1", Month = "1", Year = "1990" })).GetResult();

            Assert.Contains("AND (dmg.DateOfBirth >= @dobDateRangeStart AND dmg.DateOfBirth < @dobDateRangeEnd)", sqlQuery);
            Assert.Equal(new DateTime(1990, 1, 1), parameters.dobDateRangeStart);
            Assert.Equal(new DateTime(1990, 1, 2), parameters.dobDateRangeEnd);
        }

        [Fact]
        public void SearchByTbService_ReturnsCorrectSqlQueryAndParameters()
        {
            var firstTestGuid = new Guid("2671e495-0aa3-4303-8a91-83993e220677");
            var secondTestGuid = new Guid("83281ffa-2bba-4376-850c-46974cc89be4");
            var hospitals = new List<Hospital>
            {
                new Hospital {HospitalId = firstTestGuid},
                new Hospital {HospitalId = secondTestGuid}
            };
            var hospitalList = Task.FromResult((IList<Hospital>)hospitals);
            mockReferenceDataRepository.Setup(s => s.GetHospitalsByTbServiceCodesAsync(new List<string> { "TBS0001" })).Returns(hospitalList);
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByTBService("TBS0001")).GetResult();

            Assert.Contains("AND (n.NtbsHospitalId IN @hospitals)", sqlQuery);
            Assert.Equal(new List<Guid> { firstTestGuid, secondTestGuid }, parameters.hospitals);
        }
    }
}
