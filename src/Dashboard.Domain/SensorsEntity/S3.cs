using Volo.Abp.Domain.Entities;

namespace Dashboard.SensorsEntity
{
    public class S3 : Entity<string>
    {
        public double TPL301 { get; set; }
        public double TPL302 { get; set; }
        public double TPL303 { get; set; }

        protected S3()
        {
            
        }
    }
}