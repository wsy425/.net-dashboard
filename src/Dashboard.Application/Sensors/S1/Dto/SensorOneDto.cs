using Volo.Abp.Application.Dtos;

namespace Dashboard.Sensors.S1.Dto
{
    public class SensorOneDto : EntityDto<string>
    {
        public double TPL101 { get; set; }
        public double TPL102 { get; set; }
        public double TPL103 { get; set; }
        public double TPL104 { get; set; }
    }
}