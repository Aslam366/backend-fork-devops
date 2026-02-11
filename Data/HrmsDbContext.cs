using Microsoft.EntityFrameworkCore;
using ZetaSaasHRMSBackend.Middleware;
using ZetaSaasHRMSBackend.Models;

namespace ZetaSaasHRMSBackend.Data
{
    public class HrmsDbContext : DbContext
    {
        private readonly TenantContext _tenantContext;
        public HrmsDbContext(DbContextOptions<HrmsDbContext> options, TenantContext tenantContext)
        : base(options) 
        {
            _tenantContext = tenantContext;
        }

        public DbSet<User> User => Set<User>();
        public DbSet<Role> Role => Set<Role>();
        public DbSet<UserRole> UserRole => Set<UserRole>();
        public DbSet<Menu> Menu => Set<Menu>();
        public DbSet<RoleMenuRight> RoleMenuRight => Set<RoleMenuRight>();
        public DbSet<Employee> Employee => Set<Employee>();
        public DbSet<RefreshToken> RefreshToken => Set<RefreshToken>();

        // 🔥 AUTO-SET TENANTID LIVES HERE
        public override int SaveChanges()
        {
            SetTenantId();
            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            SetTenantId();
            return base.SaveChangesAsync(cancellationToken);
        }

        private void SetTenantId()
        {
            foreach (var entry in ChangeTracker.Entries()
                .Where(e => e.Entity is BaseEntity && e.State == EntityState.Added))
            {
                ((BaseEntity)entry.Entity).TenantId = _tenantContext.TenantId;
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                .HasQueryFilter(e => e.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<Role>()
                .HasQueryFilter(d => d.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<UserRole>()
                .HasQueryFilter(l => l.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<RoleMenuRight>()
                .HasQueryFilter(l => l.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<Employee>()
                .HasQueryFilter(l => l.TenantId == _tenantContext.TenantId);

            modelBuilder.Entity<RefreshToken>()
                .HasQueryFilter(l => l.TenantId == _tenantContext.TenantId);

            // Add more explicitly as needed

            base.OnModelCreating(modelBuilder);
        }
    }
}
