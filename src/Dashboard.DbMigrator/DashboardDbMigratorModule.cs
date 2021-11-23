using Dashboard.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.BackgroundJobs;
using Volo.Abp.Modularity;

namespace Dashboard.DbMigrator
{
    [DependsOn(
        typeof(AbpAutofacModule),
        typeof(DashboardEntityFrameworkCoreModule),
        typeof(DashboardApplicationContractsModule)
        )]
    public class DashboardDbMigratorModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            Configure<AbpBackgroundJobOptions>(options => options.IsJobExecutionEnabled = false);
        }
    }
}
