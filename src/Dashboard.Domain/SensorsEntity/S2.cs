using Volo.Abp.Domain.Entities;

namespace Dashboard.SensorsEntity
{
    public class S2 : Entity<string>
    {
        public double TPL201 { get; set; }
        public double TPL202 { get; set; }
        public double TPL203 { get; set; }

        protected S2()
        {
            
        }
    }
}