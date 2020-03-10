using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public partial class VisitorDetails
    {
        public string HasRecentVisitor => HasVisitor.FormatYesNo();
    }
}