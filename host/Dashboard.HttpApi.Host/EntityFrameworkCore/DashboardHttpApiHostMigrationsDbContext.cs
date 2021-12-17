using Microsoft.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Dashboard.EntityFrameworkCore
{
    public class DashboardHttpApiHostMigrationsDbContext : AbpDbContext<DashboardHttpApiHostMigrationsDbContext>
    {
        public DashboardHttpApiHostMigrationsDbContext(DbContextOptions<DashboardHttpApiHostMigrationsDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ConfigureDashboard();
        }
    }
}
