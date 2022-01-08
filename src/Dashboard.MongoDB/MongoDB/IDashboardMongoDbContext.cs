using Dashboard.SensorsEntity;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Dashboard.MongoDB
{
    [ConnectionStringName(DashboardDbProperties.MongoConnectionStringName)]
    public interface IDashboardMongoDbContext : IAbpMongoDbContext
    {
        /* Define mongo collections here. Example:
         * IMongoCollection<Question> Questions { get; }
         */
        IMongoCollection<S1> S1 { get; }
        IMongoCollection<S2> S2 { get; }
        IMongoCollection<S3> S3 { get; }
    }
}
