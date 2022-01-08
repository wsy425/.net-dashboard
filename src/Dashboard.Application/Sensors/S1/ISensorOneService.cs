using System.Threading.Tasks;
using Dashboard.Parameters;
using Dashboard.Sensors.S1.Dto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dashboard.Sensors.S1
{
    public interface ISensorOneService : ICrudAppService<
        SensorOneDto,
        string,
        PagedAndSortedResultRequestDto,
        CreateSensorOneDto
    >
    {
        Task ManualAsync(FormGroup requestBody);
    }
}