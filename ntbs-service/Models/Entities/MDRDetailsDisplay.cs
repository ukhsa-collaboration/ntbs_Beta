namespace ntbs_service.Models.Entities
{
    public partial class MDRDetails
    {
        public string MDRCaseCountryName => Country?.Name;
    }
}