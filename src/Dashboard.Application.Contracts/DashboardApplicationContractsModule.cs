using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;

namespace Dashboard
{
    [DependsOn(
        typeof(DashboardDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule)
        )]
    public class DashboardApplicationContractsModule : AbpModule
    {

    }
}
