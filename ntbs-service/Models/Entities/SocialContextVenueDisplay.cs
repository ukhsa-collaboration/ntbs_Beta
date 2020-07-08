namespace ntbs_service.Models.Entities
{
    public partial class SocialContextVenue
    {
        public string Title => $"{Name ?? VenueType?.FormatCategoryAndName() ?? "Unspecified venue"} - {DateRange}";
    }
}
