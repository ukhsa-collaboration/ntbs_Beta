using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Helpers;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;
using Serilog;

// ReSharper disable UseObjectOrCollectionInitializer
// We're not using object initialization syntax in this file, as it obscures errors caused by wrong date types
// on the way out from the dynamic objects

namespace ntbs_service.DataMigration
{
    public interface INotificationMapper
    {
        Task<IEnumerable<IEnumerable<Notification>>> GetNotificationsGroupedByPatient(List<string> notificationId);
        Task<IEnumerable<IEnumerable<Notification>>> GetNotificationsGroupedByPatient(DateTime rangeStartDate, DateTime endStartDate);
    }

    public class NotificationMapper : INotificationMapper
    {
        private readonly IMigrationRepository _migrationRepository;
        private readonly IReferenceDataRepository _referenceDataRepository;

        public NotificationMapper(IMigrationRepository migrationRepository, IReferenceDataRepository referenceDataRepository)
        {
            _migrationRepository = migrationRepository;
            _referenceDataRepository = referenceDataRepository;
        }

        public async Task<IEnumerable<IEnumerable<Notification>>> GetNotificationsGroupedByPatient(DateTime rangeStartDate, DateTime endStartDate)
        {
            var notificationsRaw = await _migrationRepository.GetNotificationsByDate(rangeStartDate, endStartDate);

            return await GetGroupedResultsAsNotificationAsync(notificationsRaw);
        }

        public async Task<IEnumerable<IEnumerable<Notification>>> GetNotificationsGroupedByPatient(List<string> notificationIds)
        {
            var notificationsRaw = await _migrationRepository.GetNotificationsById(notificationIds);

            return await GetGroupedResultsAsNotificationAsync(notificationsRaw);
        }

        private async Task<IEnumerable<IEnumerable<Notification>>> GetGroupedResultsAsNotificationAsync(IEnumerable<dynamic> notificationsRaw)
        {
            var diseaseSitesPerNotification = await GetDiseaseSitesPerNotification(notificationsRaw);
            var notifications = notificationsRaw.Join(diseaseSitesPerNotification,
                n => n.OldNotificationId,
                nsGroup => nsGroup.Key,
                (n, sites) => (rawNotification: n, rawSites: sites.Where(x => x != null && x.SiteId != null)));

            var notificationGroups = notifications.GroupBy(x => x.rawNotification.GroupId);

            return Task.WhenAll(notificationGroups
                .Select(group =>
                {
                    return Task.WhenAll(group.Select(AsNotificationAsync));
                })).Result;
        }

        private async Task<IEnumerable<IGrouping<string, dynamic>>> GetDiseaseSitesPerNotification(IEnumerable<dynamic> notificationsRaw)
        {
            var legacyIds = notificationsRaw.Select(x => x.OldNotificationId).Cast<string>();
            var notificationSitesRaw = await _migrationRepository.GetNotificationSites(legacyIds);
            return notificationSitesRaw.GroupBy(x => x.OldNotificationId as string);
        }

        private async Task<Notification> AsNotificationAsync((dynamic rawNotification, IEnumerable<dynamic> rawSites) rawResult)
        {
            var rawNotification = rawResult.rawNotification;
            var sites = AsSites(rawResult.rawSites);
            var notification = new Notification();
            notification.ETSID = rawNotification.Source == "ETS" ? rawNotification.OldNotificationId.ToString() : null;
            notification.LTBRID = rawNotification.Source == "LTBR" ? rawNotification.OldNotificationId.ToString() : null;
            notification.LTBRPatientId = rawNotification.Source == "LTBR" ? rawNotification.GroupId : null;
            notification.NotificationDate = rawNotification.NotificationDate;
            notification.CreationDate = DateTime.Now;
            notification.PatientDetails = ExtractPatientDetails(rawNotification);
            notification.ClinicalDetails = ExtractClinicalDetails(rawNotification);
            notification.TravelDetails = ExtractTravelDetails(rawNotification);
            notification.VisitorDetails = ExtractVisitorDetails(rawNotification);
            notification.ComorbidityDetails = ExtractComorbidityDetails(rawNotification);
            notification.ImmunosuppressionDetails = ExtractImmunosuppressionDetails(rawNotification);
            notification.SocialRiskFactors = ExtractSocialRiskFactors(rawNotification);
            notification.HospitalDetails = ExtractHospitalDetails(rawNotification);
            notification.NotificationStatus = NotificationStatus.Notified;
            notification.NotificationSites = sites;
            notification.TreatmentEvents = ExtractTreatmentEvents(rawNotification);

            if (notification.HospitalDetails.HospitalId is Guid guid)
            {
                var tbService = (await _referenceDataRepository.GetTbServiceFromHospitalIdAsync(guid));
                if (tbService == null)
                {
                    Log.Error($"No TB service exists for hospital with guid {guid}");
                }
                else
                {
                    // It's OK to only set this where it exists
                    // - the service missing will come up in notification validation  
                    notification.HospitalDetails.TBServiceCode = tbService.Code;
                }
            }

            return notification;
        }

