using ntbs_service.Helpers;
using ntbs_service.Models.Enums;

namespace ntbs_service.Models.Entities
{
    public partial class DenotificationDetails
    {
        public string FormattedDenotificationDate => DateOfDenotification.ConvertToString();

        public string DenotificationReasonString =>
            Reason.GetDisplayName() + (Reason == DenotificationReason.Other ? $" - {OtherDescription}" : "");
    }
}
