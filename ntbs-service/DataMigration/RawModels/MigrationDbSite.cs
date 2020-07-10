namespace ntbs_service.DataMigration.RawModels
{
    // ReSharper disable once ClassNeverInstantiated.Global
    // NotificationSite
    public class MigrationDbSite : MigrationDbRecord
    {
        public string Source { get; set; }
        public int? SiteId { get; set; }
        public string SiteDescription { get; set; }
        public string FreeTextUsedToDetermineSiteId { get; set; }
    }
}
