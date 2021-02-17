using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Services;
using ntbs_service_unit_tests.DataAccess;
using Xunit;

namespace ntbs_service_unit_tests.Services
{
    public class NtbsSearchBuilderTest
    {
        private DbContextOptions<NtbsContext> ContextOptions { get; }

        public NtbsSearchBuilderTest()
        {
            ContextOptions = new DbContextOptionsBuilder<NtbsContext>()
                .UseInMemoryDatabase(nameof(NtbsSearchBuilderTest))
                .Options;
        }

        [Fact]
        public async Task SearchById_ReturnsMatchOnNotificationId()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterById("1")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal(1, result.FirstOrDefault().NotificationId);
            }
        }

        [Fact]
        public async Task SearchById_ReturnsMatchOnETSID()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterById("12")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("12", result.FirstOrDefault().ETSID);
            }
        }

        [Fact]
        public async Task SearchById_ReturnsMatchOnLTBRID()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterById("223")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("223", result.FirstOrDefault().LTBRID);
            }
        }

        [Fact]
        public async Task SearchById_ReturnsMatchOnNhsNumber()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterById("1234567890")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("1234567890", result.FirstOrDefault().PatientDetails.NhsNumber);
            }
        }

        [Fact]
        public async Task SearchByIdWithSpaces_ReturnsMatchOnNhsNumber()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterById("123 456 7890")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("1234567890", result.FirstOrDefault().PatientDetails.NhsNumber);
            }
        }

        [Fact]
        public async Task SearchById_ReturnsEmptyList_WhenNoMatchFound()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterById("30")).GetResult().ToList();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task SearchByFamilyName_FullSurname()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByFamilyName("merry")).GetResult().ToList();

                Assert.Equal(2, result.Count);
                Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);
            }
        }

        [Fact]
        public async Task SearchByFamilyName_WildcardedPrefix()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByFamilyName("ry")).GetResult().ToList();

                Assert.Equal(2, result.Count);
                Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);
            }
        }

        [Fact]
        public async Task SearchByFamilyName_WildcardedSuffix()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByFamilyName("merr")).GetResult().ToList();

                Assert.Equal(2, result.Count);
                Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);
            }
        }

        [Fact]
        public async Task SearchByFamilyName_WildcardedPrefixAndSuffix()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByFamilyName("err")).GetResult().ToList();

                Assert.Equal(2, result.Count);
                Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);
            }
        }

        [Fact]
        public async Task SearchByFamilyName_WhitespacePrefixAndSuffix()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByFamilyName("   Merry  ")).GetResult().ToList();

                Assert.Equal(2, result.Count);
                Assert.Equal("Merry", result.FirstOrDefault().PatientDetails.FamilyName);
            }
        }

        [Fact]
        public async Task SearchByFamilyName_NoResults()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByFamilyName("NonexistingName")).GetResult().ToList();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task SearchByGivenName_FullGivenName()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByGivenName("Christmas")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("Christmas", result.FirstOrDefault().PatientDetails.GivenName);
            }
        }

        [Fact]
        public async Task SearchByGivenName_WildcardedPrefix()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByGivenName("mas")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("Christmas", result.FirstOrDefault().PatientDetails.GivenName);
            }
        }

        [Fact]
        public async Task SearchByGivenName_WildcardedSuffix()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByGivenName("Go")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("Goround", result.FirstOrDefault().PatientDetails.GivenName);
            }
        }

        [Fact]
        public async Task SearchByGivenName_WildcardedPrefixAndSuffix()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByGivenName("roun")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("Goround", result.FirstOrDefault().PatientDetails.GivenName);
            }
        }

        [Fact]
        public async Task SearchByGivenName_WhitespacePrefixAndSuffix()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByGivenName("  Goround   ")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("Goround", result.FirstOrDefault().PatientDetails.GivenName);
            }
        }

        [Fact]
        public async Task SearchByGivenName_NoResults()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByGivenName("NonexistingName")).GetResult().ToList();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task SearchByPostcode_FullPostcode()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByPostcode("SW1 2RT")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("SW12RT", result.FirstOrDefault().PatientDetails.PostcodeToLookup);
            }
        }

        [Fact]
        public async Task SearchByPostcode_WildcardedSuffix()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByPostcode("SW1")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("SW12RT", result.FirstOrDefault().PatientDetails.PostcodeToLookup);
            }
        }

        [Fact]
        public async Task SearchByPostcode_NoResults()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByPostcode("Wrongpostcode")).GetResult().ToList();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task SearchBySex_ReturnsMatchOnSexId()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterBySex(1)).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal(1, result.FirstOrDefault().PatientDetails.SexId);
            }
        }

        [Fact]
        public async Task SearchBySex_NoResults()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterBySex(30)).GetResult().ToList();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task SearchByTBService_ReturnsMatchOnTBServiceCode()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByTBService("Ashford hospital")).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal("Ashford hospital", result.FirstOrDefault().HospitalDetails.TBServiceCode);
            }
        }

        [Fact]
        public async Task SearchByTBService_NoResults()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByTBService("NonexistentService")).GetResult().ToList();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task SearchByBirthCountry_ReturnsMatchOnCountryId()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByBirthCountry(1)).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal(1, result.FirstOrDefault().PatientDetails.CountryId);
            }
        }

        [Fact]
        public async Task SearchByBirthCountry_NoResults()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result = ((INtbsSearchBuilder)builder.FilterByBirthCountry(30)).GetResult().ToList();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task SearchByPartialNotificationDate_ReturnsMatchOnNotificationDate()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result =
                    ((INtbsSearchBuilder)builder.FilterByPartialNotificationDate(
                        new PartialDate() {Day = "1", Month = "1", Year = "2000"})).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal(new DateTime(2000, 1, 1), result.FirstOrDefault().NotificationDate);
            }
        }

        [Fact]
        public async Task SearchByPartialNotificationDate_NoResults()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result =
                    ((INtbsSearchBuilder)builder.FilterByPartialNotificationDate(
                        new PartialDate() {Day = "1", Month = "1", Year = "3000"})).GetResult().ToList();

                Assert.Empty(result);
            }
        }

        [Fact]
        public async Task SearchByPartialDob_ReturnsMatchOnDob()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result =
                    ((INtbsSearchBuilder)builder.FilterByPartialDob(new PartialDate()
                    {
                        Day = "1", Month = "1", Year = "1990"
                    })).GetResult().ToList();

                Assert.Single(result);
                Assert.Equal(new DateTime(1990, 1, 1), result.FirstOrDefault().PatientDetails.Dob);
            }
        }

        [Fact]
        public async Task SearchByPartialDob_NoResults()
        {
            using (var context = new NtbsContext(ContextOptions))
            {
                await context.Database.EnsureDeletedAsync();
                await context.Database.EnsureCreatedAsync();
                var builder = SetupTestDatabase(context);
                var result =
                    ((INtbsSearchBuilder)builder.FilterByPartialDob(new PartialDate()
                    {
                        Day = "1", Month = "1", Year = "3000"
                    })).GetResult().ToList();

                Assert.Empty(result);
            }
        }

        private INtbsSearchBuilder SetupTestDatabase(NtbsContext context)
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
            return new NtbsSearchBuilder(context.Notification);
        }
    }
}
