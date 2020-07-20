using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Hangfire.Server;
using MoreLinq;
using ntbs_service.DataAccess;
using ntbs_service.DataMigration.RawModels;
using ntbs_service.Helpers;
using ntbs_service.Models;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;
using ntbs_service.Services;
using Serilog;
using Countries = ntbs_service.Models.Countries;

// ReSharper disable UseObjectOrCollectionInitializer
// We're not using object initialization syntax in this file, as it obscures errors caused by wrong date types
// on the way out from the dynamic objects

namespace ntbs_service.DataMigration
{
    public interface INotificationMapper
    {
        Task<IEnumerable<IList<Notification>>> GetNotificationsGroupedByPatient(PerformContext context,
            string requestId, List<string> notificationId);
        Task<IEnumerable<IList<Notification>>> GetNotificationsGroupedByPatient(PerformContext context,
            string requestId, DateTime rangeStartDate, DateTime endStartDate);
    }

    public class NotificationMapper : INotificationMapper
    {
        private readonly IMigrationRepository _migrationRepository;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IImportLogger _logger;
        private readonly TreatmentOutcome _postMortemOutcomeType;
        private readonly IPostcodeService _postcodeService;

        public NotificationMapper(IMigrationRepository migrationRepository,
            IReferenceDataRepository referenceDataRepository,
            IImportLogger logger,
            IPostcodeService postcodeService)
        {
            _migrationRepository = migrationRepository;
            _referenceDataRepository = referenceDataRepository;
            _logger = logger;
            _postcodeService = postcodeService;

            // This is a database-based value, but static from the runtime point of view, so we fetch it once here.
            _postMortemOutcomeType = _referenceDataRepository.GetTreatmentOutcomeForTypeAndSubType(
                TreatmentOutcomeType.Died,
                TreatmentOutcomeSubType.Unknown).Result;
        }

        public async Task<IEnumerable<IList<Notification>>> GetNotificationsGroupedByPatient(PerformContext context,
            string requestId, DateTime rangeStartDate, DateTime endStartDate)
        {
            var groupedIds = await _migrationRepository.GetGroupedNotificationIdsByDate(rangeStartDate, endStartDate);

            return await GetGroupedResultsAsNotificationAsync(groupedIds, context, requestId);
        }

        public async Task<IEnumerable<IList<Notification>>> GetNotificationsGroupedByPatient(PerformContext context,
            string requestId, List<string> notificationIds)
        {
            var groupedIds = await _migrationRepository.GetGroupedNotificationIdsById(notificationIds);

            return await GetGroupedResultsAsNotificationAsync(groupedIds, context, requestId);
        }

        private async Task<IEnumerable<IList<Notification>>> GetGroupedResultsAsNotificationAsync(IEnumerable<IGrouping<string, string>> groupedIds, PerformContext context, string requestId)
        {
            var resultList = new List<IList<Notification>>();
            foreach (var group in groupedIds)
            {
                var legacyIds = group.ToList();
                _logger.LogInformation(context, requestId, $"Fetching data for legacy notifications {string.Join(", ", legacyIds)}");
                var legacyNotifications = _migrationRepository.GetNotificationsById(legacyIds);
                var sitesOfDisease = _migrationRepository.GetNotificationSites(legacyIds);
                var manualTestResults = _migrationRepository.GetManualTestResults(legacyIds);
                var socialContextVenues = _migrationRepository.GetSocialContextVenues(legacyIds);
                var socialContextAddresses = _migrationRepository.GetSocialContextAddresses(legacyIds);
                var transferEvents =  _migrationRepository.GetTransferEvents(legacyIds);
                var outcomeEvents = _migrationRepository.GetOutcomeEvents(legacyIds);
                var mbovisAnimalExposure = _migrationRepository.GetMigrationMBovisAnimalExposure(legacyIds);
                var mbovisExposureToKnownCase = _migrationRepository.GetMigrationMBovisExposureToKnownCase(legacyIds);
                var mbovisOccupationExposures = _migrationRepository.GetMigrationMBovisOccupationExposures(legacyIds);
                var mbovisUnpasteurisedMilkConsumption = _migrationRepository.GetMigrationMBovisUnpasteurisedMilkConsumption(legacyIds);

                var notificationsForGroup = await CombineDataForGroup(
                    legacyIds,
                    (await legacyNotifications).ToList(),
                    (await sitesOfDisease).ToList(),
                    (await manualTestResults).ToList(),
                    (await socialContextVenues).ToList(),
                    (await socialContextAddresses).ToList(),
                    (await transferEvents).ToList(),
                    (await outcomeEvents).ToList(),
                    (await mbovisAnimalExposure).ToList(),
                    (await mbovisExposureToKnownCase).ToList(),
                    (await mbovisOccupationExposures).ToList(),
                    (await mbovisUnpasteurisedMilkConsumption).ToList()
                );
                resultList.Add(notificationsForGroup);
            }
            return resultList;
        }

