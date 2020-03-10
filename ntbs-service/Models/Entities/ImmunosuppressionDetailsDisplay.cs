using System.Text;

namespace ntbs_service.Models.Entities
{
    public partial class ImmunosuppressionDetails
    {
        public string FormattedTypesOfImmunosuppression()
        {
            var sb = new StringBuilder();
            sb.Append(HasBioTherapy == true ? "Biological Therapy" : string.Empty);

            if (HasTransplantation == true)
            {
                if (sb.Length != 0)
                    sb.Append(", ");
                sb.Append("Transplantation");
            }

            if (HasOther == true)
            {
                if (sb.Length != 0)
                    sb.Append(", ");
                sb.Append("Other");
            }

            return sb.ToString();
        }
    }
}
