namespace ntbs_service.Properties
{
    public class ScheduledJobConfig
    {
        public bool UserSyncEnabled { get; set; }
        public string UserSyncTiming { get; set; }
        
        public bool DrugResistanceProfileUpdateEnabled { get; set; }
        public string DrugResistanceProfileUpdateTiming { get; set; }
        
        public bool UnmatchedLabResultAlertsEnabled { get; set; }
        public string UnmatchedLabResultAlertsTiming { get; set; }
        
        public bool DataQualityAlertsEnabled { get; set; }
        public string DataQualityAlertsTiming { get; set; }
        
        public bool NotificationClusterUpdateEnabled { get; set; }
        public string NotificationClusterUpdateTiming { get; set; }
    }
}
