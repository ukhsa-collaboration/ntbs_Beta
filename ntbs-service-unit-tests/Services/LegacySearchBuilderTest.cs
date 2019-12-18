using System;
using ntbs_service.Models;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class LegacySearchBuilderTest
    {
        readonly LegacySearchBuilder builder;

        public LegacySearchBuilderTest()
        {
            builder = new LegacySearchBuilder();
        }

        [Fact]
        public void SearchById_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterById("1")).GetResult();

            Assert.Contains("WHERE dmg.OldNotificationId = @id OR n.GroupId = @id AND n.Source = 'LTBR' OR dmg.NhsNumber = @id", sqlQuery);
            Assert.Equal("1", parameters.id);
        }

        [Fact]
        public void SearchByFamilyName_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByFamilyName("Smith")).GetResult();

            Assert.Contains("WHERE dmg.FamilyName LIKE @familyName", sqlQuery);
            Assert.Equal("%Smith%", parameters.familyName);
        }

        [Fact]
        public void SearchByFamilyNameAndId_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterById("1").FilterByFamilyName("Smith")).GetResult();

            Assert.Contains("WHERE dmg.OldNotificationId = @id OR n.GroupId = @id AND n.Source = 'LTBR' OR dmg.NhsNumber = @id", sqlQuery);
            Assert.Contains("AND dmg.FamilyName LIKE @familyName", sqlQuery);
            Assert.Equal("1", parameters.id);
            Assert.Equal("%Smith%", parameters.familyName);
        }

        [Fact]
        public void SearchByGivenName_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByGivenName("Bob")).GetResult();

            Assert.Contains("WHERE dmg.GivenName LIKE @givenName", sqlQuery);
            Assert.Equal("%Bob%", parameters.givenName);
        }

        [Fact]
        public void SearchBySex_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterBySex(2)).GetResult();

            Assert.Contains("WHERE dmg.NtbsSexId = @sexId", sqlQuery);
            Assert.Equal(2, parameters.sexId);
        }

        [Fact]
        public void SearchByCountryId_ReturnsCorrectSqlQueryAndParameters()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByBirthCountry(2)).GetResult();

            Assert.Contains("WHERE dmg.BirthCountryId = @countryId", sqlQuery);
            Assert.Equal(2, parameters.countryId);
        }

        [Fact]
        public void SearchByPartialNotificationDate_ReturnsMatchOnNotificationDate()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByPartialNotificationDate(new PartialDate() {Day = "1", Month = "1", Year = "2000"})).GetResult();

            Assert.Contains("n.NotificationDate >= @notificationDateRangeStart AND n.NotificationDate < @notificationDateRangeEnd", sqlQuery);
            Assert.Equal(new DateTime(2000, 1, 1), parameters.notificationDateRangeStart);
            Assert.Equal(new DateTime(2000, 1, 2), parameters.notificationDateRangeEnd);
        }

        [Fact]
        public void SearchByPartialDob_ReturnsMatchOnDob()
        {
            var (sqlQuery, parameters) = ((ILegacySearchBuilder)builder.FilterByPartialDob(new PartialDate() {Day = "1", Month = "1", Year = "1990"})).GetResult();

            Assert.Contains("WHERE dmg.DateOfBirth >= @dobDateRangeStart AND dmg.DateOfBirth < @dobDateRangeEnd", sqlQuery);
            Assert.Equal(new DateTime(1990, 1, 1), parameters.dobDateRangeStart);
            Assert.Equal(new DateTime(1990, 1, 2), parameters.dobDateRangeEnd);
        }
    }
}
