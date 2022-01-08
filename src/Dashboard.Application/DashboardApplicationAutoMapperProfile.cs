using AutoMapper;
using Dashboard.BLOB.Dto;
using Dashboard.BLOBEntity;
using Dashboard.Sensors.S1.Dto;
using Dashboard.Sensors.S2.Dto;
using Dashboard.Sensors.S3.Dto;
using Dashboard.SensorsEntity;
using Dashboard.ServiceExtensions;

namespace Dashboard
{
    public class DashboardApplicationAutoMapperProfile : Profile
    {
        public DashboardApplicationAutoMapperProfile()
        {
            /* You can configure your AutoMapper mapping configuration here.
             * Alternatively, you can split your mapping configurations
             * into multiple profile classes for a better organization. */
            CreateMap<Blob, GetBlobsDto>()
                .ForMember(des => des.Name,
                    options =>
                        options.MapFrom(src => src.Name.RemoveSeparateCode()));
            
            SensorOne();
            
            SensorTwo();

            SensorThree();
        }

        private void SensorOne()
        {
            CreateMap<S1, SensorOneDto>();
            CreateMap<CreateSensorOneDto, S1>()
                .ForMember(des => des.Id,
                    opt => 
                        opt.MapFrom(src => 
                            src.Time.Replace("-","").Replace(":","").Replace(" ","").Substring(0,17)));
        }

        private void SensorTwo()
        {
            CreateMap<S2, SensorTwoDto>();
            CreateMap<CreateSensorTwoDto, S2>()
                .ForMember(des => des.Id,
                    opt => 
                        opt.MapFrom(src => 
                            src.Time.Replace("-","").Replace(":","").Replace(" ","").Substring(0,17)));
        }

        private void SensorThree()
        {
            CreateMap<S3, SensorThreeDto>();
            CreateMap<CreateSensorThreeDto, S3>()
                .ForMember(des => des.Id,
                    opt => 
                        opt.MapFrom(src => 
                            src.Time.Replace("-","").Replace(":","").Replace(" ","").Substring(0,17)));
        }
    }
}