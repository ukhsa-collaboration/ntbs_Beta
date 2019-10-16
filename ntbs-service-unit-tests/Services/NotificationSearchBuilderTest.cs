using System.Collections.Generic;
using System.Linq;
using ntbs_service.Models;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class NotificationSearchBuilderTest
    {
        NotificationSearchBuilder builder;

        public NotificationSearchBuilderTest()
        {
            IQueryable<Notification> notifications = (new List<Notification> { 
                new Notification { 
                    NotificationId = 1,
                    ETSID = "12",
                    LTBRID = "222",
                    PatientDetails = new PatientDetails {
                        FamilyName = "Merry", 
                        GivenName = "Christmas", 
                        NhsNumber = "1234567890"
                    } 
                },
                new Notification {
                    NotificationId = 2,
                    ETSID = "13",
                    LTBRID = "223",
                    PatientDetails = new PatientDetails { 
                        FamilyName = "Merry", 
                        GivenName = "Goround",
                        NhsNumber = "1234567891"

                    } 
                },
            }).AsQueryable();

            builder = new NotificationSearchBuilder(notifications);
        }

        [Fact]
        public void SearchById_ReturnsMatchOnNotificationId()
        {
            var result = builder.FilterById("1").GetResult().ToList();

            Assert.Equal(1, result.Count());
            Assert.Equal(1, result.FirstOrDefault().NotificationId);
        }

        [Fact]
        public void SearchById_ReturnsMatchOnETSID()
        {
            var result = builder.FilterById("12").GetResult().ToList();

            Assert.Equal(1, result.Count());
            Assert.Equal("12", result.FirstOrDefault().ETSID);
        }

        [Fact]
        public void SearchById_ReturnsMatchOnLTBRID()
        {
            var result = builder.FilterById("223").GetResult().ToList();

            Assert.Equal(1, result.Count());
            Assert.Equal("223", result.FirstOrDefault().LTBRID);
        }

        [Fact]
        public void SearchById_ReturnsMatchOnNhsNumber()
        {
            var result = builder.FilterById("1234567890").GetResult().ToList();

            Assert.Equal(1, result.Count());
            Assert.Equal( "1234567890", result.FirstOrDefault().PatientDetails.NhsNumber);
        }

        [Fact]
        public void SearchById_ReturnsEmptyList_WhenNoMatchFound()
        {
            var result = builder.FilterById("30").GetResult().ToList();

            Assert.Equal(0, result.Count());
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

            Assert.Equal(0, result.Count());
        }

        [Fact]
        public void SearchByGivenName_FullGivenName()
        {
            var result = builder.FilterByGivenName("Christmas").GetResult().ToList();

            Assert.Equal(1, result.Count());
            Assert.Equal("Christmas", result.FirstOrDefault().PatientDetails.GivenName);       
        }

        [Fact]
        public void SearchByGivenName_WildcardedPrefix()
        {
            var result = builder.FilterByGivenName("mas").GetResult().ToList();

            Assert.Equal(1, result.Count());
            Assert.Equal("Christmas", result.FirstOrDefault().PatientDetails.GivenName);       
        }

        [Fact]
        public void SearchByGivenName_WildcardedSuffix()
        {
            var result = builder.FilterByGivenName("Go").GetResult().ToList();

            Assert.Equal(1, result.Count());
            Assert.Equal("Goround", result.FirstOrDefault().PatientDetails.GivenName);       
        }

        [Fact]
        public void SearchByGivenName_WildcardedPrefixAndSuffix()
        {
            var result = builder.FilterByGivenName("roun").GetResult().ToList();

            Assert.Equal(1, result.Count());
            Assert.Equal("Goround", result.FirstOrDefault().PatientDetails.GivenName);       
        }

        [Fact]
        public void SearchByGivenName_NoResults()
        {
            var result = builder.FilterByGivenName("NonexistingName").GetResult().ToList();

            Assert.Equal(0, result.Count());
        }
    }
}
