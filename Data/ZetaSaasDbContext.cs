using Microsoft.EntityFrameworkCore;

namespace ZetaSaasHRMSBackend.Data
{
    public class ZetaSaasDbContext : DbContext
    {
        public ZetaSaasDbContext(DbContextOptions<ZetaSaasDbContext> options)
        : base(options) { }

        public DbSet<Tenant> Tenant { get; set; }
    }
}
