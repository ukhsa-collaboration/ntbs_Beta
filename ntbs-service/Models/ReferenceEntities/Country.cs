namespace ntbs_service.Models.ReferenceEntities
{
    public class Country
    {
        public int CountryId { get; set; }
        public string Name { get; set; }
        public string IsoCode { get; set; }
        public bool HasHighTbOccurence { get; set; }
        public bool IsLegacy { get; set; }
    }
}
