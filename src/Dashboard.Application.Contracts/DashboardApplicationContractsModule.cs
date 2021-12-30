using Dashboard.BLOBConstant;
using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Application;
using Volo.Abp.Modularity;
using Volo.Abp.Authorization;
using Volo.Abp.BlobStoring;
using Volo.Abp.BlobStoring.FileSystem;

namespace Dashboard
{
    [DependsOn(
        typeof(DashboardDomainSharedModule),
        typeof(AbpDddApplicationContractsModule),
        typeof(AbpAuthorizationModule),
        typeof(AbpBlobStoringModule),
        typeof(AbpBlobStoringFileSystemModule)
    )]
    public class DashboardApplicationContractsModule : AbpModule
    {
        public override void ConfigureServices(ServiceConfigurationContext context)
        {
            var configuration = context.Services.GetConfiguration();
            Configure<AbpBlobStoringOptions>(options =>
            {
                options.Containers.Configure(nameof(BlobContainerNames.BackGroundContainerName),
                    container =>
                {
                    container.IsMultiTenant = false;
                    container.UseFileSystem(fileSystem =>
                    {
                        fileSystem.BasePath = configuration["Blobs:background"];
                        fileSystem.AppendContainerNameToBasePath = false;
                    });
                });
            });
        }
    }
}
