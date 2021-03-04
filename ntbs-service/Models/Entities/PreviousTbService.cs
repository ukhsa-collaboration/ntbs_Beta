using System;
using EFAuditer;
using Microsoft.EntityFrameworkCore;

namespace ntbs_service.Models.Entities
{
    [Owned]
    public class PreviousTbService : ModelBase, IHasRootEntityForAuditing
    {
        public int PreviousTbServiceId { get; set; }
        public string TbServiceCode { get; set; }
        public string PhecCode { get; set; }
        public DateTime TransferDate { get; set; }

        public int NotificationId { get; set; }
        public virtual Notification Notification { get; set; }
        string IHasRootEntityForAuditing.RootEntityType => RootEntities.Notification;
        string IHasRootEntityForAuditing.RootId => NotificationId.ToString();
    }
}
