using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Dashboard.Data
{
    /* This is used if database provider does't define
     * IDashboardDbSchemaMigrator implementation.
     */
    public class NullDashboardDbSchemaMigrator : IDashboardDbSchemaMigrator, ITransientDependency
    {
        public Task MigrateAsync()
        {
            return Task.CompletedTask;
        }
    }
}