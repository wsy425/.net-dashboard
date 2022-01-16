using System.Threading.Tasks;
using Dashboard.Parameters;
using Dashboard.Sensors.S1;
using Dashboard.Sensors.S1.Dto;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    [Route("api/s1")]
    public class SensorOneController : AbpController,ISensorOneService
    {
        private readonly ISensorOneService _oneAppService;

        public SensorOneController(ISensorOneService oneAppService)
        {
            _oneAppService = oneAppService;
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<SensorOneDto> GetAsync(string id)
        {
            return await _oneAppService.GetAsync(id);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        [HttpGet]
        public async Task<PagedResultDto<SensorOneDto>> GetListAsync(PagedAndSortedResultRequestDto input)
        {
            return await _oneAppService.GetListAsync(input);
        }

        [HttpPost]
        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<SensorOneDto> CreateAsync(CreateSensorOneDto input)
        {
            return await _oneAppService.CreateAsync(input);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task<SensorOneDto> UpdateAsync(string id, CreateSensorOneDto input)
        {
            return await _oneAppService.UpdateAsync(id, input);
        }

        [ApiExplorerSettings(IgnoreApi = true)]
        public async Task DeleteAsync(string id)
        {
            await _oneAppService.DeleteAsync(id);
        }

        [HttpPost]
        [Route("manual")]
        public async Task ManualAsync([FromBody]FormGroup requestBody)
        {
            await _oneAppService.ManualAsync(requestBody);
        }
    }
}