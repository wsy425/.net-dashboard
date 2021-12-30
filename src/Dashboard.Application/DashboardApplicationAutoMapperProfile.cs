using AutoMapper;
using Dashboard.BLOB.Dto;
using Dashboard.BLOBEntity;
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
        }
    }
}