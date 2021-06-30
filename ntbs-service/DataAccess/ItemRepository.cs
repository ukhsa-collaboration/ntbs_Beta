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
        void AddWithoutSave(T item);
        Task UpdateAsync(Notification notification, T item);
        Task DeleteAsync(T item);
        Task SaveChangesAsync(NotificationAuditType auditDetails);
    }

    public abstract class ItemRepository<T> : IItemRepository<T> where T : class
    {
        protected readonly NtbsContext _context;

        protected ItemRepository(NtbsContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T item)
        {
            var dbSet = GetDbSet();
            dbSet.Add(item);
            await UpdateDatabaseAsync();
        }

        public void AddWithoutSave(T item)
        {
            var dbSet = GetDbSet();
            dbSet.Add(item);
        }

        public async Task UpdateAsync(Notification notification, T item)
        {
            var entity = GetEntityToUpdate(notification, item);
            _context.SetValues(entity, item);
            await UpdateDatabaseAsync();
        }

        public async Task DeleteAsync(T item)
        {
            _context.Remove(item);
            await UpdateDatabaseAsync();
        }

        public async Task SaveChangesAsync(NotificationAuditType auditDetails = NotificationAuditType.Edited)
        {
            _context.AddAuditCustomField(CustomFields.AuditDetails, auditDetails);
            await _context.SaveChangesAsync();
        }

        private async Task UpdateDatabaseAsync()
        {
            _context.AddAuditCustomField(CustomFields.AuditDetails, NotificationAuditType.Edited);

            await _context.SaveChangesAsync();
        }

        protected abstract DbSet<T> GetDbSet();

        protected abstract T GetEntityToUpdate(Notification notification, T item);
    }
}