        private static HospitalDetails ExtractHospitalDetails(dynamic rawNotification)
        {
            return new HospitalDetails
            {
                HospitalId = rawNotification.NtbsHospitalId,
                Consultant = rawNotification.Consultant
            };
        }

        private static List<NotificationSite> AsSites(IEnumerable<dynamic> rawResultRawSites)
        {
            return rawResultRawSites
                .Select(AsNotificationSite)
                // Due to many->one mapping of legacy sites to ntbs sites, there might be clashes we need to deal with..
                .GroupBy(site => site.SiteId)
                .Select(clashingSites =>
                {
                    // .. to avoid data loss, we collect the freetext entered in all the possible members of the clash
                    var combinedDescription = String.Join(", ", clashingSites.Select(site => site.SiteDescription));
                    var canonicalSite = clashingSites.First();
                    canonicalSite.SiteDescription = combinedDescription;
                    return canonicalSite;
                }).ToList();
        }

        private static NotificationSite AsNotificationSite(dynamic result)
        {
            if (result.SiteId == null)
            {
                return null;
            }
            return new NotificationSite
            {
                SiteId = result.SiteId,
                SiteDescription = result.SiteDescription
            };
        }

        private static ImmunosuppressionDetails ExtractImmunosuppressionDetails(dynamic notification)
        {
            var details = new ImmunosuppressionDetails();
            details.Status = StringToValueConverter.GetStatusFromString(notification.Status);
            details.HasBioTherapy = StringToValueConverter.GetBoolValue(notification.HasBioTherapy);
            details.HasTransplantation = StringToValueConverter.GetBoolValue(notification.HasTransplantation);
            details.HasOther = StringToValueConverter.GetBoolValue(notification.HasOther);
            details.OtherDescription = notification.OtherDescription;
            return details;
        }

        private static ComorbidityDetails ExtractComorbidityDetails(dynamic notification)
        {
            var details = new ComorbidityDetails();
            details.DiabetesStatus = StringToValueConverter.GetStatusFromString(notification.DiabetesStatus);
            details.LiverDiseaseStatus = StringToValueConverter.GetStatusFromString(notification.LiverDiseaseStatus);
            details.RenalDiseaseStatus = StringToValueConverter.GetStatusFromString(notification.RenalDiseaseStatus);
            details.HepatitisBStatus = StringToValueConverter.GetStatusFromString(notification.HepatitisBStatus);
            details.HepatitisCStatus = StringToValueConverter.GetStatusFromString(notification.HepatitisCStatus);
            return details;
        }

        private static ClinicalDetails ExtractClinicalDetails(dynamic notification)
        {
            var details = new ClinicalDetails();
            details.SymptomStartDate = notification.SymptomStartDate;
            details.FirstPresentationDate = notification.FirstPresentationDate;
            details.TBServicePresentationDate = notification.TbServicePresentationDate;
            details.DiagnosisDate = notification.DiagnosisDate ?? notification.StartOfTreatmentDate ?? notification.NotificationDate;
            details.DidNotStartTreatment = StringToValueConverter.GetNullableBoolValue(notification.DidNotStartTreatment);
            details.TreatmentStartDate = notification.StartOfTreatmentDate;
            details.MDRTreatmentStartDate = notification.MDRTreatmentStartDate;
            details.IsMDRTreatment = notification.IsMDRTreatment;
            details.IsSymptomatic = StringToValueConverter.GetNullableBoolValue(notification.IsSymptomatic);
            return details;
        }

