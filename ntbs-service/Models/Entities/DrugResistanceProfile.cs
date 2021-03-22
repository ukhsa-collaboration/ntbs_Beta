using System.ComponentModel.DataAnnotations;
using EFAuditer;

namespace ntbs_service.Models.Entities
{
    [Display(Name = "Drug resistance profile")]
    public class DrugResistanceProfile : ModelBase, IOwnedEntityForAuditing
    {
        public int NotificationId { get; set; }

        public string Species { get; set; }
        public string DrugResistanceProfileString { get; set; }

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
