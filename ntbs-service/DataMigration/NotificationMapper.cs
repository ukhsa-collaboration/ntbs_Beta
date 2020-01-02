using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;
using ntbs_service.Models.Entities;
using ntbs_service.Models.ReferenceEntities;
using Dapper;
using Microsoft.Extensions.Configuration;
using ntbs_service.Models.Enums;
using ntbs_service.Helpers;
using ntbs_service.DataAccess;

namespace ntbs_service.DataMigration
{
    public interface INotificationMapper
    {
        Task<IEnumerable<IEnumerable<Notification>>> GetNotificationsGroupedByPatient(List<string> notificationId);
        Task<IEnumerable<IEnumerable<Notification>>> GetNotificationsGroupedByPatient(DateTime rangeStartDate, DateTime endStartDate);
    }

    public class NotificationMapper : INotificationMapper
    {
        private readonly string connectionString;
        private readonly IMigrationRepository _migrationRepository;
        private readonly IReferenceDataRepository _referenceDataRepository;

        public NotificationMapper(IConfiguration configuration, IMigrationRepository migrationRepository, IReferenceDataRepository referenceDataRepository)
        {
            connectionString = configuration.GetConnectionString("migration");
            _migrationRepository = migrationRepository;
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<IEnumerable<IEnumerable<Notification>>> GetNotificationsGroupedByPatient(DateTime rangeStartDate, DateTime endStartDate)
        {
            var notificationsRaw = await _migrationRepository.GetNotificationsByDate(rangeStartDate, endStartDate);
            var legacyIds = notificationsRaw.Select(x => x.OldNotificationId).Cast<string>();
            var notificationSitesRaw = await _migrationRepository.GetNotificationSites(legacyIds);

            return GetGroupedResultsAsNotification(notificationsRaw, notificationSitesRaw);
        }

        public async Task<IEnumerable<IEnumerable<Notification>>> GetNotificationsGroupedByPatient(List<string> notificationIds)
        {
            var notificationsRaw = await _migrationRepository.GetNotificationsById(notificationIds);
            var legacyIds = notificationsRaw.Select(x => x.OldNotificationId).Cast<string>();
            var notificationSitesRaw = await _migrationRepository.GetNotificationSites(legacyIds);

            return GetGroupedResultsAsNotification(notificationsRaw, notificationSitesRaw);
        }

        private IEnumerable<IEnumerable<Notification>> GetGroupedResultsAsNotification(IEnumerable<dynamic> notificationsRaw, IEnumerable<dynamic> notificationSitesRaw)
        {
            var notificationSiteGroups = notificationSitesRaw.GroupBy(x => x.OldNotificationId);
            var notifications = notificationsRaw.Join(notificationSiteGroups,
                n => n.OldNotificationId,
                nsGroup => nsGroup.Key,
                (n, nsGroup) => (rawNotification: n, rawSites: nsGroup.Where(x => x != null && x.DiseaseSiteId != null)));

            var notificationGroups = notifications.GroupBy(x => x.rawNotification.GroupId);

            return Task.WhenAll(notificationGroups
                .Select(group =>
                {
                    return Task.WhenAll(group.Select(AsNotificationAsync));
                })).Result;
        }

        private async Task<Notification> AsNotificationAsync((dynamic rawNotification, IEnumerable<dynamic> rawSites) rawResult)
        {
            var rawNotification = rawResult.rawNotification;
            var sites = rawResult.rawSites.Select(AsNotificationSite).ToList();
            var notification = new Notification
            {
                ETSID = rawNotification.Source == "ETS" ? rawNotification.OldNotificationId.ToString() : null,
                LTBRID = rawNotification.Source == "LTBR" ? rawNotification.OldNotificationId.ToString() : null,
                LTBRPatientId = rawNotification.Source == "LTBR" ? rawNotification.GroupId : null,
                NotificationDate = rawNotification.NotificationDate,
                CreationDate = rawNotification.CreationDate,
                PatientDetails = ExtractPatientDetails(rawNotification),
                ClinicalDetails = ExtractClinicalDetails(rawNotification),
                TravelDetails = ExtractTravelDetails(rawNotification),
                VisitorDetails = ExtractVisitorDetails(rawNotification),
                ComorbidityDetails = ExtractComorbidityDetails(rawNotification),
                ImmunosuppressionDetails = ExtractImmunosuppressionDetails(rawNotification),
                Episode = ExtractEpisodeDetails(rawNotification),
                NotificationStatus = NotificationStatus.Notified,
                NotificationSites = sites
            };

            if (notification.Episode.HospitalId is Guid guid)
            {
                var tbService = (await _referenceDataRepository.GetTbServiceFromHospitalIdAsync(guid));
                notification.Episode.TBServiceCode = tbService.Code;
            }

            return notification;
        }

        private static Episode ExtractEpisodeDetails(dynamic rawNotification)
        {
            return new Episode
            {
                HospitalId = rawNotification.NtbsHospitalId,
                Consultant = rawNotification.Consultant
            };
        }

        private static NotificationSite AsNotificationSite(dynamic result)
        {
            if (result.DiseaseSiteId == null)
            {
                return null;
            }
            return new NotificationSite
            {
                SiteId = result.DiseaseSiteId,
                SiteDescription = result.DiseaseSiteText
            };
        }

        private static ImmunosuppressionDetails ExtractImmunosuppressionDetails(dynamic notification) => new ImmunosuppressionDetails
        {
            Status = StringToValueConverter.GetStatusFromString(notification.Status),
            HasBioTherapy = StringToValueConverter.GetBoolValue(notification.HasBioTherapy),
            HasTransplantation = StringToValueConverter.GetBoolValue(notification.HasTransplantation),
            HasOther = StringToValueConverter.GetBoolValue(notification.HasOther),
            OtherDescription = notification.OtherDescription
        };

        private static ComorbidityDetails ExtractComorbidityDetails(dynamic notification) => new ComorbidityDetails
        {
            DiabetesStatus = StringToValueConverter.GetStatusFromString(notification.DiabetesStatus),
            LiverDiseaseStatus = StringToValueConverter.GetStatusFromString(notification.LiverDiseaseStatus),
            RenalDiseaseStatus = StringToValueConverter.GetStatusFromString(notification.RenalDiseaseStatus),
            HepatitisBStatus = StringToValueConverter.GetStatusFromString(notification.HepatitisBStatus),
            HepatitisCStatus = StringToValueConverter.GetStatusFromString(notification.HepatitisCStatus)
        };

        private static ClinicalDetails ExtractClinicalDetails(dynamic notification) => new ClinicalDetails
        {
            SymptomStartDate = notification.SymptomStartDate,
            FirstPresentationDate = notification.FirstPresentationDate,
            TBServicePresentationDate = notification.TbServicePresentationDate,
            DiagnosisDate = notification.DiagnosisDate,
            DidNotStartTreatment = StringToValueConverter.GetNullableBoolValue(notification.DidNotStartTreatment),
            MDRTreatmentStartDate = notification.StartOfTreatmentDay,
            IsSymptomatic = StringToValueConverter.GetNullableBoolValue(notification.IsSymptomatic),
            DeathDate = notification.DeathDate
        };

        private static TravelDetails ExtractTravelDetails(dynamic notification) => new TravelDetails
        {
            HasTravel = StringToValueConverter.GetNullableBoolValue(notification.HasTravel),
            TotalNumberOfCountries = StringToValueConverter.ToNullableInt(notification.travel_TotalNumberOfCountries),
            Country1Id = notification.travel_Country1,
            Country2Id = notification.travel_Country2,
            Country3Id = notification.travel_Country3,
            StayLengthInMonths1 = notification.StayLengthInMonths1,
            StayLengthInMonths2 = notification.StayLengthInMonths2,
            StayLengthInMonths3 = notification.StayLengthInMonths3
        };

        private static VisitorDetails ExtractVisitorDetails(dynamic notification) => new VisitorDetails
        {
            HasVisitor = StringToValueConverter.GetNullableBoolValue(notification.HasVisitor),
            TotalNumberOfCountries = StringToValueConverter.ToNullableInt(notification.visitor_TotalNumberOfCountries),
            Country1Id = notification.visitor_Country1,
            Country2Id = notification.visitor_Country2,
            Country3Id = notification.visitor_Country3,
            StayLengthInMonths1 = notification.visitor_StayLengthInMonths1,
            StayLengthInMonths2 = notification.visitor_StayLengthInMonths2,
            StayLengthInMonths3 = notification.visitor_StayLengthInMonths3
        };

        private static PatientDetails ExtractPatientDetails(dynamic notification) => new PatientDetails
        {
            FamilyName = notification.FamilyName,
            GivenName = notification.GivenName,
            NhsNumber = notification.NhsNumber,
            Dob = notification.DateOfBirth,
            YearOfUkEntry = notification.UkEntryYear,
            UkBorn = StringToValueConverter.GetNullableBoolValue(notification.UkBorn),
            CountryId = notification.BirthCountryId,
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
}