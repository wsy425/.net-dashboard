using Dashboard.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Dashboard.Permissions
{
    public class DashboardPermissionDefinitionProvider : PermissionDefinitionProvider
    {
        public override void Define(IPermissionDefinitionContext context)
        {
            var myGroup = context.AddGroup(
                DashboardPermissions.GroupName, 
                L("Permission:Dashboard")
                );
            var permissionBase = myGroup.AddPermission(
                DashboardPermissions.BasePermission.BasePrefix,
                L("Permission:Dashboard.Base")
            );
            
            permissionBase.AddChild(
                DashboardPermissions.BasePermission.Use,
                L("Permission:Dashboard.Base.Use")
            );
        }

        private static LocalizableString L(string name)
        {
            return LocalizableString.Create<DashboardResource>(name);
        }
    }
}