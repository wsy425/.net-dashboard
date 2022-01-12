using Volo.Abp.Application.Dtos;

namespace Dashboard.Sensors.S3.Dto
{
    public class SensorThreeDto : EntityDto<string>
    {
        public double TPL301 { get; set; }
        public double TPL302 { get; set; }
        public double TPL303 { get; set; }
        public double TPL304 { get; set; }
    }
}