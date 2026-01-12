using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Security.Application.Interfaces.Context;
using Security.Domain.Abstractions;


namespace Security.Infrastructure.Data.Interceptors;

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry)
    {
        return entry.References.Any(r => r.TargetEntry != null && r.TargetEntry.Metadata.IsOwned()
                                                               && (r.TargetEntry.State == EntityState.Added ||
                                                                   r.TargetEntry.State == EntityState.Modified));
    }
}

public class AuditableEntityInterceptor(IUserContext userContext) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    public void UpdateEntities(DbContext? context)
    {
        if (context == null)
        {
            return;
        }

        foreach (var entry in context.ChangeTracker.Entries<IEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy =  userContext.UserId == null || userContext.UserId <= 0 ? null : userContext.UserId;
                entry.Entity.CreatedAt = DateTime.UtcNow;
            }

            if (entry.State != EntityState.Added && entry.State != EntityState.Modified) continue;
            entry.Entity.LastModifiedBy = userContext.UserId == null || userContext.UserId <= 0 ? null : userContext.UserId;
            entry.Entity.LastModified = DateTime.UtcNow;
        }
    }
}