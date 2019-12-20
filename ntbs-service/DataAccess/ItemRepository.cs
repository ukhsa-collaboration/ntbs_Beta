using System.Threading.Tasks;
using EFAuditer;
using Microsoft.EntityFrameworkCore;
using ntbs_service.Models.Entities;
using ntbs_service.Models.Enums;

namespace ntbs_service.DataAccess
{
    public interface IItemRepository<T> where T : class
    {
        Task AddAsync(T item);
        Task UpdateAsync(Notification notification, T item);
        Task DeleteAsync(T item);
    }

    public abstract class ItemRepository<T> : IItemRepository<T> where T : class
    {
        protected readonly NtbsContext _context;

        protected abstract int? ItemRootId(T item);
        protected abstract string ItemRootEntity { get; }

        protected ItemRepository(NtbsContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T item)
        {
            var dbSet = GetDbSet();
            dbSet.Add(item);
            await UpdateDatabaseAsync(ItemRootId(item), ItemRootEntity);
        }

        public async Task UpdateAsync(Notification notification, T item)
        {
            var entity = GetEntityToUpdate(notification, item);
            _context.SetValues(entity, item);
            await UpdateDatabaseAsync(ItemRootId(item), ItemRootEntity);
        }

        public async Task DeleteAsync(T item)
        {
            _context.Remove(item);
            await UpdateDatabaseAsync(ItemRootId(item), ItemRootEntity);
        }

        private async Task UpdateDatabaseAsync(
            int? rootEntityId,
            string rootEntity)
        {
            _context.AddAuditCustomField(CustomFields.RootId, rootEntityId);
            _context.AddAuditCustomField(CustomFields.AuditDetails, NotificationAuditType.Edited);
            _context.AddAuditCustomField(CustomFields.RootEntity, rootEntity);

            await _context.SaveChangesAsync();
        }

        protected abstract DbSet<T> GetDbSet();

        protected abstract T GetEntityToUpdate(Notification notification, T item);
    }
}
