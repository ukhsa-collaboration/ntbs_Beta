using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Services;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class NtbsSearchBuilderTest : IClassFixture<DatabaseFixture>
    {
        private readonly INtbsSearchBuilder _builder;

        public NtbsSearchBuilderTest(DatabaseFixture fixture)
        {
            this._builder = new NtbsSearchBuilder(fixture.Context.Notification); ;
        }

        [Fact]
        public void SearchById_ReturnsMatchOnNotificationId()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterById("1")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal(1, result.FirstOrDefault().NotificationId);
        }

        [Fact]
        public void SearchById_ReturnsMatchOnETSID()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterById("12")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("12", result.FirstOrDefault().ETSID);
        }

        [Fact]
        public void SearchById_ReturnsMatchOnLTBRID()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterById("223")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("223", result.FirstOrDefault().LTBRID);
        }

        [Fact]
        public void SearchById_ReturnsMatchOnNhsNumber()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterById("1234567890")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("1234567890", result.FirstOrDefault().PatientDetails.NhsNumber);
        }

        [Fact]
        public void SearchByIdWithSpaces_ReturnsMatchOnNhsNumber()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterById("123 456 7890")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("1234567890", result.FirstOrDefault().PatientDetails.NhsNumber);
        }

        [Fact]
        public void SearchById_ReturnsEmptyList_WhenNoMatchFound()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterById("30")).GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByFamilyName_FullSurname()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByFamilyName("merry")).GetResult().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);
        }

        [Fact]
        public void SearchByFamilyName_WildcardedPrefix()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByFamilyName("ry")).GetResult().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);
        }

        [Fact]
        public void SearchByFamilyName_WildcardedSuffix()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByFamilyName("merr")).GetResult().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);
        }

        [Fact]
        public void SearchByFamilyName_WildcardedPrefixAndSuffix()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByFamilyName("err")).GetResult().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);
        }

        [Fact]
        public void SearchByFamilyName_WhitespacePrefixAndSuffix()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByFamilyName("   Merry  ")).GetResult().ToList();

            Assert.Equal(2, result.Count);
            Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);
        }

        [Fact]
        public void SearchByFamilyName_NoResults()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByFamilyName("NonexistingName")).GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByGivenName_FullGivenName()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByGivenName("Christmas")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("Christmas", result.FirstOrDefault().PatientDetails.GivenName);
        }

        [Fact]
        public void SearchByGivenName_WildcardedPrefix()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByGivenName("mas")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("Christmas", result.FirstOrDefault().PatientDetails.GivenName);
        }

        [Fact]
        public void SearchByGivenName_WildcardedSuffix()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByGivenName("Go")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("Goround", result.FirstOrDefault().PatientDetails.GivenName);
        }

        [Fact]
        public void SearchByGivenName_WildcardedPrefixAndSuffix()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByGivenName("roun")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("Goround", result.FirstOrDefault().PatientDetails.GivenName);
        }

        [Fact]
        public void SearchByGivenName_WhitespacePrefixAndSuffix()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByGivenName("  Goround   ")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("Goround", result.FirstOrDefault().PatientDetails.GivenName);
        }

        [Fact]
        public void SearchByGivenName_NoResults()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByGivenName("NonexistingName")).GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByPostcode_FullPostcode()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByPostcode("SW1 2RT")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("SW12RT", result.FirstOrDefault().PatientDetails.PostcodeToLookup);
        }

        [Fact]
        public void SearchByPostcode_WildcardedSuffix()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByPostcode("SW1")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("SW12RT", result.FirstOrDefault().PatientDetails.PostcodeToLookup);
        }

        [Fact]
        public void SearchByPostcode_NoResults()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByPostcode("Wrongpostcode")).GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchBySex_ReturnsMatchOnSexId()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterBySex(1)).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal(1, result.FirstOrDefault().PatientDetails.SexId);
        }

        [Fact]
        public void SearchBySex_NoResults()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterBySex(30)).GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByTBService_ReturnsMatchOnTBServiceCode()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByTBService("Ashford hospital")).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal("Ashford hospital", result.FirstOrDefault().HospitalDetails.TBServiceCode);
        }

        [Fact]
        public void SearchByTBService_NoResults()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByTBService("NonexistentService")).GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByBirthCountry_ReturnsMatchOnCountryId()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByBirthCountry(1)).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal(1, result.FirstOrDefault().PatientDetails.CountryId);
        }

        [Fact]
        public void SearchByBirthCountry_NoResults()
        {
            var result = ((INtbsSearchBuilder)_builder.FilterByBirthCountry(30)).GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByPartialNotificationDate_ReturnsMatchOnNotificationDate()
        {
            var result =
                ((INtbsSearchBuilder)_builder.FilterByPartialNotificationDate(
                    new PartialDate() { Day = "1", Month = "1", Year = "2000" })).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal(new DateTime(2000, 1, 1), result.FirstOrDefault().NotificationDate);
        }

        [Fact]
        public void SearchByPartialNotificationDate_NoResults()
        {
            var result =
                ((INtbsSearchBuilder)_builder.FilterByPartialNotificationDate(
                    new PartialDate() { Day = "1", Month = "1", Year = "3000" })).GetResult().ToList();

            Assert.Empty(result);
        }

        [Fact]
        public void SearchByPartialDob_ReturnsMatchOnDob()
        {
            var result =
                ((INtbsSearchBuilder)_builder.FilterByPartialDob(new PartialDate()
                {
                    Day = "1",
                    Month = "1",
                    Year = "1990"
                })).GetResult().ToList();

            Assert.Single(result);
            Assert.Equal(new DateTime(1990, 1, 1), result.FirstOrDefault().PatientDetails.Dob);
        }

        [Fact]
        public void SearchByPartialDob_NoResults()
        {
            var result =
                ((INtbsSearchBuilder)_builder.FilterByPartialDob(new PartialDate()
                {
                    Day = "1",
                    Month = "1",
                    Year = "3000"
                })).GetResult().ToList();

            Assert.Empty(result);
        }
    }

    public class DatabaseFixture : IDisposable
    {
        public DatabaseFixture()
        {
            Context = SetupTestDatabase(
                new NtbsContext(new DbContextOptionsBuilder<NtbsContext>()
                    .UseInMemoryDatabase(nameof(NtbsSearchBuilderTest))
                    .Options
                )
            );
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        private NtbsContext SetupTestDatabase(NtbsContext context)
        {
            context.Notification.AddRange(new List<Notification>
            {
                new Notification
                {
                    NotificationId = 1,
                    ETSID = "12",
                    LTBRID = "222",
                    LTBRPatientId = "22",
                    NotificationDate = new DateTime(2000, 1, 1),
                    PatientDetails =
                        new PatientDetails
                        {
                            FamilyName = "Merry",
                            GivenName = "Christmas",
                            NhsNumber = "1234567890",
                            SexId = 1,
                            CountryId = 1,
                            Dob = new DateTime(1990, 1, 1),
                            PostcodeToLookup = "SW12RT"
                        },
                    HospitalDetails = new HospitalDetails {TBServiceCode = "Ashford hospital"}
                },
                new Notification
                {
                    NotificationId = 2,
                    ETSID = "13",
                    LTBRID = "223",
                    LTBRPatientId = "23",
                    NotificationDate = new DateTime(2001, 1, 1),
                    PatientDetails =
                        new PatientDetails
                        {
                            FamilyName = "Merry",
                            GivenName = "Goround",
                            NhsNumber = "1234567891",
                            SexId = 2,
                            CountryId = 2,
                            Dob = new DateTime(1991, 1, 1),
                            PostcodeToLookup = "SW294FB"
                        },
                    HospitalDetails = new HospitalDetails {TBServiceCode = "Not Ashford"}
                }
            });
            context.SaveChanges();
            return context;
        }

        public NtbsContext Context { get; private set; }
    }
}
