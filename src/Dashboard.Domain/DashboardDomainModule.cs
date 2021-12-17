using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace Dashboard
{
    [DependsOn(
        typeof(AbpDddDomainModule),
        typeof(DashboardDomainSharedModule)
    )]
    public class DashboardDomainModule : AbpModule
    {

    }
}
