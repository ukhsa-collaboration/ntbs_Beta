namespace ntbs_service.Properties
{
    public class ApplicationInsightsOptions
    {
        public bool? Enabled { get; set; }
        public bool? EnableSqlCommandTextInstrumentation { get; set; }
        public bool? EnableProfiler { get; set; }
        public float? RandomProfilingOverhead { get; set; }
        public int? ProfilerDurationSeconds { get; set; }
        public string ConnectionString { get; set; }
    }
}