        private static TravelDetails ExtractTravelDetails(dynamic notification)
        {
            bool? hasTravel = notification.HasTravel;
            var totalNumberOfCountries = hasTravel ?? false ? StringToValueConverter.ToNullableInt(notification.travel_TotalNumberOfCountries) : null;

            var details = new TravelDetails();
            details.HasTravel = hasTravel;
            details.TotalNumberOfCountries = totalNumberOfCountries;
            details.Country1Id = notification.travel_Country1;
            details.Country2Id = notification.travel_Country2;
            details.Country3Id = notification.travel_Country3;
            details.StayLengthInMonths1 = notification.travel_StayLengthInMonths1;
            details.StayLengthInMonths2 = notification.travel_StayLengthInMonths2;
            details.StayLengthInMonths3 = notification.travel_StayLengthInMonths3;
            return details;
        }

        private static VisitorDetails ExtractVisitorDetails(dynamic notification)
        {
            bool? hasVisitor = notification.HasVisitor;
            var totalNumberOfCountries = hasVisitor ?? false ? StringToValueConverter.ToNullableInt(notification.visitor_TotalNumberOfCountries) : null;

            var details = new VisitorDetails();
            details.HasVisitor = hasVisitor;
            details.TotalNumberOfCountries = totalNumberOfCountries;
            details.Country1Id = notification.visitor_Country1;
            details.Country2Id = notification.visitor_Country2;
            details.Country3Id = notification.visitor_Country3;
            details.StayLengthInMonths1 = StringToValueConverter.ToNullableInt(notification.visitor_StayLengthInMonths1);
            details.StayLengthInMonths2 = StringToValueConverter.ToNullableInt(notification.visitor_StayLengthInMonths2);
            details.StayLengthInMonths3 = StringToValueConverter.ToNullableInt(notification.visitor_StayLengthInMonths3);
            return details;
        }

        private static PatientDetails ExtractPatientDetails(dynamic notification)
        {
            var address = RemoveCharactersNotIn(
                ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
                notification.Line1 + " " + notification.Line2);
            var givenName = RemoveCharactersNotIn(ValidationRegexes.CharacterValidation, notification.GivenName);
            var familyName = RemoveCharactersNotIn(ValidationRegexes.CharacterValidation, notification.FamilyName);
            var localPatientId = RemoveCharactersNotIn(
                ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
                notification.LocalPatientId);

            var details = new PatientDetails();
            details.FamilyName = familyName;
            details.GivenName = givenName;
            details.NhsNumber = notification.NhsNumber;
            details.NhsNumberNotKnown = notification.NhsNumberNotKnown == 1 || notification.NhsNumber == null;
            details.Dob = notification.DateOfBirth;
            details.YearOfUkEntry = notification.UkEntryYear;
            details.UkBorn = notification.UkBorn;
            details.CountryId = notification.BirthCountryId ?? Countries.UnknownId;
            details.LocalPatientId = localPatientId;
            details.Postcode = notification.Postcode;
            details.NoFixedAbode = notification.NoFixedAbode == 1;
            details.Address = address;
            details.EthnicityId = notification.NtbsEthnicGroupId ?? Ethnicities.NotStatedId;
            details.SexId = notification.NtbsSexId ?? Sexes.UnknownId;
            details.OccupationId = notification.NtbsOccupationId;
            details.OccupationOther = notification.NtbsOccupationFreeText;

            ForceValidNhsNumber(details);
            
            return details;
        }

