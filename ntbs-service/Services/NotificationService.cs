using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EFAuditer;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;

namespace ntbs_service.Services
{
    public interface INotificationService
    {
        Task UpdatePatientAsync(Notification notification, PatientDetails patientDetails);
        Task UpdatePatientFlagsAsync(PatientDetails patientDetails);
        Task UpdateClinicalDetailsAsync(Notification notification, ClinicalDetails timeline);
        Task UpdateTestDataAsync(Notification notification, TestData testData);
        Task UpdateSitesAsync(int notificationId, IEnumerable<NotificationSite> notificationSites);
        Task UpdateComorbidityAsync(Notification notification, ComorbidityDetails comorbidityDetails);
        Task UpdateEpisodeAsync(Notification notification, Episode episode);
        Task SubmitNotificationAsync(Notification notification);
        Task UpdateContactTracingAsync(Notification notification, ContactTracing contactTracing);
        Task UpdateTravelAndVisitorAsync(Notification notification, TravelDetails travelDetails, VisitorDetails visitorDetails);
        void ClearTravelOrVisitorFields(ITravelOrVisitorDetails travelOrVisitorDetails);
        Task UpdatePatientTbHistoryAsync(Notification notification, PatientTBHistory history);
        Task UpdateSocialRiskFactorsAsync(Notification notification, SocialRiskFactors riskFactors);
        Task UpdateImmunosuppresionDetailsAsync(Notification notification, ImmunosuppressionDetails immunosuppressionDetails);
        Task<Notification> CreateLinkedNotificationAsync(Notification notification, ClaimsPrincipal user);
        Task DenotifyNotificationAsync(int notificationId, DenotificationDetails denotificationDetails);
        Task DeleteNotificationAsync(int notificationId, string deletionReason);
        Task<Notification> CreateNewNotificationForUser(ClaimsPrincipal user);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository _notificationRepository;
        private readonly IReferenceDataRepository _referenceDataRepository;
        private readonly IUserService _userService;
        private readonly NtbsContext _context;

        public NotificationService(
            INotificationRepository notificationRepository,
            IReferenceDataRepository referenceDataRepository,
            IUserService userService,
            NtbsContext context)
        {
            _notificationRepository = notificationRepository;
            _referenceDataRepository = referenceDataRepository;
            _userService = userService;
            _context = context;
        }

        public async Task UpdatePatientAsync(Notification notification, PatientDetails patient)
        {
            await UpdatePatientFlagsAsync(patient);
            _context.SetValues(notification.PatientDetails, patient);

            await UpdateDatabaseAsync();
        }

        public async Task UpdatePatientFlagsAsync(PatientDetails patientDetails)
        {
            if (patientDetails.NhsNumberNotKnown)
            {
                patientDetails.NhsNumber = null;
            }

            if (patientDetails.NoFixedAbode)
            {
                patientDetails.Postcode = null;
            }

            await UpdateUkBorn(patientDetails);
            UpdateEntryYearToUk(patientDetails);

            await UpdateOccupation(patientDetails);
        }

        private async Task UpdateUkBorn(PatientDetails patient)
        {
            if (patient.CountryId == null)
            {
                patient.UkBorn = null;
                return;
            }

            var country = await _referenceDataRepository.GetCountryByIdAsync(patient.CountryId.Value);
            if (country == null)
            {
                patient.UkBorn = null;
                return;
            }

            switch (country.IsoCode)
            {
                case Countries.UkCode:
                    patient.UkBorn = true;
                    break;
                case Countries.UnknownCode:
                    patient.UkBorn = null;
                    break;
                default:
                    patient.UkBorn = false;
                    break;
            }
        }

        private static void UpdateEntryYearToUk(PatientDetails patient)
        {
            if (patient.UkBorn != false)
            {
                patient.YearOfUkEntry = null;
            }
        }

        private async Task UpdateOccupation(PatientDetails patient)
        {
            if (patient.OccupationId.HasValue)
            {
                var occupation = await _referenceDataRepository.GetOccupationByIdAsync(patient.OccupationId.Value);
                if (occupation != null)
                {
                    if (occupation.HasFreeTextField)
                    {
                        return;
                    }
                }
            }

            patient.OccupationOther = null;
        }

        public async Task UpdateClinicalDetailsAsync(Notification notification, ClinicalDetails clinicalDetails)
        {
            _context.SetValues(notification.ClinicalDetails, clinicalDetails);

            await UpdateDatabaseAsync();
        }

