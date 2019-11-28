using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.Models;
using Dapper;
using Microsoft.Extensions.Configuration;

namespace ntbs_service.DataMigration
{
    public interface INotificationSearcher
    {
        Task<IEnumerable<Notification>> Search(string notificationId);
    }

    public class NotificationSearcher : INotificationSearcher
    {
        const string Query = @"
            SELECT *
            FROM Notifications n 
            LEFT JOIN Patients p ON p.OldNotificationId = n.OldNotificationId
            LEFT JOIN Addresses addrs ON addrs.OldNotificationId = n.OldNotificationId
            LEFT JOIN Demographics d ON d.OldNotificationId = n.OldNotificationId
            LEFT JOIN DeathDates dd ON dd.OldNotificationId = n.OldNotificationId
            LEFT JOIN VisitorHistory vh ON vh.OldNotificationId = n.OldNotificationId
            LEFT JOIN TravelHistory th ON th.OldNotificationId = n.OldNotificationId
            LEFT JOIN ClinicalDates cd ON cd.OldNotificationId = n.OldNotificationId
            WHERE GroupId IN (
                SELECT GroupId
                FROM Notifications n WHERE n.OldNotificationId = @NotificationId
            )";

        private readonly string connectionString;

        public NotificationSearcher(IConfiguration _configuration)
        {
            connectionString = _configuration.GetConnectionString("migration");
        }

        public async Task<IEnumerable<Notification>> Search(string notificationId)
        {
            using (var connection = new SqlConnection(connectionString))
            {
                connection.Open();

                var results = await connection.QueryAsync(Query, new
                {
                    NotificationId = $"{notificationId}"
                });

                return results.Select(AsNotification);
            }
        }

        private static Notification AsNotification(dynamic result)
        {
            var notification = new Notification
            {
                ETSID = result.ETSID?.ToString(),
                LTBRID = result.LTBRID?.ToString(),
                NotificationDate = result.NotificationDate,
                CreationDate = result.CreationDate,
                PatientDetails = ExtractPatientDetails(result),
                ClinicalDetails = ExtractClinicalDetails(result),
                TravelDetails = ExtractTravelDetails(result),
                VisitorDetails = ExtractVisitorDetails(result)
            };

            return notification;
        }

        private static ClinicalDetails ExtractClinicalDetails(dynamic notification)
        {
            return new ClinicalDetails
                {
                    SymptomStartDate = notification.SymptomStartDate,
                    FirstPresentationDate = notification.FirstPresentationDate,
                    TBServicePresentationDate = notification.TbServicePresentationDate,
                    DiagnosisDate = notification.DiagnosisDate,
                    DidNotStartTreatment = GetBoolValue(notification.DidNotStartTreatment),
                    MDRTreatmentStartDate = notification.StartOfTreatmentDay,
                    IsSymptomatic = GetBoolValue(notification.IsSymptomatic),
                    DeathDate = notification.DeathDate
                };
        }

        private static TravelDetails ExtractTravelDetails(dynamic notification)
        {
            return new TravelDetails
                {
                    HasTravel = GetBoolValue(notification.HasTravel),
                    TotalNumberOfCountries = notification.TotalNumberOfCountries,
                    Country1Id = notification.Country1,
                    Country2Id = notification.Country2,
                    Country3Id = notification.Country3,
                    StayLengthInMonths1 = notification.StayLengthInMonths1,                    
                    StayLengthInMonths2 = notification.StayLengthInMonths2,
                    StayLengthInMonths3 = notification.StayLengthInMonths3
                };
        }

        private static VisitorDetails ExtractVisitorDetails(dynamic notification)
        {
            return new VisitorDetails
                {
                    HasVisitor = GetBoolValue(notification.HasVisitor),
                    TotalNumberOfCountries = notification.TotalNumberOfCountries,
                    Country1Id = notification.Country1,
                    Country2Id = notification.Country2,
                    Country3Id = notification.Country3,
                    StayLengthInMonths1 = notification.StayLengthInMonths1,
                    StayLengthInMonths2 = notification.StayLengthInMonths2,
                    StayLengthInMonths3 = notification.StayLengthInMonths3
                };
        }

        private static PatientDetails ExtractPatientDetails(dynamic notification)
        {
            return new PatientDetails
                {
                    FamilyName = notification.FamilyName,
                    GivenName = notification.GivenName,
                    NhsNumber = notification.NhsNumber,
                    Dob = notification.DateOfBirth,
                    YearOfUkEntry = notification.UkEntryYear,
                    UkBorn = GetBoolValue(notification.UkBorn),
                    LocalPatientId = notification.LocalPatientId,
                    Postcode = notification.Postcode,
                    Address = notification.Line1 + " " + notification.Line2,
                    EthnicityId = notification.NtbsEthnicGroupId,
                    SexId = notification.NtbsSexId,
                    NhsNumberNotKnown = notification.NhsNumberNotKnown == 1,
                    NoFixedAbode = notification.NoFixedAbode == 1,
                    OccupationId = notification.NtbsOccupationId,
                    OccupationOther = notification.NtbsOccupationFreeText
                };
        }

        private static bool? GetBoolValue(int? value)
        {
            if (value == null)
            {
                return null;
            }
            return value == 1 ? true : false;
        }
    }
  
}