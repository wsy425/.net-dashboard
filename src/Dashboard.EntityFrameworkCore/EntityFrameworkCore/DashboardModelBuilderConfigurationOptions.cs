﻿using JetBrains.Annotations;
using Volo.Abp.EntityFrameworkCore.Modeling;

namespace Dashboard.EntityFrameworkCore
{
    public class DashboardModelBuilderConfigurationOptions : AbpModelBuilderConfigurationOptions
    {
        public DashboardModelBuilderConfigurationOptions(
            [NotNull] string tablePrefix = "",
            [CanBeNull] string schema = null)
            : base(
                tablePrefix,
                schema)
        {

        }
    }
}