        public async Task UpdateTestDataAsync(Notification notification, TestData testData)
        {
            _context.SetValues(notification.TestData, testData);

            await UpdateDatabaseAsync();
        }

        public async Task UpdateEpisodeAsync(Notification notification, Episode episode)
        {
            _context.SetValues(notification.Episode, episode);

            await UpdateDatabaseAsync();
        }

        public async Task UpdateContactTracingAsync(Notification notification, ContactTracing contactTracing)
        {
            _context.SetValues(notification.ContactTracing, contactTracing);

            await UpdateDatabaseAsync();
        }

        public async Task UpdateTravelAndVisitorAsync(Notification notification, TravelDetails travelDetails, VisitorDetails visitorDetails)
        {
            UpdateTravelDetails(notification, travelDetails);
            UpdateVisitorDetails(notification, visitorDetails);

            await UpdateDatabaseAsync();
        }

        private void UpdateTravelDetails(Notification notification, TravelDetails travelDetails)
        {
            if (travelDetails.HasTravel != true)
            {
                ClearTravelOrVisitorFields(travelDetails);
            }
            _context.SetValues(notification.TravelDetails, travelDetails);
        }

        private void UpdateVisitorDetails(Notification notification, VisitorDetails visitorDetails)
        {
            if (visitorDetails.HasVisitor != true)
            {
                ClearTravelOrVisitorFields(visitorDetails);
            }
            _context.SetValues(notification.VisitorDetails, visitorDetails);
        }

        public void ClearTravelOrVisitorFields(ITravelOrVisitorDetails travelOrVisitorDetails)
        {
            travelOrVisitorDetails.TotalNumberOfCountries = null;
            travelOrVisitorDetails.Country1 = null;
            travelOrVisitorDetails.Country1Id = null;
            travelOrVisitorDetails.StayLengthInMonths1 = null;
            travelOrVisitorDetails.Country2 = null;
            travelOrVisitorDetails.Country2Id = null;
            travelOrVisitorDetails.StayLengthInMonths2 = null;
            travelOrVisitorDetails.Country3 = null;
            travelOrVisitorDetails.Country3Id = null;
            travelOrVisitorDetails.StayLengthInMonths3 = null;
        }

        public async Task UpdatePatientTbHistoryAsync(Notification notification, PatientTBHistory tBHistory)
        {
            _context.SetValues(notification.PatientTBHistory, tBHistory);

            await UpdateDatabaseAsync();
        }

        public async Task UpdateSocialRiskFactorsAsync(Notification notification, SocialRiskFactors socialRiskFactors)
        {
            UpdateSocialRiskFactorsFlags(socialRiskFactors);
            _context.SetValues(notification.SocialRiskFactors, socialRiskFactors);

            _context.SetValues(notification.SocialRiskFactors.RiskFactorDrugs, socialRiskFactors.RiskFactorDrugs);

            _context.SetValues(notification.SocialRiskFactors.RiskFactorHomelessness, socialRiskFactors.RiskFactorHomelessness);

            _context.SetValues(notification.SocialRiskFactors.RiskFactorImprisonment, socialRiskFactors.RiskFactorImprisonment);

            await UpdateDatabaseAsync();
        }

