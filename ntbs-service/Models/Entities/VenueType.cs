namespace ntbs_service.Models.Entities
{
    public class VenueType
    {
        public int VenueTypeId { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }

        public string FormatCategoryAndName()
        {
            return $"{Category} - {Name}";
        }
    }
}