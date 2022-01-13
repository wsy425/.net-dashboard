using System;
using System.Threading.Tasks;
using Dashboard.HubClient;
using Dashboard.Manual;
using Dashboard.Parameters;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Dashboard.ManualService
{
    [RemoteService(false)]
    public class ManualService : ApplicationService, IManualService
    {
        private readonly IServiceProvider _serviceProvider;

        public ManualService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task<ManualResponse> GetManualResultAsync(ManualRequest request)
        {
            var result = new ManualResponse();
            if (request.State == Status.Success)
            {
                // 根据文件名去获取数据  需要读写文件
                return result;
            }
            else
            {
                result.Code = 400;
                result.Info = request.Name + "算法出现异常";
                result.Data = null;
                var serializeDataList = JsonConvert.SerializeObject(result, Formatting.None);
            }
            return result;
        }
    }
}