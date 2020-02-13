using EFAuditer;
using Microsoft.EntityFrameworkCore;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class DrugResistanceProfile : ModelBase, IOwnedEntityForAuditing
    {
        public string Species { get; set; }
        public string DrugResistanceProfileString { get; set; }
        
        string IOwnedEntityForAuditing.RootEntityType => RootEntities.Notification;
    }
}
