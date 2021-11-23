using Dashboard.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace Dashboard
{
    [DependsOn(
        typeof(DashboardEntityFrameworkCoreTestModule)
        )]
    public class DashboardDomainTestModule : AbpModule
    {

    }
}