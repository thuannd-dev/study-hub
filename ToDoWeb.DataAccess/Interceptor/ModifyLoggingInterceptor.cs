using Microsoft.EntityFrameworkCore;
using System.Text.Json;
using Microsoft.EntityFrameworkCore.Diagnostics;
using TodoWeb.Domains.Entities;
using System.Security.AccessControl;
using TodoWeb.Constants.Enums;
using Castle.Components.DictionaryAdapter.Xml;

namespace TodoWeb.Infrastructures.Interceptor
{
    public class ModifyLoggingInterceptor : SaveChangesInterceptor
    {
        DateTime time = DateTime.Now;
        public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
        {
            var context = eventData.Context as ApplicationDbContext;
            foreach (var entry in context.ChangeTracker.Entries())
            {
                if (entry.State == EntityState.Added)
                {
                    if (entry.Entity is ICreate entity)
                    {
                        entity.CreateAt = time;
                        entity.CreateBy = Role.User;
                    }

                }
                if (entry.State == EntityState.Modified)
                {
                    if (entry.Entity is IUpdate entity)
                    {
                        entity.UpdateAt = time;
                        entity.UpdateBy = Role.User;
                    }
                }
                if (entry.State == EntityState.Deleted)
                {
                    if (entry.Entity is IDelete entity)
                    {
                        entity.DeleteAt = time;
                        entity.DeleteBy = Role.User;
                        entity.Status = Status.Deleted;
                        entry.State = EntityState.Modified;
                    }
                }
            }
            return base.SavingChanges(eventData, result);
        }

        public override int SavedChanges(SaveChangesCompletedEventData eventData, int result)
        {
            return base.SavedChanges(eventData, result);
        }
    }
}
