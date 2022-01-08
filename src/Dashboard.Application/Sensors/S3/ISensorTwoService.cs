using System.Threading.Tasks;
using Dashboard.Parameters;
using Dashboard.Sensors.S3.Dto;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace Dashboard.Sensors.S3
{
    public interface ISensorThreeService: ICrudAppService<
        SensorThreeDto,
        string,
        PagedAndSortedResultRequestDto,
        CreateSensorThreeDto>
    {
        Task ManualAsync(FormGroup requestBody);
    }
}