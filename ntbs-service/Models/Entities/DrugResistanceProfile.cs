using EFAuditer;
using Microsoft.EntityFrameworkCore;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class DrugResistanceProfile : ModelBase, IOwnedEntity
    {
        public string Species { get; set; }
        public string DrugResistanceProfileString { get; set; }
        string IOwnedEntity.RootEntityType => RootEntities.Notification;
    }
}
