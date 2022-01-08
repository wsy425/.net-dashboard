using System.Threading.Tasks;
using Dashboard.Parameters;
using Dashboard.Sensors.S3;
using Dashboard.Sensors.S3.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    [Route("api/s3")]
    public class SensorThreeController:AbpController,ISensorThreeService
    {
        private readonly ISensorThreeService _service;

        public SensorThreeController(ISensorThreeService service)
        {
            _service = service;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<SensorThreeDto> GetAsync(string id)
        {
            return await _service.GetAsync(id);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<PagedResultDto<SensorThreeDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await _service.GetListAsync(input);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<SensorThreeDto> CreateAsync(CreateSensorThreeDto input)
        {
            return await _service.CreateAsync(input);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<SensorThreeDto> UpdateAsync(string id, CreateSensorThreeDto input)
        {
            return await _service.UpdateAsync(id, input);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task DeleteAsync(string id)
        {
            await _service.DeleteAsync(id);
        }

        [HttpPost]
        [Route("manual")]
        public async Task ManualAsync([FromBody]FormGroup requestBody)
        {
            await _service.ManualAsync(requestBody);
        }
    }
}