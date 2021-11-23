using Dashboard.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Dashboard.Permissions
{
    public class DashboardPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(DashboardPermissions.GroupName);
            //Define your own permissions here. Example:
            //myGroup.AddPermission(DashboardPermissions.MyPermission1, L("Permission:MyPermission1"));
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DashboardResource>(name);
        }
    }
}