        private async Task<IList<Notification>> CombineDataForGroup(IEnumerable<string> legacyIds,
            List<MigrationDbNotification> notifications,
            List<MigrationDbSite> sitesOfDisease,
            List<MigrationDbManualTest> manualTestResults,
            List<MigrationDbSocialContextVenue> socialContextVenues,
            List<MigrationDbSocialContextAddress> socialContextAddresses,
            List<MigrationDbTransferEvent> transferEvents,
            List<MigrationDbOutcomeEvent> outcomeEvents,
            List<MigrationDbMBovisAnimal> mbovisAnimalExposures,
            List<MigrationDbMBovisKnownCase> mbovisExposureToKnownCase,
            List<MigrationDbMBovisOccupation> mbovisOccupationExposures,
            List<MigrationDbMBovisMilkConsumption> mbovisUnpasteurisedMilkConsumption)
        {
            return await Task.WhenAll(legacyIds.Select(async id =>
            {
                var rawNotification = notifications.Single(n => n.OldNotificationId == id);
                var notificationSites = AsSites(sitesOfDisease
                    .Where(s => s.OldNotificationId == id)
                );
                var notificationTestResults = manualTestResults
                    .Where(t => t.OldNotificationId == id)
                    .Select(AsManualTestResult)
                    .ToList();
                var notificationSocialContextVenues = socialContextVenues
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsSocialContextVenue)
                    .ToList();
                var notificationSocialContextAddresses = socialContextAddresses
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsSocialContextAddress)
                    .ToList();
                var notificationTransferEvents = await Task.WhenAll(transferEvents
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsTransferEvent)
                    .ToList()
                );
                var notificationOutcomeEvents = await Task.WhenAll(outcomeEvents
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsOutcomeEvent)
                    .ToList()
                );
                var notificationMBovisAnimalExposures = mbovisAnimalExposures
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsMBovisAnimalExposure)
                    .ToList();
                var notificationMBovisExposureToKnownCase = mbovisExposureToKnownCase
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsMBovisExposureToKnownCase)
                    .ToList();
                var notificationMBovisOccupationExposures = mbovisOccupationExposures
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsMBovisOccupationExposure)
                    .ToList();
                var notificationMBovisUnpasteurisedMilkConsumption = mbovisUnpasteurisedMilkConsumption
                    .Where(sc => sc.OldNotificationId == id)
                    .Select(AsMBovisUnpasteurisedMilkConsumption)
                    .ToList();                

                Notification notification = await AsNotificationAsync(rawNotification);
                notification.NotificationSites = notificationSites;
                notification.TestData = new TestData
                {
                    HasTestCarriedOut = notificationTestResults.Any(), ManualTestResults = notificationTestResults
                };
                notification.SocialContextAddresses = notificationSocialContextAddresses;
                notification.SocialContextVenues = notificationSocialContextVenues;
                notification.TreatmentEvents = CombineTreatmentEvents(notification,
                    rawNotification,
                    notificationTransferEvents,
                    notificationOutcomeEvents);
                notification.MBovisDetails.HasAnimalExposure = notificationMBovisAnimalExposures.Any();
                notification.MBovisDetails.MBovisAnimalExposures = notificationMBovisAnimalExposures;
                notification.MBovisDetails.HasExposureToKnownCases = notificationMBovisExposureToKnownCase.Any();
                notification.MBovisDetails.MBovisExposureToKnownCases = notificationMBovisExposureToKnownCase;
                notification.MBovisDetails.HasOccupationExposure = notificationMBovisOccupationExposures.Any();
                notification.MBovisDetails.MBovisOccupationExposures = notificationMBovisOccupationExposures;
                notification.MBovisDetails.HasUnpasteurisedMilkConsumption =
                    notificationMBovisUnpasteurisedMilkConsumption.Any();
                notification.MBovisDetails.MBovisUnpasteurisedMilkConsumptions =
                    notificationMBovisUnpasteurisedMilkConsumption;
                return notification;
            }));
        }

        private List<TreatmentEvent> CombineTreatmentEvents(Notification notification,
            MigrationDbNotification rawNotification,
            IEnumerable<TreatmentEvent> notificationTransferEvents,
            IEnumerable<TreatmentEvent> notificationOutcomeEvents)
        {

            // For post mortem cases the death event is the ONLY event we want to import so the final outcome is 
            // correctly reported.
            if (notification.ClinicalDetails.IsPostMortem == true)
            {
                return new List<TreatmentEvent>
                {
                    new TreatmentEvent
                    {
                        EventDate = rawNotification.DeathDate,
                        TreatmentEventType = TreatmentEventType.TreatmentOutcome,
                        TreatmentOutcomeId = _postMortemOutcomeType.TreatmentOutcomeId,
                        TreatmentOutcome = _postMortemOutcomeType
                    }
                };
            }

            var treatmentEvents = new List<TreatmentEvent>();
            if (notification.NotificationStatus != NotificationStatus.Draft)
            {
                treatmentEvents.Add(NotificationHelper.CreateTreatmentStartEvent(notification));
            }

            treatmentEvents.AddRange(notificationTransferEvents);
            treatmentEvents.AddRange(notificationOutcomeEvents);
            return treatmentEvents;
        }

        private async Task<Notification> AsNotificationAsync(MigrationDbNotification rawNotification)
        {
            var notification = new Notification();
            notification.ETSID = rawNotification.EtsId;
            notification.LTBRID = rawNotification.LtbrId;
            notification.LTBRPatientId = rawNotification.Source == "LTBR" ? rawNotification.GroupId : null;
            notification.NotificationDate = rawNotification.NotificationDate;
            notification.CreationDate = DateTime.Now;
            if (Converter.GetNullableBoolValue(rawNotification.IsDenotified) == true)
            {
                notification.NotificationStatus = NotificationStatus.Denotified;
                notification.DenotificationDetails = new DenotificationDetails
                {
                    DateOfDenotification = rawNotification.DenotificationDate ?? DateTime.Now,
                    Reason = DenotificationReason.Other,
                    OtherDescription = "Denotified in legacy system, with denotification date " +
                                       (rawNotification.DenotificationDate?.ToString() ?? "missing")
                };
            }
            else
            {
                notification.NotificationStatus = NotificationStatus.Notified;
            }
            
            notification.PatientDetails = await ExtractPatientDetails(rawNotification);
            notification.ClinicalDetails = ExtractClinicalDetails(rawNotification);
            notification.TravelDetails = ExtractTravelDetails(rawNotification);
            notification.VisitorDetails = ExtractVisitorDetails(rawNotification);
            notification.ComorbidityDetails = ExtractComorbidityDetails(rawNotification);
            notification.ImmunosuppressionDetails = ExtractImmunosuppressionDetails(rawNotification);
            notification.SocialRiskFactors = ExtractSocialRiskFactors(rawNotification);
            notification.HospitalDetails = await ExtractHospitalDetailsAsync(rawNotification);
            notification.ContactTracing = ExtractContactTracingDetails(rawNotification);
            notification.PreviousTbHistory = ExtractPreviousTbHistory(rawNotification);
            notification.MDRDetails = ExtractMdrDetailsAsync(rawNotification);

            return notification;
        }

        private async Task<HospitalDetails> ExtractHospitalDetailsAsync(MigrationDbNotification rawNotification)
        {
            var details = new HospitalDetails();
            details.HospitalId = rawNotification.NtbsHospitalId;
            details.CaseManagerUsername = rawNotification.CaseManager;
            var consultant = RemoveCharactersNotIn(ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended, rawNotification.Consultant);
            details.Consultant = consultant;
            // details.TBServiceCode is set below, based on the hospital

            if (details.HospitalId is Guid guid)
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
                    details.TBServiceCode = tbService.Code;
                }
            }
            
            // we are not doing the same check to case manager here, because leaving it empty would make it pass
            // validation - it is not a mandatory field. we don't want to lose it where it exists, so that check
            // is explicitly done during the validation step

            return details;
        }

        private static List<NotificationSite> AsSites(IEnumerable<MigrationDbSite> rawResultRawSites)
        {
            return rawResultRawSites
                .Select(AsNotificationSite)
                // Due to many->one mapping of legacy sites to ntbs sites, there might be clashes we need to deal with..
                .GroupBy(site => site.SiteId)
                .Select(clashingSites =>
                {
                    // .. to avoid data loss, we collect the free text entered in all the possible members of the clash
                    var combinedDescription = String.Join(", ", clashingSites.Select(site => site.SiteDescription));
                    var canonicalSite = clashingSites.First();
                    canonicalSite.SiteDescription = combinedDescription;
                    return canonicalSite;
                })
                .FallbackIfEmpty(new NotificationSite
                {
                    SiteId = (int)SiteId.OTHER,
                    SiteDescription = "Unknown, no site specified in legacy system"
                })
                .ToList();
        }

        private static NotificationSite AsNotificationSite(MigrationDbSite result)
        {
            if (result.SiteId == null)
            {
                return null;
            }

            var site = new NotificationSite();
            site.SiteId = (int) result.SiteId;
            site.SiteDescription = result.SiteDescription;
            return site;
        }

        private static ImmunosuppressionDetails ExtractImmunosuppressionDetails(MigrationDbNotification notification)
        {
            var details = new ImmunosuppressionDetails();
            details.Status = Converter.GetStatusFromString(notification.ImmunosuppressionStatus);
            if (details.Status != Status.Yes)
            {
                return details;
            }

            details.HasBioTherapy = Converter.GetNullableBoolValue(notification.HasBioTherapy);
            details.HasTransplantation = Converter.GetNullableBoolValue(notification.HasTransplantation);
            details.HasOther = Converter.GetNullableBoolValue(notification.HasOther);

            if (details.HasBioTherapy != true && details.HasTransplantation != true && details.HasOther != true)
            {
                details.HasOther = true;
                details.OtherDescription = "No immunosuppression type was provided in the legacy record";
            }
            else if (details.HasOther == true)
            {
                if (!string.IsNullOrWhiteSpace(notification.OtherDescription))
                {
                    var otherDescription = RemoveCharactersNotIn(
                        ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
                        notification.OtherDescription);
                    details.OtherDescription = otherDescription;
                }
                else
                {
                    details.OtherDescription = "No description provided in the legacy system";
                }
            }

            // Ensure that if a description exists, HasOther is ticked
            if (details.HasOther != true && !string.IsNullOrWhiteSpace(details.OtherDescription))
                details.HasOther = true;

            return details;
        }

        private static ComorbidityDetails ExtractComorbidityDetails(MigrationDbNotification notification)
        {
            var details = new ComorbidityDetails();
            details.DiabetesStatus = Converter.GetStatusFromString(notification.DiabetesStatus);
            details.LiverDiseaseStatus = Converter.GetStatusFromString(notification.LiverDiseaseStatus);
            details.RenalDiseaseStatus = Converter.GetStatusFromString(notification.RenalDiseaseStatus);
            details.HepatitisBStatus = Converter.GetStatusFromString(notification.HepatitisBStatus);
            details.HepatitisCStatus = Converter.GetStatusFromString(notification.HepatitisCStatus);
            return details;
        }

        private static ClinicalDetails ExtractClinicalDetails(MigrationDbNotification notification)
        {
            var details = new ClinicalDetails();
            details.SymptomStartDate = notification.SymptomOnsetDate;
            details.FirstPresentationDate = notification.FirstPresentationDate;
            details.TBServicePresentationDate = notification.TbServicePresentationDate;
            details.DiagnosisDate = notification.DiagnosisDate ?? notification.StartOfTreatmentDate ?? notification.NotificationDate;
            details.DidNotStartTreatment = Converter.GetNullableBoolValue(notification.DidNotStartTreatment);
            details.TreatmentStartDate = notification.StartOfTreatmentDate;
            details.MDRTreatmentStartDate = notification.MDRTreatmentStartDate;
            details.IsSymptomatic = Converter.GetNullableBoolValue(notification.IsSymptomatic);
            details.IsPostMortem = Converter.GetNullableBoolValue(notification.IsPostMortem);
            details.HIVTestState = Converter.GetEnumValue<HIVTestStatus>(notification.HivTestStatus);
            details.IsDotOffered = Converter.GetStatusFromString(notification.IsDotOffered);
            details.DotStatus = Converter.GetEnumValue<DotStatus>(notification.DotStatus);
            details.EnhancedCaseManagementStatus = Converter.GetStatusFromString(notification.EnhancedCaseManagementStatus);
            details.BCGVaccinationState = Converter.GetStatusFromString(notification.BCGVaccinationState);
            details.BCGVaccinationYear = notification.BCGVaccinationYear;
            details.TreatmentRegimen = Converter.GetEnumValue<TreatmentRegimen>(notification.TreatmentRegimen);
            details.Notes = notification.Notes;
            return details;
        }

        private ContactTracing ExtractContactTracingDetails(MigrationDbNotification rawNotification)
        {
            var details = new ContactTracing();
            details.AdultsIdentified = rawNotification.AdultsIdentified;
            details.ChildrenIdentified = rawNotification.ChildrenIdentified;
            details.AdultsScreened = rawNotification.AdultsScreened;
            details.ChildrenScreened = rawNotification.ChildrenScreened;
            details.AdultsActiveTB = rawNotification.AdultsActiveTB;
            details.ChildrenActiveTB = rawNotification.ChildrenActiveTB;
            details.AdultsLatentTB = rawNotification.AdultsLatentTB;
            details.ChildrenLatentTB = rawNotification.ChildrenLatentTB;
            details.AdultsStartedTreatment = rawNotification.AdultsStartedTreatment;
            details.ChildrenStartedTreatment = rawNotification.ChildrenStartedTreatment;
            details.AdultsFinishedTreatment = rawNotification.AdultsFinishedTreatment;
            details.ChildrenFinishedTreatment = rawNotification.ChildrenFinishedTreatment;
            return details;
        }

        private PreviousTbHistory ExtractPreviousTbHistory(MigrationDbNotification rawNotification)
        {
            var details = new PreviousTbHistory();
            details.PreviouslyHadTb = Converter.GetStatusFromString(rawNotification.PreviouslyHadTb);
            details.PreviousTbDiagnosisYear = rawNotification.PreviousTbDiagnosisYear;
            details.PreviouslyTreated = Converter.GetStatusFromString(rawNotification.PreviouslyTreated);
            details.PreviousTreatmentCountryId = rawNotification.PreviousTreatmentCountryId;
            return details;
        }

        private static TravelDetails ExtractTravelDetails(MigrationDbNotification notification)
        {
            var hasTravel = Converter.GetStatusFromString(notification.HasTravel);
            int? numberOfCountries = Converter.ToNullableInt(notification.travel_TotalNumberOfCountries);
            var countriesRecorded = new List<int?>
                {
                    notification.travel_Country1, notification.travel_Country2, notification.travel_Country3
                }.Distinct()
                .Count(c => c != null);
            int? totalNumberOfCountries = hasTravel == Status.Yes && numberOfCountries != null
                ? Math.Max(numberOfCountries.Value, countriesRecorded) 
                : (int?) null;
            
            var details = new TravelDetails();
            details.HasTravel = hasTravel;
            details.TotalNumberOfCountries = totalNumberOfCountries;
            details.Country1Id = notification.travel_Country1;
            details.Country2Id = notification.travel_Country2;
            details.Country3Id = notification.travel_Country3;
            details.StayLengthInMonths1 = notification.travel_StayLengthInMonths1;
            details.StayLengthInMonths2 = notification.travel_StayLengthInMonths2;
            details.StayLengthInMonths3 = notification.travel_StayLengthInMonths3;
            RemoveDuplicateCountries(details);
            return details;
        }

        private static VisitorDetails ExtractVisitorDetails(MigrationDbNotification notification)
        {
            var hasVisitor = Converter.GetStatusFromString(notification.HasVisitor);
            int? numberOfCountries = Converter.ToNullableInt(notification.visitor_TotalNumberOfCountries);
            var countriesRecorded = new List<int?>
                    {
                        notification.visitor_Country1, notification.visitor_Country2, notification.visitor_Country3
                    }.Distinct()
                    .Count(c => c != null);
            int? totalNumberOfCountries = hasVisitor == Status.Yes && numberOfCountries != null 
                ? Math.Max(numberOfCountries.Value, countriesRecorded) 
                : (int?) null;

            var details = new VisitorDetails();
            details.HasVisitor = hasVisitor;
            details.TotalNumberOfCountries = totalNumberOfCountries;
            details.Country1Id = notification.visitor_Country1;
            details.Country2Id = notification.visitor_Country2;
            details.Country3Id = notification.visitor_Country3;
            details.StayLengthInMonths1 = notification.visitor_StayLengthInMonths1;
            details.StayLengthInMonths2 = notification.visitor_StayLengthInMonths2;
            details.StayLengthInMonths3 = notification.visitor_StayLengthInMonths3;
            RemoveDuplicateCountries(details);
            return details;
        }

        private static void RemoveDuplicateCountries(ITravelOrVisitorDetails details)
        {
            if (details.Country1Id != null && details.Country1Id == details.Country2Id)
            {
                if (details.StayLengthInMonths1 != null && details.StayLengthInMonths2 != null)
                {
                    details.StayLengthInMonths1 = Math.Max(details.StayLengthInMonths1.Value,
                        details.StayLengthInMonths2.Value);
                }

                details.Country2Id = null;
                details.StayLengthInMonths2 = null;
            }

            if (details.Country1Id != null && details.Country1Id == details.Country3Id)
            {
                if (details.StayLengthInMonths1 != null && details.StayLengthInMonths3 != null)
                {
                    details.StayLengthInMonths1 = Math.Max(details.StayLengthInMonths1.Value,
                        details.StayLengthInMonths3.Value);
                }

                details.Country3Id = null;
                details.StayLengthInMonths3 = null;
            }

            if (details.Country2Id != null && details.Country2Id == details.Country3Id)
            {
                if (details.StayLengthInMonths2 != null && details.StayLengthInMonths3 != null)
                {
                    details.StayLengthInMonths2 = Math.Max(details.StayLengthInMonths2.Value,
                        details.StayLengthInMonths3.Value);
                }

                details.Country3Id = null;
                details.StayLengthInMonths3 = null;
            }
        }

        private async Task<PatientDetails> ExtractPatientDetails(MigrationDbNotification notification)
        {
            var addressRaw = string.Join(" \n",
                notification.Line1,
                notification.Line2,
                notification.City,
                notification.County);
            var address = RemoveCharactersNotIn(
                ValidationRegexes.CharacterValidationWithNumbersForwardSlashAndNewLine,
                addressRaw);
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
            details.PostcodeLookup = await _postcodeService.FindPostcodeAsync(notification.Postcode);
            details.PostcodeToLookup = details.PostcodeLookup?.Postcode;
            details.NoFixedAbode = notification.NoFixedAbode == 1;
            details.Address = address;
            details.EthnicityId = notification.NtbsEthnicGroupId ?? Ethnicities.NotStatedId;
            details.SexId = notification.NtbsSexId ?? Sexes.UnknownId;
            details.OccupationId = notification.NtbsOccupationId;
            details.OccupationOther = RemoveCharactersNotIn(
                ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended,
                notification.OccupationFreetext);

            ForceValidNhsNumber(details);
            
            return details;
        }

        private static string RemoveCharactersNotIn(string matchingRegex, string input)
        {
            if (input == null) { return null; }

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

        private static SocialRiskFactors ExtractSocialRiskFactors(MigrationDbNotification notification)
        {
            var factors = new SocialRiskFactors();
            factors.AlcoholMisuseStatus = Converter.GetStatusFromString(notification.AlcoholMisuseStatus);
            factors.MentalHealthStatus = Converter.GetStatusFromString(notification.MentalHealthStatus);
            factors.AsylumSeekerStatus = Converter.GetStatusFromString(notification.AsylumSeekerStatus);
            factors.ImmigrationDetaineeStatus = Converter.GetStatusFromString(notification.ImmigrationDetaineeStatus);

            factors.RiskFactorSmoking.Status = Converter.GetStatusFromString(notification.SmokingStatus);

            ExtractRiskFactor(factors.RiskFactorDrugs,
                notification.riskFactorDrugs_Status,
                notification.riskFactorDrugs_IsCurrent,
                notification.riskFactorDrugs_InPastFiveYears,
                notification.riskFactorDrugs_MoreThanFiveYearsAgo);

            ExtractRiskFactor(factors.RiskFactorHomelessness,
                notification.riskFactorHomelessness_Status,
                notification.riskFactorHomelessness_IsCurrent,
                notification.riskFactorHomelessness_InPastFiveYears,
                notification.riskFactorHomelessness_MoreThanFiveYearsAgo);

            ExtractRiskFactor(factors.RiskFactorImprisonment,
                notification.riskFactorImprisonment_Status,
                notification.riskFactorImprisonment_IsCurrent,
                notification.riskFactorImprisonment_InPastFiveYears,
                notification.riskFactorImprisonment_MoreThanFiveYearsAgo);

            return factors;
        }

        private static void ExtractRiskFactor(RiskFactorDetails riskFactor,
            string status,
            int? isCurrent,
            int? inPastFiveYears,
            int? moreThanFiveYearsAgo)
        {
            riskFactor.Status = Converter.GetStatusFromString(status);
            if (riskFactor.Status != Status.Yes)
                return;

            riskFactor.IsCurrent = Converter.GetNullableBoolValue(isCurrent);
            riskFactor.InPastFiveYears = Converter.GetNullableBoolValue(inPastFiveYears);
            riskFactor.MoreThanFiveYearsAgo = Converter.GetNullableBoolValue(moreThanFiveYearsAgo);
        }

        private static ManualTestResult AsManualTestResult(MigrationDbManualTest rawResult)
        {
            var manualTest = new ManualTestResult();
            manualTest.ManualTestTypeId = rawResult.ManualTestTypeId;
            manualTest.SampleTypeId = rawResult.SampleTypeId;
            manualTest.Result =  Converter.GetEnumValue<Result>(rawResult.Result);
            manualTest.TestDate = rawResult.TestDate;
            return manualTest;
        }

        private static SocialContextVenue AsSocialContextVenue(MigrationDbSocialContextVenue rawVenue)
        {
            var venue = new SocialContextVenue();
            venue.VenueTypeId = rawVenue.VenueTypeId;
            venue.Name = rawVenue.Name;
            venue.Address = rawVenue.Address;
            venue.Postcode = rawVenue.Postcode;
            venue.Frequency = Converter.GetEnumValue<Frequency>(rawVenue.Frequency);
            venue.DateFrom = rawVenue.DateFrom;
            venue.DateTo = rawVenue.DateTo;
            var details = RemoveCharactersNotIn(ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended, rawVenue.Details);
            venue.Details = details;
            return venue;
        }

        private static SocialContextAddress AsSocialContextAddress(MigrationDbSocialContextAddress rawAddress)
        {
            var address = new SocialContextAddress();
             address.Address = rawAddress.Address;
             address.Postcode = rawAddress.Postcode;
             address.DateFrom = rawAddress.DateFrom;
             address.DateTo = rawAddress.DateTo;
             var details = RemoveCharactersNotIn(ValidationRegexes.CharacterValidationWithNumbersForwardSlashExtended, rawAddress.Details);
             address.Details = details;
            return address;
        }

        private static MBovisAnimalExposure AsMBovisAnimalExposure(MigrationDbMBovisAnimal rawData)
        {
            var animalExposure = new MBovisAnimalExposure();
            animalExposure.YearOfExposure = rawData.YearOfExposure;
            animalExposure.AnimalType = Converter.GetEnumValue<AnimalType>(rawData.AnimalType);
            animalExposure.Animal = rawData.Animal;
            animalExposure.AnimalTbStatus = Converter.GetEnumValue<AnimalTbStatus>(rawData.AnimalTbStatus);
            animalExposure.ExposureDuration = rawData.ExposureDuration;
            animalExposure.CountryId = rawData.CountryId;
            animalExposure.OtherDetails = rawData.OtherDetails;
            return animalExposure;
        }

        private static MBovisExposureToKnownCase AsMBovisExposureToKnownCase(MigrationDbMBovisKnownCase rawData)
        {
            var caseExposure = new MBovisExposureToKnownCase();
            caseExposure.YearOfExposure = rawData.YearOfExposure;
            caseExposure.ExposureSetting = Converter.GetEnumValue<ExposureSetting>(rawData.ExposureSetting);
            caseExposure.ExposureNotificationId = rawData.ExposureNotificationId;
            caseExposure.NotifiedToPheStatus = Converter.GetStatusFromString(rawData.NotifiedToPheStatus) ?? Status.Unknown;
            caseExposure.OtherDetails = rawData.OtherDetails;
            return caseExposure;
        }

        private static MBovisOccupationExposure AsMBovisOccupationExposure(MigrationDbMBovisOccupation rawData)
        {
            var occupationExposure = new MBovisOccupationExposure();
            occupationExposure.YearOfExposure = rawData.YearOfExposure;
            occupationExposure.OccupationSetting = Converter.GetEnumValue<OccupationSetting>(rawData.OccupationSetting);
            occupationExposure.OccupationDuration = rawData.OccupationDuration;
            occupationExposure.CountryId = rawData.CountryId;
            occupationExposure.OtherDetails = rawData.OtherDetails;
            return occupationExposure;
        }

        private static MBovisUnpasteurisedMilkConsumption AsMBovisUnpasteurisedMilkConsumption(MigrationDbMBovisMilkConsumption rawData)
        {
            var milkConsumption = new MBovisUnpasteurisedMilkConsumption();
            milkConsumption.YearOfConsumption = rawData.YearOfConsumption;
            milkConsumption.MilkProductType = Converter.GetEnumValue<MilkProductType>(rawData.MilkProductType);
            milkConsumption.ConsumptionFrequency =
                Converter.GetEnumValue<ConsumptionFrequency>(rawData.ConsumptionFrequency);
            milkConsumption.CountryId = rawData.CountryId;
            milkConsumption.OtherDetails = rawData.OtherDetails;
            return milkConsumption;
        }

        private MDRDetails ExtractMdrDetailsAsync(MigrationDbNotification rawNotification)
        {
            var mdr = new MDRDetails();
            mdr.ExposureToKnownCaseStatus = Converter.GetStatusFromString(rawNotification.mdr_ExposureToKnownTbCase);
            mdr.RelationshipToCase = rawNotification.mdr_RelationshipToCase;
            // Notification.mdr_CaseInUKStatus is not used, as in NTBS it's calculated on the fly
            mdr.CountryId = rawNotification.mdr_CountryId;
            if (!string.IsNullOrEmpty(rawNotification.mdr_RelatedNotificationId))
            {
                mdr.NotifiedToPheStatus = Status.Yes;
                mdr.RelatedNotificationId = int.Parse(rawNotification.mdr_RelatedNotificationId);
            }

            return mdr;
        }

        private async Task<TreatmentEvent> AsTransferEvent(MigrationDbTransferEvent rawEvent)
        {
            var ev = new TreatmentEvent();
            ev.EventDate = rawEvent.EventDate;
            ev.TreatmentEventType = Converter.GetEnumValue<TreatmentEventType>(rawEvent.TreatmentEventType);
            ev.CaseManagerUsername = rawEvent.CaseManager;

            // ReSharper disable once InvertIf
            if (rawEvent.HospitalId is Guid guid)
            {
                var tbService = (await _referenceDataRepository.GetTbServiceFromHospitalIdAsync(guid));
                if (tbService == null)
                {
                    Log.Warning(
                        $"No TB service exists for hospital with guid {guid} - treatment event recorded without a service");
                }
                else
                {
                    ev.TbServiceCode = tbService.Code;
                }
            }

            return ev;
        }

        private async Task<TreatmentEvent> AsOutcomeEvent(MigrationDbOutcomeEvent rawEvent)
        {
            var ev = new TreatmentEvent();
            ev.EventDate = rawEvent.EventDate;
            ev.TreatmentEventType = Converter.GetEnumValue<TreatmentEventType>(rawEvent.TreatmentEventType);
            ev.TreatmentOutcomeId = rawEvent.TreatmentOutcomeId;
            ev.CaseManagerUsername = rawEvent.CaseManager;
            ev.Note = rawEvent.Note;

            // ReSharper disable once InvertIf
            if (rawEvent.NtbsHospitalId is Guid guid)
            {
                var tbService = (await _referenceDataRepository.GetTbServiceFromHospitalIdAsync(guid));
                if (tbService == null)
                {
                    Log.Warning(
                        $"No TB service exists for hospital with guid {guid} - treatment event recorded without a service");
                }
                else
                {
                    ev.TbServiceCode = tbService.Code;
                }
            }

            return ev;
        }
    }
}
