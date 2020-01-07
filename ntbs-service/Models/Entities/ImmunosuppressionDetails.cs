﻿using System.ComponentModel.DataAnnotations;
using System.Text;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class ImmunosuppressionDetails : ModelBase, IOwnedEntity
    {
        [AssertThat(@"Status != Enums.Status.Yes
            || (HasBioTherapy || HasTransplantation || HasOther)",
            ErrorMessage = ValidationMessages.ImmunosuppressionTypeRequired)]
        public Status? Status { get; set; }

        public bool HasBioTherapy { get; set; }
        public bool HasTransplantation { get; set; }
        public bool HasOther { get; set; }

        [RequiredIf("Status == Enums.Status.Yes && HasOther == true",
            ErrorMessage = ValidationMessages.ImmunosuppressionDetailRequired)]
        [MaxLength(100)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        [Display(Name = "Details")]
        public string OtherDescription { get; set; }

        public string CreateTypesOfImmunosuppressionString()
        {
            var sb = new StringBuilder();
            sb.Append(HasBioTherapy ? "Biological Therapy" : string.Empty);

            if (HasTransplantation)
            {
                if (sb.Length != 0)
                    sb.Append(", ");
                sb.Append("Transplantation");
            }

            if (HasOther)
            {
                if (sb.Length != 0)
                    sb.Append(", ");
                sb.Append("Other");
            }

            return sb.ToString();
        }

        string IOwnedEntity.RootEntityType => RootEntities.Notification;
    }
}
