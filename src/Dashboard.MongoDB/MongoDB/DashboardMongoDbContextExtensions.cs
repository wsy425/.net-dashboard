using System;
using Dashboard.SensorsEntity;
using Volo.Abp;
using Volo.Abp.MongoDB;

namespace Dashboard.MongoDB
{
    public static class DashboardMongoDbContextExtensions
    {
        public static void ConfigureDashboard(
            this IMongoModelBuilder builder,
            Action<AbpMongoModelBuilderConfigurationOptions> optionsAction = null)
        {
            Check.NotNull(builder, nameof(builder));

            var options = new DashboardMongoModelBuilderConfigurationOptions(
                DashboardDbProperties.DbTablePrefix
            );

            builder.Entity<S1>(b =>
            {
                b.CollectionName = nameof(S1);
            });
            
            builder.Entity<S2>(b =>
            {
                b.CollectionName = nameof(S2);
            });
            
            builder.Entity<S3>(b =>
            {
                b.CollectionName = nameof(S3);
            });
            
            optionsAction?.Invoke(options);
        }
    }
}