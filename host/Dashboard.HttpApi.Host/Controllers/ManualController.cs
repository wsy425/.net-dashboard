using System.Threading.Tasks;
using Dashboard.Manual;
using Dashboard.Parameters;
using Microsoft.AspNetCore.Mvc;

namespace Dashboard.Controllers
{
    [Route("api/manual/result")]
    public class ManualController : DashboardController,IManualService
    {
        private readonly IManualService _service;

        public ManualController(IManualService service)
        {
            _service = service;
        }

        [HttpPost]
        public async Task<ManualResponse> GetManualResultAsync(ManualRequest request)
        {
            return await _service.GetManualResultAsync(request);
        }
    }
}