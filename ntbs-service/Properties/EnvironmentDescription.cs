namespace ntbs_service.Properties
{
    public class EnvironmentDescription
    {
        public bool ContainsLiveData { get; set; } = false;
        public bool IsLiveSystem { get; set; } = false;
        public string DisplayName { get; set; } = "Unknown environment";
        public string EnvironmentName { get; set; }
    }
}
