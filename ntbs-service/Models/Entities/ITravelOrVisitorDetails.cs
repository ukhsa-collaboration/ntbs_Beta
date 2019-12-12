using ntbs_service.Models.ReferenceEntities;

namespace ntbs_service.Models.Entities
{
    public interface ITravelOrVisitorDetails
    {
        int? TotalNumberOfCountries { get; set; }

        int? Country1Id { get; set; }
        Country Country1 { get; set; }
        int? StayLengthInMonths1 { get; set; }

        int? Country2Id { get; set; }
        Country Country2 { get; set; }
        int? StayLengthInMonths2 { get; set; }

        int? Country3Id { get; set; }
        Country Country3 { get; set; }
        int? StayLengthInMonths3 { get; set; }
    }
}
