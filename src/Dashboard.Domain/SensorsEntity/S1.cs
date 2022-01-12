using Volo.Abp.Domain.Entities;

namespace Dashboard.SensorsEntity
{
    public class S1 : Entity<string>
    {
        public double TPL101 { get; set; }
        public double TPL102 { get; set; }
        public double TPL103 { get; set; }
        public double TPL104 { get; set; }

        protected S1()
        {
            
        }
    }
}