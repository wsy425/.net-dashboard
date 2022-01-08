using Volo.Abp.Application.Dtos;

namespace Dashboard.Sensors.S2.Dto
{
    public class SensorTwoDto : EntityDto<string>
    {
        public double TPL201 { get; set; }
        public double TPL202 { get; set; }
        public double TPL203 { get; set; }
    }
}