namespace ntbs_service.Models.Entities
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public bool HasHighTbOccurence { get; set; }
    }
}
