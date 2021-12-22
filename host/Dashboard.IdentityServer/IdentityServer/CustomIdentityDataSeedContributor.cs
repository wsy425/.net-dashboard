using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Volo.Abp;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Guids;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.Uow;
using IdentityRole = Volo.Abp.Identity.IdentityRole;
using IdentityUser = Volo.Abp.Identity.IdentityUser;

namespace Dashboard.IdentityServer
{
    public class CustomIdentityDataSeedContributor : ITransientDependency,IDataSeedContributor
    {
        private readonly IGuidGenerator _guidGenerator;
        private readonly IIdentityRoleRepository _roleRepository;
        private readonly IdentityRoleManager _roleManager;
        private readonly ICurrentTenant _currentTenant;
        private readonly IIdentityUserRepository _userRepository;
        private readonly ILookupNormalizer _normalizer;
        private readonly IdentityUserManager _userManager;
        
        private const string AdministratorRoleName = "管理员";
        private const string NormalRoleName = "普通用户";
        private const string AdminName = "admin";

        public CustomIdentityDataSeedContributor(
            IGuidGenerator guidGenerator, 
            IIdentityRoleRepository roleRepository,
            IdentityRoleManager roleManager,
            ICurrentTenant currentTenant, 
            IIdentityUserRepository userRepository, 
            ILookupNormalizer normalizer, 
            IdentityUserManager userManager)
        {
            _guidGenerator = guidGenerator;
            _roleRepository = roleRepository;
            _roleManager = roleManager;
            _currentTenant = currentTenant;
            _userRepository = userRepository;
            _normalizer = normalizer;
            _userManager = userManager;
        }
        
        [UnitOfWork]
        public async Task SeedAsync(DataSeedContext context)
        {
            using (_currentTenant.Change(context?.TenantId))
            {
                var customRoles = new List<string>{ AdministratorRoleName,NormalRoleName };
                await InitialRolesAsync(customRoles,context);
                await ChangeAdminDefaultRoles();
            }
        }
        
        private async Task InitialRolesAsync(IEnumerable<string> customRoles,DataSeedContext context)
        {
            foreach (var customRoleName in customRoles)
            {
                var role = await _roleRepository.FindByNormalizedNameAsync(customRoleName);
                if (role == null)
                {
                    role = new IdentityRole(
                        _guidGenerator.Create(),
                        customRoleName,
                        context?.TenantId
                    )
                    {
                        IsStatic = true,
                        IsPublic = true,
                        IsDefault = customRoleName == NormalRoleName
                    };

                    (await _roleManager.CreateAsync(role)).CheckErrors();
                }
            }
        }
        
        private async Task ChangeAdminDefaultRoles()
        {
            var userResult = await _userRepository.FindByNormalizedUserNameAsync(_normalizer.NormalizeName(AdminName));
            Check.NotNull(userResult, nameof(IdentityUser));
            var roleResult = await _roleRepository.FindByNormalizedNameAsync(_normalizer.NormalizeName(AdministratorRoleName));
            Check.NotNull(roleResult, nameof(IdentityRole));
            (await _userManager.SetRolesAsync(userResult, new List<string>{AdministratorRoleName})).CheckErrors();
        }
    }
}