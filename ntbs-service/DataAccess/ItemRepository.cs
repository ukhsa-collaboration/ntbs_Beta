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
        Task UpdateAsync(Notification Notification, T item);
        Task DeleteAsync(T item);
    }

    public abstract class ItemRepository<T> : IItemRepository<T> where T : class
    {
        protected readonly NtbsContext _context;

        public ItemRepository(NtbsContext context)
        {
            _context = context;
        }

        public async Task AddAsync(T item)
        {
            var dbSet = GetDbSet();
            dbSet.Add(item);
            await UpdateDatabaseAsync(AuditType.Added);
        }

        public async Task UpdateAsync(Notification Notification, T item)
        {
            var entity = GetEntityToUpdate(Notification, item);
            _context.SetValues(entity, item);
            await UpdateDatabaseAsync(AuditType.Edit);
        }

        public async Task DeleteAsync(T item)
        {
            _context.Remove(item);
            await UpdateDatabaseAsync(AuditType.Deleted);
        }

        private async Task UpdateDatabaseAsync(AuditType auditType)
        {
            _context.AddAuditCustomField(CustomFields.AuditDetails, auditType);
            await _context.SaveChangesAsync();
        }

        protected abstract DbSet<T> GetDbSet();

        protected abstract T GetEntityToUpdate(Notification notification, T item);
    }
}
