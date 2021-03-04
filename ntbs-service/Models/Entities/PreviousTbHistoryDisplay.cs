namespace ntbs_service.Models.Entities
{
    public partial class PreviousTbHistory
    {
        public string PreviousTreatmentCountryName => PreviousTreatmentCountry?.Name;
    }
}
