using System.Linq;
using System.Threading.Tasks;
using Dashboard.Permissions;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.MultiTenancy;
using Volo.Abp.PermissionManagement;
using Volo.Abp.Uow;

namespace Dashboard.IdentityServer
{
    public class CustomPermissionContributor: ITransientDependency,IDataSeedContributor
    {
        private readonly ICurrentTenant _currentTenant;
        private readonly IGuidGenerator _guidGenerator;
        private readonly IPermissionGrantRepository _permissionGrantRepository;
        private readonly IPermissionDefinitionManager _permissionDefinitionManager;

        private const string ProviderName = RolePermissionValueProvider.ProviderName;
        private const string ProviderKey = "管理员";
        private const string GeneralUser = "普通用户";

        public CustomPermissionContributor(
            ICurrentTenant currentTenant,
            IGuidGenerator guidGenerator, 
            IPermissionGrantRepository permissionGrantRepository, 
            IPermissionDefinitionManager permissionDefinitionManager)
        {
            _currentTenant = currentTenant;
            _guidGenerator = guidGenerator;
            _permissionGrantRepository = permissionGrantRepository;
            _permissionDefinitionManager = permissionDefinitionManager;
        }
        
        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            var multiTenancySide = _currentTenant.GetMultiTenancySide();
            var permissionNames = _permissionDefinitionManager
                .GetPermissions()
                .Where(p => p.MultiTenancySide.HasFlag(multiTenancySide))
                .Where(p => !p.Providers.Any() || p.Providers.Contains(ProviderName))
                .Select(p => p.Name)
                .ToArray();
            var basePermissionNames = permissionNames
                .Where(p => p.Contains(DashboardPermissions.GroupName));

            using (_currentTenant.Change(context?.TenantId))
            {
                foreach (var permissionName in permissionNames)
                {
                    if (await _permissionGrantRepository.FindAsync(permissionName, ProviderName, ProviderKey) != null)
                    {
                        continue;
                    }

                    await _permissionGrantRepository.InsertAsync(
                        new PermissionGrant(
                            _guidGenerator.Create(),
                            permissionName,
                            ProviderName,
                            ProviderKey,
                            context?.TenantId
                        )
                    );
                }
            }
            
            foreach (var permissionName in basePermissionNames)
            {
                if (await _permissionGrantRepository.FindAsync(permissionName, ProviderName, GeneralUser) != null)
                {
                    continue;
                }

                await _permissionGrantRepository.InsertAsync(
                    new PermissionGrant(
                        _guidGenerator.Create(),
                        permissionName,
                        ProviderName,
                        GeneralUser,
                        context?.TenantId
                    )
                );
            }
        }
    }
}