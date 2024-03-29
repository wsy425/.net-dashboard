﻿using Dashboard.BLOBEntity;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.EntityFrameworkCore;

namespace Dashboard.EntityFrameworkCore
{
    [ConnectionStringName(DashboardDbProperties.MySQLConnectionStringName)]
    public interface IDashboardDbContext : IEfCoreDbContext
    {
        /* Add DbSet for each Aggregate Root here. Example:
         * DbSet<Question> Questions { get; }
         */
        DbSet<Blob> Blobs { get; }
        DbSet<File> Files { get; }
    }
}