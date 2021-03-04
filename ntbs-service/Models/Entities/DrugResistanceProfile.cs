using System.ComponentModel.DataAnnotations;
using EFAuditer;
using Microsoft.EntityFrameworkCore;

namespace ntbs_service.Models.Entities
{
    [Owned]
    [Display(Name = "Drug resistance profile")]
    public class DrugResistanceProfile : ModelBase, IOwnedEntityForAuditing
    {
        public string Species { get; set; }
        public string DrugResistanceProfileString { get; set; }

        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
