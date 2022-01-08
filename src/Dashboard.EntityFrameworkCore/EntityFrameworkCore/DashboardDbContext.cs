using Dashboard.BLOBEntity;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Dashboard.EntityFrameworkCore
{
    [ConnectionStringName(DashboardDbProperties.MySQLConnectionStringName)]
    public class DashboardDbContext : AbpDbContext<DashboardDbContext>, IDashboardDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * public DbSet<Question> Questions { get; set; }
         */
        public DbSet<Blob> Blobs { get; set; }
        
        public DbSet<File> Files { get; set; }

        public DashboardDbContext(DbContextOptions<DashboardDbContext> options) 
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.ConfigureDashboard();
        }
    }
}