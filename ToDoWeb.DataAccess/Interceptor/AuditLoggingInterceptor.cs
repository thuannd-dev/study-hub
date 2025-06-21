using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TodoWeb.Domains.Entities;

namespace TodoWeb.Infrastructures.Interceptor
{
    public class AuditLoggingInterceptor : SaveChangesInterceptor
    {
        private List<EntityEntry> addedEntities = new List<EntityEntry>();
        //đang chuẩn bị lưu
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context as ApplicationDbContext;
            var auditLogs = new List<AuditLog>();
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog)
                {
                    continue;
                }

                var log = new AuditLog
                {
                    EntityName = entry.Entity.GetType().Name,
                    CreatedAt = DateTime.Now,
                    Action = entry.State.ToString()
                };
                if (entry.State == EntityState.Added)
                {
                    addedEntities.Add(entry);
                    //log.NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject());
                }
                if (entry.State == EntityState.Modified)
                {
                    log.OldValue = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                    log.NewValue = JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                    auditLogs.Add(log);
                }
                if (entry.State == EntityState.Deleted)
                {
                    log.OldValue = JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                    auditLogs.Add(log);
                }


            }
            if (auditLogs.Any())
            {
                context.AuditLog.AddRange(auditLogs);//State =  Added
            }
            return base.SavingChanges(eventData, result);
        }

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            var context = eventData.Context as ApplicationDbContext;
            
            if (addedEntities.Any())
            {

                var auditLogs = addedEntities.Select(entity => new AuditLog
                {
                    EntityName = entity.Entity.GetType().Name,
                    CreatedAt = DateTime.Now,
                    Action = EntityState.Added.ToString(),
                    NewValue = JsonSerializer.Serialize(entity.CurrentValues.ToObject()),
                });
                context.AuditLog.AddRange(auditLogs);
                addedEntities.Clear();
                context.SaveChanges();
            }
            

            return base.SavedChanges(eventData, result);
        }
    }
}
