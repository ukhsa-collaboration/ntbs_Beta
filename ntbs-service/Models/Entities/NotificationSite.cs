using System.ComponentModel.DataAnnotations;
using EFAuditer;
using ExpressiveAnnotations.Attributes;
using ntbs_service.Models.ReferenceEntities;
using ntbs_service.Models.Validations;

namespace ntbs_service.Models.Entities
{
    public class NotificationSite : ModelBase, IHasRootEntity
    {
        public int NotificationId { get; set; }

        public int SiteId { get; set; }
        public virtual Site Site { get; set; }

        [Display(Name = "Site name")]
        [RequiredIf("ShouldValidateFull && SiteId == SiteId.OTHER", ErrorMessage = ValidationMessages.FieldRequired)]
        [RegularExpression(ValidationRegexes.CharacterValidation, ErrorMessage = ValidationMessages.StandardStringFormat)]
        public string SiteDescription { get; set; }

        string IHasRootEntity.RootEntityType => RootEntities.Notification;
        string IHasRootEntity.RootId => NotificationId.ToString();
    }
}