        private static string RemoveCharactersNotIn(string matchingRegex, string input)
        {
            // We assume the matching regex to be of the format "[someLettersHere]+"
            // and we are aiming to turn it into "[^someLettersHere]"
            var notMatchingRegex = matchingRegex
                .Remove(matchingRegex.Length - 1)
                .Insert(1, "^");
            return new Regex(notMatchingRegex).Replace(input, "");
        }

        private static void ForceValidNhsNumber(PatientDetails details)
        {
            var validationResults = new List<ValidationResult>();
            var context = new ValidationContext(details) { MemberName = nameof(PatientDetails.NhsNumber)};
            Validator.TryValidateProperty(details.NhsNumber, context, validationResults);
            if (validationResults.Any())
            {
                details.NhsNumber = null;
                details.NhsNumberNotKnown = true;
            }
        }

        private static SocialRiskFactors ExtractSocialRiskFactors(dynamic notification)
        {
            var factors = new SocialRiskFactors();
            factors.AlcoholMisuseStatus = StringToValueConverter.GetStatusFromString(notification.AlcoholMisuseStatus);
            factors.SmokingStatus = StringToValueConverter.GetStatusFromString(notification.SmokingStatus);
            factors.MentalHealthStatus = StringToValueConverter.GetStatusFromString(notification.MentalHealthStatus);
            factors.AsylumSeekerStatus = StringToValueConverter.GetStatusFromString(notification.AsylumSeekerStatus);
            factors.ImmigrationDetaineeStatus = StringToValueConverter.GetStatusFromString(notification.ImmigrationDetaineeStatus);
            
            factors.RiskFactorDrugs.Status = StringToValueConverter.GetStatusFromString(notification.riskFactorDrugs_Status);
            factors.RiskFactorDrugs.IsCurrent = StringToValueConverter.GetBoolValue(notification.riskFactorDrugs_IsCurrent);
            factors.RiskFactorDrugs.InPastFiveYears = StringToValueConverter.GetBoolValue(notification.riskFactorDrugs_InPastFiveYears);
            factors.RiskFactorDrugs.MoreThanFiveYearsAgo = StringToValueConverter.GetBoolValue(notification.riskFactorDrugs_MoreThanFiveYearsAgo);
            
            factors.RiskFactorHomelessness.Status = StringToValueConverter.GetStatusFromString(notification.riskFactorHomelessness_Status);
            factors.RiskFactorHomelessness.IsCurrent = StringToValueConverter.GetBoolValue(notification.riskFactorHomelessness_IsCurrent);
            factors.RiskFactorHomelessness.InPastFiveYears = StringToValueConverter.GetBoolValue(notification.riskFactorHomelessness_InPastFiveYears);
            factors.RiskFactorHomelessness.MoreThanFiveYearsAgo = StringToValueConverter.GetBoolValue(notification.riskFactorHomelessness_MoreThanFiveYearsAgo);
            
            factors.RiskFactorImprisonment.Status = StringToValueConverter.GetStatusFromString(notification.riskFactorImprisonment_Status);
            factors.RiskFactorImprisonment.IsCurrent = StringToValueConverter.GetBoolValue(notification.riskFactorImprisonment_IsCurrent);
            factors.RiskFactorImprisonment.InPastFiveYears = StringToValueConverter.GetBoolValue(notification.riskFactorImprisonment_InPastFiveYears);
            factors.RiskFactorImprisonment.MoreThanFiveYearsAgo = StringToValueConverter.GetBoolValue(notification.riskFactorImprisonment_MoreThanFiveYearsAgo);
            return factors;
        }
        
         private static List<TreatmentEvent> ExtractTreatmentEvents(dynamic notification)
         {
             var treatmentEvents = new List<TreatmentEvent> {new TreatmentEvent
             {
                 EventDate = notification.DeathDate,
                 TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                 TreatmentOutcome = new TreatmentOutcome
                 {
                     TreatmentOutcomeId = 10,
                     TreatmentOutcomeType = TreatmentOutcomeType.Died,
                     TreatmentOutcomeSubType = TreatmentOutcomeSubType.Unknown
                 }
             }};
            return treatmentEvents;
        }
    }
}
