namespace ntbs_service.Models.Validations
{
    public class InputValidationModel
    {
        public string Value { get; set; }
        public string Key { get; set; }
        public bool ShouldValidateFull { get; set; }
    }

    public class DateValidationModel
    {
        public string Key {get; set;}
        public string Day {get; set;}
        public string Month {get; set;}
        public string Year {get; set;}
        public int NotificationId { get; set; }
    }

    public class NhsNumberValidationModel
    {
        public int NotificationId { get; set; }
        public string NhsNumber { get; set; }
    }

    public class ImmunosuppressionValidationModel
    {
        public string ImmunosuppressionStatus { get; set; }
        public bool HasBioTherapy { get; set; }
        public bool HasTransplantation { get; set; }
        public bool HasOther { get; set; }
        public string OtherDescription { get; set; }
    }

    public class YearComparisonValidationModel
    {
        public int? NewYear { get; set; }
        public int ExistingYear { get; set; }
        public string PropertyName { get; set; }
    }

    public class PostcodeValidationModel
    {
        public string Postcode { get; set; }
        public bool ShouldValidateFull { get; set; }
    }
}
