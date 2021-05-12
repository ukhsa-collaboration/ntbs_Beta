namespace ntbs_service
{
    public static class Constants
    {
        public const string Release = "RELEASE";
        public const string AuditEnabledConfigValue = "AppConfig:AuditingEnabled";
        public const string ClusterMatchingConfig = "ClusterMatchingConfig";
        public const string ClusterMatchingConfigMockOut = "MockOutClusterMatching";
        public const string DbConnectionStringReporting = "reporting";
        public const string HangfireEnabled = "Hangfire:Enabled";
        public const string HangfireWorkerCount = "Hangfire:WorkerCount";
        public const string LegacySearchEnabledConfigValue = "AppConfig:LegacySearchEnabled";
        public const string ReferenceLabResultsConfig = "ReferenceLabResultsConfig";
        public const string ReferenceLabResultsConfigMockOut = "MockOutSpecimenMatching";
        public const string ScheduledJobsConfig = "ScheduledJobsConfig";
        public const string DbConnectionStringMigration = "migration";
        public const string DbConnectionStringSpecimenMatching = "specimenMatching";
        public const string ExternalLinks = "ExternalLinks";
        public const string EnvironmentDescription = "EnvironmentDescription";
        public const string EnvironmentName = "EnvironmentDescription:EnvironmentName";

        public const int SqlServerDefaultCommandTimeOut = 600;
    }
}
