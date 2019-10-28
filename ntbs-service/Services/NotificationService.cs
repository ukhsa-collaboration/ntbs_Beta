using EFAuditer;
using Microsoft.EntityFrameworkCore;
using ntbs_service.DataAccess;
using ntbs_service.Models;
using ntbs_service.Models.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ntbs_service.Services
{
    public interface INotificationService
    {
        IQueryable<Notification> GetBaseNotificationIQueryable();
        IQueryable<Notification> GetBaseQueryableNotificationByStatus(IList<NotificationStatus> statuses);
        Task<IList<Notification>> GetRecentNotificationsAsync(IEnumerable<TBService> TBServices);
        Task<IList<Notification>> GetDraftNotificationsAsync(IEnumerable<TBService> TBServices);
        Task<Notification> GetNotificationAsync(int? id);
        Task<Notification> GetNotificationWithNotificationSitesAsync(int? id);
        Task<Notification> GetNotificationWithAllInfoAsync(int? id);
        Task<NotificationGroup> GetNotificationGroupAsync(int id);
        Task UpdatePatientAsync(Notification notification, PatientDetails patientDetails);
        Task UpdatePatientFlags(PatientDetails patientDetails);
        Task UpdateClinicalDetailsAsync(Notification notification, ClinicalDetails timeline);
        Task UpdateSitesAsync(int notificationId, IEnumerable<NotificationSite> notificationSites);
        Task UpdateComorbidityAsync(Notification notification, ComorbidityDetails comorbidityDetails);
        Task UpdateEpisodeAsync(Notification notification, Episode episode);
        Task SubmitNotification(Notification notification);
        Task UpdateContactTracingAsync(Notification notification, ContactTracing contactTracing);
        Task UpdateTravelAndVisitorAsync(Notification notification, TravelDetails travelDetails, VisitorDetails visitorDetails);
        void ClearTravelOrVisitorFields(ITravelOrVisitorDetails travelOrVisitorDetails);
        Task UpdatePatientTBHistoryAsync(Notification notification, PatientTBHistory history);
        Task UpdateSocialRiskFactorsAsync(Notification notification, SocialRiskFactors riskFactors);
        Task UpdateImmunosuppresionDetailsAsync(Notification notification, ImmunosuppressionDetails immunosuppressionDetails);
        Task<Notification> CreateLinkedNotificationAsync(Notification notification);
        Task<IEnumerable<Notification>> GetNotificationsByIdAsync(IList<int> ids);
        Task DenotifyNotification(int notificationId, DenotificationDetails denotificationDetails);
    }

    public class NotificationService : INotificationService
    {
        private readonly INotificationRepository repository;
        private readonly NtbsContext context;

        public NotificationService(INotificationRepository repository, NtbsContext context)
        {
            this.repository = repository;
            this.context = context;
        }

        public IQueryable<Notification> GetBaseNotificationIQueryable()
        {
            return repository.GetBaseNotificationIQueryable();
        }

        public async Task<IList<Notification>> GetRecentNotificationsAsync(IEnumerable<TBService> TBServices)
        {
            return await repository.GetRecentNotificationsAsync(TBServices);
        }

        public async Task<IList<Notification>> GetDraftNotificationsAsync(IEnumerable<TBService> TBServices)
        {
            return await repository.GetDraftNotificationsAsync(TBServices);
        }

        public async Task<Notification> GetNotificationAsync(int? id)
        {
            return await repository.GetNotificationAsync(id);
        }

        public async Task<NotificationGroup> GetNotificationGroupAsync(int id)
        {
            return await context.NotificationGroup.Include(n => n.Notifications)
                .FirstOrDefaultAsync(n => n.NotificationGroupId == id);
        }

        public async Task UpdatePatientAsync(Notification notification, PatientDetails patient)
        {
            await UpdatePatientFlags(patient);
            context.Entry(notification.PatientDetails).CurrentValues.SetValues(patient);

            await UpdateDatabase();
        }

        public async Task UpdatePatientFlags(PatientDetails patientDetails)
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

        public async Task UpdateClinicalDetailsAsync(Notification notification, ClinicalDetails clinicalDetails)
        {
            context.Entry(notification.ClinicalDetails).CurrentValues.SetValues(clinicalDetails);

            await UpdateDatabase();
        }

        public async Task UpdateEpisodeAsync(Notification notification, Episode episode)
        {
            context.Entry(notification.Episode).CurrentValues.SetValues(episode);

            await UpdateDatabase();
        }

        public async Task UpdateContactTracingAsync(Notification notification, ContactTracing contactTracing)
        {
            context.Entry(notification.ContactTracing).CurrentValues.SetValues(contactTracing);

            await UpdateDatabase();
        }

        public async Task UpdateTravelAndVisitorAsync(Notification notification, TravelDetails travelDetails, VisitorDetails visitorDetails)
        {
            UpdateTravelDetails(notification, travelDetails);
            UpdateVisitorDetails(notification, visitorDetails);

            await UpdateDatabase();
        }

        private void UpdateTravelDetails(Notification notification, TravelDetails travelDetails)
        {
            if (travelDetails.HasTravel != true)
            {
                ClearTravelOrVisitorFields(travelDetails);
            }
            context.Entry(notification.TravelDetails).CurrentValues.SetValues(travelDetails);
        }

        private void UpdateVisitorDetails(Notification notification, VisitorDetails visitorDetails)
        {
            if (visitorDetails.HasVisitor != true)
            {
                ClearTravelOrVisitorFields(visitorDetails);
            }
            context.Entry(notification.VisitorDetails).CurrentValues.SetValues(visitorDetails);
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
            context.Entry(notification.PatientTBHistory).CurrentValues.SetValues(tBHistory);

            await UpdateDatabase();
        }

        public async Task UpdateSocialRiskFactorsAsync(Notification notification, SocialRiskFactors socialRiskFactors)
        {
            UpdateSocialRiskFactorsFlags(socialRiskFactors);
            context.Entry(notification.SocialRiskFactors).CurrentValues.SetValues(socialRiskFactors);

            context.Entry(notification.SocialRiskFactors.RiskFactorDrugs).CurrentValues.SetValues(socialRiskFactors.RiskFactorDrugs);

            context.Entry(notification.SocialRiskFactors.RiskFactorHomelessness).CurrentValues.SetValues(socialRiskFactors.RiskFactorHomelessness);

            context.Entry(notification.SocialRiskFactors.RiskFactorImprisonment).CurrentValues.SetValues(socialRiskFactors.RiskFactorImprisonment);

            await UpdateDatabase();
        }

        private void UpdateSocialRiskFactorsFlags(SocialRiskFactors socialRiskFactors)
        {
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorDrugs);
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorHomelessness);
            UpdateRiskFactorFlags(socialRiskFactors.RiskFactorImprisonment);
        }

        private void UpdateRiskFactorFlags(RiskFactorDetails riskFactor)
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

            context.Entry(notification.ImmunosuppressionDetails).CurrentValues.SetValues(immunosuppressionDetails);
            await UpdateDatabase();
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

            await UpdateDatabase();
        }

        public async Task SubmitNotification(Notification notification)
        {
            context.Attach(notification);
            notification.NotificationStatus = NotificationStatus.Notified;
            notification.SubmissionDate = DateTime.UtcNow;

            await UpdateDatabase(AuditType.Notified);
        }

        public async Task<Notification> GetNotificationWithAllInfoAsync(int? id)
        {
            return await repository.GetNotificationWithAllInfoAsync(id);
        }

        public IQueryable<Notification> GetBaseQueryableNotificationByStatus(IList<NotificationStatus> statuses)
        {
            return repository.GetBaseQueryableNotificationByStatus(statuses);
        }

        public async Task<Notification> CreateLinkedNotificationAsync(Notification notification)
        {
            var linkedNotification = new Notification();
            context.Notification.Add(linkedNotification);
            context.Entry(linkedNotification.PatientDetails).CurrentValues.SetValues(notification.PatientDetails);

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

        private async Task UpdateDatabase(AuditType auditType = AuditType.Edit)
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
            context.Entry(notification.ComorbidityDetails).CurrentValues.SetValues(comorbidityDetails);
            await UpdateDatabase();
        }

        public async Task DenotifyNotification(int notificationId, DenotificationDetails denotificationDetails)
        {
            var notification = await repository.GetNotificationAsync(notificationId);
            if (notification.DenotificationDetails == null)
            {
                notification.DenotificationDetails = denotificationDetails;
            }
            else
            {
                context.Entry(notification.DenotificationDetails).CurrentValues.SetValues(denotificationDetails);
            }

            notification.NotificationStatus = NotificationStatus.Denotified;
            await UpdateDatabase(AuditType.Denotified);
        }
    }
}
