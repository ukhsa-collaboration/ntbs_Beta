using EFAuditer;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ntbs_service.Services
{
    public interface INotificationService
    {
        IQueryable<Notification> GetBaseQueryableNotificationByStatus(IList<NotificationStatus> statuses);
        Task<Notification> GetNotificationAsync(int? id);
        Task<Notification> GetNotificationWithNotificationSitesAsync(int? id);
        Task<Notification> GetNotificationWithAllInfoAsync(int? id);
        Task UpdatePatientAsync(Notification notification, PatientDetails patientDetails);
        Task UpdatePatientFlagsAsync(PatientDetails patientDetails);
        Task UpdateClinicalDetailsAsync(Notification notification, ClinicalDetails timeline);
        Task UpdateSitesAsync(int notificationId, IEnumerable<NotificationSite> notificationSites);
        Task UpdateComorbidityAsync(Notification notification, ComorbidityDetails comorbidityDetails);
        Task UpdateEpisodeAsync(Notification notification, Episode episode);
        Task SubmitNotificationAsync(Notification notification);
        Task UpdateContactTracingAsync(Notification notification, ContactTracing contactTracing);
        Task UpdateTravelAndVisitorAsync(Notification notification, TravelDetails travelDetails, VisitorDetails visitorDetails);
        void ClearTravelOrVisitorFields(ITravelOrVisitorDetails travelOrVisitorDetails);
        Task UpdatePatientTBHistoryAsync(Notification notification, PatientTBHistory history);
        Task UpdateSocialRiskFactorsAsync(Notification notification, SocialRiskFactors riskFactors);
        Task UpdateImmunosuppresionDetailsAsync(Notification notification, ImmunosuppressionDetails immunosuppressionDetails);
        Task<Notification> CreateLinkedNotificationAsync(Notification notification, ClaimsPrincipal user);
        Task<IEnumerable<Notification>> GetNotificationsByIdAsync(IList<int> ids);
        Task DenotifyNotificationAsync(int notificationId, DenotificationDetails denotificationDetails);
        Task DeleteNotificationAsync(int notificationId, string deletionReason);
        Task<Notification> CreateNewNotificationForUser(ClaimsPrincipal user);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository repository;
        private readonly IUserService userService;
        private readonly NtbsContext context;

        public NotificationService(INotificationRepository repository, IUserService userService, NtbsContext context)
        {
            this.repository = repository;
            this.userService = userService;
            this.context = context;
        }

        public async Task<Notification> GetNotificationAsync(int? id)
        {
            return await repository.GetNotificationAsync(id);
        }

        public async Task UpdatePatientAsync(Notification notification, PatientDetails patient)
        {
            await UpdatePatientFlagsAsync(patient);
            context.SetValues(notification.PatientDetails, patient);

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
            var country = await context.GetCountryByIdAsync(patient.CountryId);
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
                var occupation = await context.GetOccupationByIdAsync(patient.OccupationId.Value);
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
            context.SetValues(notification.ClinicalDetails, clinicalDetails);

            await UpdateDatabaseAsync();
        }

        public async Task UpdateEpisodeAsync(Notification notification, Episode episode)
        {
            context.SetValues(notification.Episode, episode);

            await UpdateDatabaseAsync();
        }

        public async Task UpdateContactTracingAsync(Notification notification, ContactTracing contactTracing)
        {
            context.SetValues(notification.ContactTracing, contactTracing);

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
            context.SetValues(notification.TravelDetails, travelDetails);
        }

        private void UpdateVisitorDetails(Notification notification, VisitorDetails visitorDetails)
        {
            if (visitorDetails.HasVisitor != true)
            {
                ClearTravelOrVisitorFields(visitorDetails);
            }
            context.SetValues(notification.VisitorDetails, visitorDetails);
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

        public async Task UpdatePatientTBHistoryAsync(Notification notification, PatientTBHistory tBHistory)
        {
            context.SetValues(notification.PatientTBHistory, tBHistory);

            await UpdateDatabaseAsync();
        }

        public async Task UpdateSocialRiskFactorsAsync(Notification notification, SocialRiskFactors socialRiskFactors)
        {
            UpdateSocialRiskFactorsFlags(socialRiskFactors);
            context.SetValues(notification.SocialRiskFactors, socialRiskFactors);

            context.SetValues(notification.SocialRiskFactors.RiskFactorDrugs, socialRiskFactors.RiskFactorDrugs);

            context.SetValues(notification.SocialRiskFactors.RiskFactorHomelessness, socialRiskFactors.RiskFactorHomelessness);

            context.SetValues(notification.SocialRiskFactors.RiskFactorImprisonment, socialRiskFactors.RiskFactorImprisonment);

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

        public async Task<Notification> GetNotificationWithNotificationSitesAsync(int? id)
        {
            return await repository.GetNotificationWithNotificationSitesAsync(id);
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

            context.SetValues(notification.ImmunosuppressionDetails, immunosuppressionDetails);
            await UpdateDatabaseAsync();
        }

        public async Task UpdateSitesAsync(int notificationId, IEnumerable<NotificationSite> notificationSites)
        {
            var currentSites = context.NotificationSite.Where(ns => ns.NotificationId == notificationId);

            foreach (var newSite in notificationSites)
            {
                var existingSite = currentSites.FirstOrDefault(s => s.SiteId == newSite.SiteId);
                if (existingSite == null)
                {
                    context.NotificationSite.Add(newSite);
                }
                else if (existingSite.SiteDescription != newSite.SiteDescription)
                {
                    existingSite.SiteDescription = newSite.SiteDescription;
                }
            }

            var sitesToRemove = currentSites.Where(s => !notificationSites.Select(ns => ns.SiteId).Contains(s.SiteId));
            context.NotificationSite.RemoveRange(sitesToRemove);

            await UpdateDatabaseAsync();
        }

        public async Task SubmitNotificationAsync(Notification notification)
        {
            context.Attach(notification);
            notification.NotificationStatus = NotificationStatus.Notified;
            notification.SubmissionDate = DateTime.UtcNow;

            await UpdateDatabaseAsync(AuditType.Notified);
        }

        public async Task<Notification> GetNotificationWithAllInfoAsync(int? id)
        {
            return await repository.GetNotificationWithAllInfoAsync(id);
        }

        public IQueryable<Notification> GetBaseQueryableNotificationByStatus(IList<NotificationStatus> statuses)
        {
            return repository.GetBaseQueryableNotificationByStatus(statuses);
        }

        public async Task<Notification> CreateLinkedNotificationAsync(Notification notification, ClaimsPrincipal user)
        {
            var linkedNotification = await CreateNewNotificationForUser(user);
            context.Attach(linkedNotification);
            context.SetValues(linkedNotification.PatientDetails, notification.PatientDetails);

            if (notification.GroupId != null)
            {
                linkedNotification.GroupId = notification.GroupId;
            }
            else
            {
                var group = new NotificationGroup();
                context.NotificationGroup.Add(group);

                linkedNotification.GroupId = group.NotificationGroupId;
                notification.GroupId = group.NotificationGroupId;
            }

            await context.SaveChangesAsync();

            return linkedNotification;
        }

        public async Task<Notification> CreateNewNotificationForUser(ClaimsPrincipal user)
        {
            var notification = new Notification
            {
                CreationDate = DateTime.Now,
                // We need to set a default value for TBService for users that do not have full permissions
                Episode = { TBService = await userService.GetDefaultTBService(user) }
            };

            await repository.AddNotificationAsync(notification);
            return notification;
        }

        private async Task UpdateDatabaseAsync(AuditType auditType = AuditType.Edit)
        {
            context.AddAuditCustomField(CustomFields.AuditDetails, auditType);
            await context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Notification>> GetNotificationsByIdAsync(IList<int> ids)
        {
            return await repository.GetNotificationsByIdsAsync(ids);
        }

        public async Task UpdateComorbidityAsync(Notification notification, ComorbidityDetails comorbidityDetails)
        {
            context.SetValues(notification.ComorbidityDetails, comorbidityDetails);
            await UpdateDatabaseAsync();
        }

        public async Task DenotifyNotificationAsync(int notificationId, DenotificationDetails denotificationDetails)
        {
            var notification = await repository.GetNotificationAsync(notificationId);
            if (notification.DenotificationDetails == null)
            {
                notification.DenotificationDetails = denotificationDetails;
            }
            else
            {
                context.SetValues(notification.DenotificationDetails, denotificationDetails);
            }

            notification.NotificationStatus = NotificationStatus.Denotified;
            await UpdateDatabaseAsync(AuditType.Denotified);
        }

        public async Task DeleteNotificationAsync(int notificationId, string deletionReason)
        {
            var notification = await repository.GetNotificationAsync(notificationId);
            notification.DeletionReason = deletionReason;

            notification.NotificationStatus = NotificationStatus.Deleted;
            await UpdateDatabaseAsync(AuditType.Deleted);
        }
    }
}
