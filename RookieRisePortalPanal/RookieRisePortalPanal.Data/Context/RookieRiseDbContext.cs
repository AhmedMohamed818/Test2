using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RookieRisePortalPanal.Data.Entities;
using RookieRisePortalPanal.Data.Entities.Enums;
using System.Reflection.Emit;

namespace RookieRisePortalPanal.Data.Context
{
    public class RookieRiseDbContext
        : IdentityDbContext<AppUser, IdentityRole<Guid>, Guid>
    {
        // ✅ هنستقبل القيم من الـ Service
        public Guid? CurrentUserId { get; set; }
        public string? CurrentIpAddress { get; set; }
        public string? CurrentUserAgent { get; set; }

        public RookieRiseDbContext(
            DbContextOptions<RookieRiseDbContext> options
        ) : base(options)
        {
        }

        public DbSet<Company> Companies { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        // ⚠️ غيرنا الاسم علشان conflict مع Identity
        public DbSet<UserToken> AppUserTokens { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<AppUser>()
            .HasOne(u => u.Company)
             .WithOne(c => c.User)
            .HasForeignKey<AppUser>(u => u.CompanyId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<AuditLog>()
                .HasIndex(a => a.UserId);

            builder.Entity<UserToken>()
               .HasIndex(t => t.UserId);

            builder.Entity<UserToken>()
                .HasIndex(t => t.ExpirationTime);

            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (typeof(ITrackableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    var method = typeof(RookieRiseDbContext)
                        .GetMethod(nameof(SetSoftDeleteFilter), System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static)!
                        .MakeGenericMethod(entityType.ClrType);

                    method.Invoke(null, new object[] { builder });
                }
            }
        }

        private static void SetSoftDeleteFilter<T>(ModelBuilder builder)
            where T : class, ITrackableEntity
        {
            builder.Entity<T>().HasQueryFilter(e => !e.IsDeleted);
        }

        public override int SaveChanges()
        {
            HandleTrackingAndAudit();
            return base.SaveChanges();
        }

        public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            HandleTrackingAndAudit();
            return await base.SaveChangesAsync(cancellationToken);
        }

        private void HandleTrackingAndAudit()
        {
            var userId = CurrentUserId;
            var ip = CurrentIpAddress;
            var userAgent = CurrentUserAgent;

            var auditLogs = new List<AuditLog>();

            foreach (var entry in ChangeTracker.Entries())
            {
                if (entry.Entity is AuditLog ||
                    entry.State == EntityState.Detached ||
                    entry.State == EntityState.Unchanged)
                    continue;

                var originalState = entry.State;

                // ✅ Trackable
                if (entry.Entity is ITrackableEntity trackable)
                {
                    if (originalState == EntityState.Added)
                    {
                        trackable.CreatedAt = DateTime.UtcNow;
                        trackable.CreatedBy = userId;
                    }
                    else if (originalState == EntityState.Modified)
                    {
                        trackable.UpdatedAt = DateTime.UtcNow;
                        trackable.UpdatedBy = userId;
                    }
                    else if (originalState == EntityState.Deleted)
                    {
                        entry.State = EntityState.Modified;

                        trackable.IsDeleted = true;
                        trackable.DeletedAt = DateTime.UtcNow;
                        trackable.DeletedBy = userId;
                    }
                }

                // ✅ Audit
                var audit = new AuditLog
                {
                    TableName = entry.Metadata.GetTableName() ?? entry.Entity.GetType().Name,
                    Action = originalState switch
                    {
                        EntityState.Added => AuditAction.Create,
                        EntityState.Modified => AuditAction.Update,
                        EntityState.Deleted => AuditAction.Delete,
                        _ => AuditAction.Update
                    },
                    UserId = userId,
                    CreatedAt = DateTime.UtcNow,
                    IpAddress = ip,
                    UserAgent = userAgent
                };

                if (originalState == EntityState.Modified)
                {
                    audit.OldValues = System.Text.Json.JsonSerializer.Serialize(entry.OriginalValues.ToObject());
                    audit.NewValues = System.Text.Json.JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                }
                else if (originalState == EntityState.Added)
                {
                    audit.NewValues = System.Text.Json.JsonSerializer.Serialize(entry.CurrentValues.ToObject());
                }

                auditLogs.Add(audit);
            }

            if (auditLogs.Any())
            {
                AuditLogs.AddRange(auditLogs);
            }
        }
    }
}