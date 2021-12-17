using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.BlobStoring;

namespace Dashboard
{
    [DependsOn(
        typeof(DashboardDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpBlobStoringModule)
        )]
    public class DashboardApplicationContractsModule : AbpModule
    {

    }
}
