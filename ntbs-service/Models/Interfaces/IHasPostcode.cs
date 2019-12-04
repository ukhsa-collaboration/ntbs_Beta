namespace ntbs_service.Models.Interfaces
{
    public interface IHasPostcode
    {
        string Postcode { get; set; }
        string PostcodeToLookup { get; set; }
    }
}