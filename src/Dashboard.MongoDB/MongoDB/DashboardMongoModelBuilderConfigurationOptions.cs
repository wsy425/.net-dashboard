using JetBrains.Annotations;
using Volo.Abp.MongoDB;

namespace Dashboard.MongoDB
{
    public class DashboardMongoModelBuilderConfigurationOptions : AbpMongoModelBuilderConfigurationOptions
    {
        public DashboardMongoModelBuilderConfigurationOptions(
            [NotNull] string collectionPrefix = "")
            : base(collectionPrefix)
        {
        }
    }
}