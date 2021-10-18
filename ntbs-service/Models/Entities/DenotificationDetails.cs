using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Enums;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public partial class DenotificationDetails : IOwnedEntityForAuditing
    {
        [AssertThat(nameof(DenotificationAfterNotification), ErrorMessage = ValidationMessages.DateShouldBeLaterThanNotification)]
        [AssertThat(nameof(DenotificationNotAfterToday), ErrorMessage = ValidationMessages.DenotificationDateLatestToday)]
        [Display(Name = "Denotification date")]
        public DateTime DateOfDenotification { get; set; }

        /// <summary>
        /// Used for validation purposes only, requires consumer to populate it.
        /// </summary>
        [NotMapped]
        [Display(Name = "Denotification date")]
        public DateTime? DateOfNotification { get; set; }

        /// <summary>
        /// If DateOfNotification has not been supplied default to passing this test.
        /// </summary>
        [NotMapped]
        public bool DenotificationAfterNotification => (DateOfNotification == null) || DateOfDenotification.Date >= DateOfNotification.Value.Date;

        [NotMapped]
        public bool DenotificationNotAfterToday => DateOfDenotification.Date <= DateTime.Now.Date;

        [AssertThat("Reason != DenotificationReason.NotSelected", ErrorMessage = ValidationMessages.DenotificationReasonRequired)]
        public DenotificationReason Reason { get; set; }

        [RequiredIf("Reason == DenotificationReason.Other", ErrorMessage = ValidationMessages.DenotificationReasonOtherRequired)]
        [MaxLength(150)]
        [RegularExpression(
            ValidationRegexes.CharacterValidationAsciiBasic,
            ErrorMessage = ValidationMessages.InvalidCharacter)]
        [Display(Name = "Description")]
        public string OtherDescription { get; set; }

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
