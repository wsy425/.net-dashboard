using Volo.Abp.Reflection;

namespace Dashboard.Permissions
{
    public class DashboardPermissions
    {
        public const string GroupName = "Dashboard";
        
        public static class BasePermission
        {
            public const string BasePrefix = GroupName + ".Base";
            
            public const string Use = BasePrefix + ".Use";
        }

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(DashboardPermissions));
        }
    }
}