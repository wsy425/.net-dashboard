using System;
using Volo.Abp.Domain.Entities;

namespace Dashboard.BLOBEntity
{
    public class Blob : Entity<Guid>
    {
        public string Name { get; set; }
        public string WebUrl { get; set; }
    }
}