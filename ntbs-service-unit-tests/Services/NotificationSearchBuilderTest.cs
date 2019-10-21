using System;
using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class NotificationSearchBuilderTest
    {
        readonly NotificationSearchBuilder builder;

        public NotificationSearchBuilderTest()
        {
            IQueryable<Notification> notifications = (new List<Notification> { 
                new Notification { 
                    NotificationId = 1,
                    ETSID = "12",
                    LTBRID = "222",
                    SubmissionDate = new DateTime(2000, 1, 1),
                    PatientDetails = new PatientDetails {
                        FamilyName = "Merry", 
                        GivenName = "Christmas", 
                        NhsNumber = "1234567890",
                        SexId = 1,
                        CountryId = 1,
                        Dob = new DateTime(1990, 1, 1)
                    },
                    Episode = new Episode {
                        TBServiceCode = "Ashford hospital"
                    }
                },
                new Notification {
                    NotificationId = 2,
                    ETSID = "13",
                    LTBRID = "223",
                    SubmissionDate = new DateTime(2001, 1, 1),
                    PatientDetails = new PatientDetails { 
                        FamilyName = "Merry", 
                        GivenName = "Goround",
                        NhsNumber = "1234567891",
                        SexId = 2,
                        CountryId = 2,
                        Dob = new DateTime(1991, 1, 1)
                    },
                    Episode = new Episode {
                        TBServiceCode = "Not Ashford"
                    }
                },
            }).AsQueryable();

            builder = new NotificationSearchBuilder(notifications);
        }

        [Fact]
        public void SearchById_ReturnsMatchOnNotificationId()
        {
            var result = builder.FilterById("1").GetResult().ToList();

            Assert.Single(result);
            Assert.Equal(1, result.FirstOrDefault().NotificationId);
        }

        [Fact]
        public void SearchById_ReturnsMatchOnETSID()
        {
            var result = builder.FilterById("12").GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("12", result.FirstOrDefault().ETSID);
        }

        [Fact]
        public void SearchById_ReturnsMatchOnLTBRID()
        {
            var result = builder.FilterById("223").GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("223", result.FirstOrDefault().LTBRID);
        }

        [Fact]
        public void SearchById_ReturnsMatchOnNhsNumber()
        {
            var result = builder.FilterById("1234567890").GetResult().ToList();

            Assert.Single(result);
            Assert.Equal( "1234567890", result.FirstOrDefault().PatientDetails.NhsNumber);
        }

        [Fact]
        public void SearchById_ReturnsEmptyList_WhenNoMatchFound()
        {
            var result = builder.FilterById("30").GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByFamilyName_FullSurname()
        {
            var result = builder.FilterByFamilyName("merry").GetResult().ToList();

            Assert.Equal(2, result.Count());
            Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);       
        }

        [Fact]
        public void SearchByFamilyName_WildcardedPrefix()
        {
            var result = builder.FilterByFamilyName("ry").GetResult().ToList();

            Assert.Equal(2, result.Count());
            Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);       
        }

        [Fact]
        public void SearchByFamilyName_WildcardedSuffix()
        {
            var result = builder.FilterByFamilyName("merr").GetResult().ToList();

            Assert.Equal(2, result.Count());
            Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);       
        }

        [Fact]
        public void SearchByFamilyName_WildcardedPrefixAndSuffix()
        {
            var result = builder.FilterByFamilyName("err").GetResult().ToList();

            Assert.Equal(2, result.Count());
            Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);       
        }

        [Fact]
        public void SearchByFamilyName_NoResults()
        {
            var result = builder.FilterByFamilyName("NonexistingName").GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByGivenName_FullGivenName()
        {
            var result = builder.FilterByGivenName("Christmas").GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("Christmas", result.FirstOrDefault().PatientDetails.GivenName);       
        }

        [Fact]
        public void SearchByGivenName_WildcardedPrefix()
        {
            var result = builder.FilterByGivenName("mas").GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("Christmas", result.FirstOrDefault().PatientDetails.GivenName);       
        }

        [Fact]
        public void SearchByGivenName_WildcardedSuffix()
        {
            var result = builder.FilterByGivenName("Go").GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("Goround", result.FirstOrDefault().PatientDetails.GivenName);       
        }

        [Fact]
        public void SearchByGivenName_WildcardedPrefixAndSuffix()
        {
            var result = builder.FilterByGivenName("roun").GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("Goround", result.FirstOrDefault().PatientDetails.GivenName);       
        }

        [Fact]
        public void SearchByGivenName_NoResults()
        {
            var result = builder.FilterByGivenName("NonexistingName").GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchBySex_ReturnsMatchOnSexId()
        {
            var result = builder.FilterBySex(1).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal(1, result.FirstOrDefault().PatientDetails.SexId);
        }

        [Fact]
        public void SearchBySex_NoResults()
        {
            var result = builder.FilterBySex(30).GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByTBService_ReturnsMatchOnTBServiceCode()
        {
            var result = builder.FilterByTBService("Ashford hospital").GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("Ashford hospital", result.FirstOrDefault().Episode.TBServiceCode);
        }

        [Fact]
        public void SearchByTBService_NoResults()
        {
            var result = builder.FilterByTBService("NonexistentService").GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByBirthCountry_ReturnsMatchOnCountryId()
        {
            var result = builder.FilterByBirthCountry(1).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal(1, result.FirstOrDefault().PatientDetails.CountryId);
        }

        [Fact]
        public void SearchByBirthCountry_NoResults()
        {
            var result = builder.FilterByBirthCountry(30).GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByPartialNotificationDate_ReturnsMatchOnNotificationDate()
        {
            var result = builder.FilterByPartialNotificationDate(new PartialDate() {Day = "1", Month = "1", Year = "2000"}).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal(new DateTime(2000, 1, 1), result.FirstOrDefault().SubmissionDate);
        }

        [Fact]
        public void SearchByPartialNotificationDate_NoResults()
        {
            var result = builder.FilterByPartialNotificationDate(new PartialDate() {Day = "1", Month = "1", Year = "3000"}).GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByPartialDob_ReturnsMatchOnDob()
        {
            var result = builder.FilterByPartialDob(new PartialDate() {Day = "1", Month = "1", Year = "1990"}).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal(new DateTime(1990, 1, 1), result.FirstOrDefault().PatientDetails.Dob);
        }

        [Fact]
        public void SearchByPartialDob_NoResults()
        {
            var result = builder.FilterByPartialDob(new PartialDate() {Day = "1", Month = "1", Year = "3000"}).GetResult().ToList();

            Assert.Empty(result);
        }
    }
}
