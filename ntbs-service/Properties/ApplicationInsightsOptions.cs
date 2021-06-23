namespace ntbs_service.Properties
{
    public class ApplicationInsightsOptions
    {
        public bool? Enabled { get; set; }
        public bool? EnableSqlCommandTextInstrumentation { get; set; }
        public string ConnectionString { get; set; }
    }
}
