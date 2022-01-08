using Dashboard.SensorsEntity;
using MongoDB.Driver;
using Volo.Abp.Data;
using Volo.Abp.MongoDB;

namespace Dashboard.MongoDB
{
    [ConnectionStringName(DashboardDbProperties.MongoConnectionStringName)]
    public class DashboardMongoDbContext : AbpMongoDbContext, IDashboardMongoDbContext
    {
        /* Add mongo collections here. Example:
         * public IMongoCollection<Question> Questions => Collection<Question>();
         */
        
        public IMongoCollection<S1> S1 => Collection<S1>();
        public IMongoCollection<S2> S2 => Collection<S2>();
        public IMongoCollection<S3> S3 => Collection<S3>();

        protected override void CreateModel(IMongoModelBuilder modelBuilder)
        {
            base.CreateModel(modelBuilder);

            modelBuilder.ConfigureDashboard();
        }
    }
}