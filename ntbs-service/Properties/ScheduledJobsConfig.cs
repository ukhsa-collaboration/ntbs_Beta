namespace ntbs_service.Properties
{
    public class ScheduledJobsConfig
    {
        public bool UserSyncEnabled { get; set; }
        public string UserSyncCron { get; set; }

        public bool CloseInactiveNotificationsEnabled { get; set; }
        public string CloseInactiveNotificationsCron { get; set; }

        public bool DrugResistanceProfileUpdateEnabled { get; set; }
        public string DrugResistanceProfileUpdateCron { get; set; }

        public bool UnmatchedLabResultAlertsEnabled { get; set; }
        public string UnmatchedLabResultAlertsCron { get; set; }

        public bool DataQualityAlertsEnabled { get; set; }
        public string DataQualityAlertsCron { get; set; }

        public bool NotificationClusterUpdateEnabled { get; set; }
        public string NotificationClusterUpdateCron { get; set; }

        public bool MarkImportedNotificationsAsImportedEnabled { get; set; }
        public string MarkImportedNotificationsAsImportedCron { get; set; }

        public bool ReportingDataRefreshJobEnabled { get; set; }
        public string ReportingDataRefreshJobCron { get; set; }

        public bool ReportingDataProcessingJobEnabled { get; set; }
        public string ReportingDataProcessingJobCron { get; set; }

        public bool UpdateTableCountsJobEnabled { get; set; }
        public string UpdateTableCountsJobCron { get; set; }

        public bool GenericStoredProcedureJobEnabled { get; set; }
        public string GenericStoredProcedureJobCron { get; set; }
        public string GenericStoredProcedureNameToRun { get; set; }
    }
}
