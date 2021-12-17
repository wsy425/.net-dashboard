using Volo.Abp.Modularity;

namespace Dashboard
{
    [DependsOn(
        typeof(DashboardApplicationModule),
        typeof(DashboardDomainTestModule)
        )]
    public class DashboardApplicationTestModule : AbpModule
    {

    }
}