        private static void UpdateSocialRiskFactorsFlags(SocialRiskFactors socialRiskFactors)
        {
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorDrugs);
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorHomelessness);
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorImprisonment);
        }

        private static void UpdateRiskFactorFlags(RiskFactorDetails riskFactor)
        {
            if (riskFactor.Status != Status.Yes)
            {
                riskFactor.IsCurrent = false;
                riskFactor.InPastFiveYears = false;
                riskFactor.MoreThanFiveYearsAgo = false;
            }
        }

        public async Task UpdateImmunosuppresionDetailsAsync(Notification notification, ImmunosuppressionDetails immunosuppressionDetails)
        {
            if (immunosuppressionDetails.Status != Status.Yes)
            {
                immunosuppressionDetails.HasBioTherapy = false;
                immunosuppressionDetails.HasTransplantation = false;
                immunosuppressionDetails.HasOther = false;
                immunosuppressionDetails.OtherDescription = null;
            }

            if (immunosuppressionDetails.HasOther == false)
            {
                immunosuppressionDetails.OtherDescription = null;
            }

            _context.SetValues(notification.ImmunosuppressionDetails, immunosuppressionDetails);
            await UpdateDatabaseAsync();
        }

        public async Task UpdateSitesAsync(int notificationId, IEnumerable<NotificationSite> notificationSites)
        {
            var currentSites = _context.NotificationSite.Where(ns => ns.NotificationId == notificationId);

            foreach (var newSite in notificationSites)
            {
                var existingSite = currentSites.FirstOrDefault(s => s.SiteId == newSite.SiteId);
                if (existingSite == null)
                {
                    _context.NotificationSite.Add(newSite);
                }
                else if (existingSite.SiteDescription != newSite.SiteDescription)
                {
                    existingSite.SiteDescription = newSite.SiteDescription;
                }
            }

            var sitesToRemove = currentSites.Where(s => !notificationSites.Select(ns => ns.SiteId).Contains(s.SiteId));
            _context.NotificationSite.RemoveRange(sitesToRemove);

            await UpdateDatabaseAsync();
        }

        public async Task SubmitNotificationAsync(Notification notification)
        {
            _context.Attach(notification);
            notification.NotificationStatus = NotificationStatus.Notified;
            notification.SubmissionDate = DateTime.UtcNow;

            await UpdateDatabaseAsync(AuditType.Notified);
        }

        public async Task<Notification> CreateLinkedNotificationAsync(Notification notification, ClaimsPrincipal user)
        {
            var linkedNotification = await CreateNewNotificationForUser(user);
            _context.Attach(linkedNotification);
            _context.SetValues(linkedNotification.PatientDetails, notification.PatientDetails);

            if (notification.GroupId != null)
            {
                linkedNotification.GroupId = notification.GroupId;
            }
            else
            {
                var group = new NotificationGroup();
                _context.NotificationGroup.Add(group);

                linkedNotification.GroupId = group.NotificationGroupId;
                notification.GroupId = group.NotificationGroupId;
            }

            await _context.SaveChangesAsync();

            return linkedNotification;
        }

        public async Task<Notification> CreateNewNotificationForUser(ClaimsPrincipal user)
        {
            var defaultTbService = await _userService.GetDefaultTbService(user);
            var caseManagerEmail = await GetDefaultCaseManagerEmail(user, defaultTbService?.Code);

            var notification = new Notification
            {
                CreationDate = DateTime.Now,
                Episode =
                {
                    TBService = defaultTbService,
                    CaseManagerEmail = caseManagerEmail
                }
            };

            await _notificationRepository.AddNotificationAsync(notification);
            return notification;
        }

        private async Task<string> GetDefaultCaseManagerEmail(ClaimsPrincipal user, string tbServiceCode)
        {
            var caseManagersForTbService =
                await _referenceDataRepository.GetCaseManagersByTbServiceCodesAsync(new List<string> { tbServiceCode });
            var userEmail = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var upperUserEmail = userEmail?.ToUpperInvariant();

            return caseManagersForTbService.Any(c => c.Email.ToUpperInvariant() == upperUserEmail) ? userEmail : null;
        }

        private async Task UpdateDatabaseAsync(AuditType auditType = AuditType.Edit)
        {
            _context.AddAuditCustomField(CustomFields.AuditDetails, auditType);
            await _context.SaveChangesAsync();
        }

        public async Task UpdateComorbidityAsync(Notification notification, ComorbidityDetails comorbidityDetails)
        {
            _context.SetValues(notification.ComorbidityDetails, comorbidityDetails);
            await UpdateDatabaseAsync();
        }

        public async Task DenotifyNotificationAsync(int notificationId, DenotificationDetails denotificationDetails)
        {
            var notification = await _notificationRepository.GetNotificationAsync(notificationId);
            if (notification.DenotificationDetails == null)
            {
                notification.DenotificationDetails = denotificationDetails;
            }
            else
            {
                _context.SetValues(notification.DenotificationDetails, denotificationDetails);
            }

            notification.NotificationStatus = NotificationStatus.Denotified;
            await UpdateDatabaseAsync(AuditType.Denotified);
        }

        public async Task DeleteNotificationAsync(int notificationId, string deletionReason)
        {
            var notification = await _notificationRepository.GetNotificationAsync(notificationId);
            notification.DeletionReason = deletionReason;
            notification.GroupId = null;
            notification.NotificationStatus = NotificationStatus.Deleted;
            await UpdateDatabaseAsync(AuditType.Deleted);
        }
    }
}
