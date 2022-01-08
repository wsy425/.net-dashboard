using System.Threading.Tasks;
using Dashboard.Parameters;
using Dashboard.Sensors.S2.Dto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dashboard.Sensors.S2
{
    public interface ISensorTwoService : ICrudAppService<
        SensorTwoDto,
        string,
        PagedAndSortedResultRequestDto,
        CreateSensorTwoDto>
    {
        Task ManualAsync(FormGroup requestBody);
    }
}