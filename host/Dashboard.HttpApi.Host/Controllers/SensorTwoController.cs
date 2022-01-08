using System.Threading.Tasks;
using Dashboard.Parameters;
using Dashboard.Sensors.S2;
using Dashboard.Sensors.S2.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    [Route("api/s2")]
    public class SensorTwoController : AbpController,ISensorTwoService
    {
        private readonly ISensorTwoService _service;

        public SensorTwoController(ISensorTwoService service)
        {
            _service = service;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<SensorTwoDto> GetAsync(string id)
        {
            return await _service.GetAsync(id);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<PagedResultDto<SensorTwoDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await _service.GetListAsync(input);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<SensorTwoDto> CreateAsync(CreateSensorTwoDto input)
        {
            return await _service.CreateAsync(input);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<SensorTwoDto> UpdateAsync(string id, CreateSensorTwoDto input)
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