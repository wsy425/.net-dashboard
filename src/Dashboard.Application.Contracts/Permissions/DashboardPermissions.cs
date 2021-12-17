using Volo.Abp.Reflection;

namespace Dashboard.Permissions
{
    public class DashboardPermissions
    {
        public const string GroupName = "Dashboard";

        public static string[] GetAll()
        {
            return ReflectionHelper.GetPublicConstantsRecursively(typeof(DashboardPermissions));
        }
    }
}