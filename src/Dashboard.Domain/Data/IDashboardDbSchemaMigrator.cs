using System.Threading.Tasks;

namespace Dashboard.Data
{
    public interface IDashboardDbSchemaMigrator
    {
        Task MigrateAsync();
    }
}
