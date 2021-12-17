using System.IO;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace Dashboard.EntityFrameworkCore
{
    public class DashboardHttpApiHostMigrationsDbContextFactory : IDesignTimeDbContextFactory<DashboardHttpApiHostMigrationsDbContext>
    {
        public DashboardHttpApiHostMigrationsDbContext CreateDbContext(string[] args)
        {
            var configuration = BuildConfiguration();

            var connectionString = configuration.GetConnectionString("Default");

            var builder = new DbContextOptionsBuilder<DashboardHttpApiHostMigrationsDbContext>()
                .UseMySql(connectionString,ServerVersion.AutoDetect(connectionString));

            return new DashboardHttpApiHostMigrationsDbContext(builder.Options);
        }

        private static IConfigurationRoot BuildConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false);

            return builder.Build();
        }
    }
}
