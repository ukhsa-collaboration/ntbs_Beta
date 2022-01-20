using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using ntbs_service.Helpers;

namespace ntbs_service.Models.Entities
{
    public partial class PatientDetails
    {
        [Display(Name = "Name")]
        public string FullName =>
            string.Join(", ", new[] { FamilyName?.ToUpper(), GivenName }.Where(s => !string.IsNullOrEmpty(s)));

        public string FormattedNhsNumber => NhsNumberNotKnown
            ? "Not known"
            : NotificationFieldFormattingHelper.FormatNHSNumberForDisplay(NhsNumber);

        public string SexLabel => Sex?.Label;
        public string EthnicityLabel => Ethnicity?.Label;
        public string CountryName => Country?.Name;
        public bool UkBorn => CountryId == Countries.UkId;
        public IList<string> FormattedAddress => (Address ?? string.Empty).Split(Environment.NewLine);
        public string FormattedNoAbodeOrPostcodeString => NoFixedAbode ? "No fixed abode" : Postcode?.Trim();
        public string FormattedDob => Dob.ConvertToString();

        public string FormattedOccupationString
        {
            get
            {
                if (Occupation == null)
                {
                    return string.Empty;
                }

                if (Occupation.HasFreeTextField && !string.IsNullOrEmpty(OccupationOther))
                {
                    return $"{Occupation.Sector} - {OccupationOther}";
                }

                return Occupation.Sector == "Other" ? Occupation.Role : $"{Occupation.Sector} - {Occupation.Role}";
            }
        }

        public string LocalAuthorityName => PostcodeLookup?.LocalAuthority?.Name;
        public string ResidencePHECName => PostcodeLookup?.LocalAuthority?.LocalAuthorityToPHEC?.PHEC?.Name;
    }
}
