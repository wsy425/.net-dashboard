using System;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Dashboard.HubClient;
using Dashboard.Manual;
using Dashboard.ManualShared;
using Dashboard.Parameters;
using Dashboard.ServiceExtensions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Volo.Abp;
using Volo.Abp.Application.Services;

namespace Dashboard.ManualService
{
    [RemoteService(false)]
    public class ManualService : ApplicationService, IManualService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private readonly ILogger<ManualService> _logger;

        public ManualService(
            IServiceProvider serviceProvider, 
            IConfiguration configuration, 
            ILogger<ManualService> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _logger = logger;
        }

        public async Task<ManualResponse> GetManualResultAsync(ManualRequest request)
        {
            var result = new ManualResponse();
            if (RequestCheck.ManualRequestCheckError(request))
            {
                return new ManualResponse
                {
                    Code = 500,
                    Info = "请求参数有误",
                    Data = null
                };
            }
            if (request.State == Status.Success)
            {
                var method = Enum.GetName(typeof(Algorithm), request.Name)?.ToLowerInvariant();
                var filePath = _configuration["ManualResult:path"] + method + ".txt";
                // 根据文件名去获取数据  需要读写文件
                var data = ReadResultFile(filePath);
                result.Code = 200;
                result.Info = request.Name + "算法调用成功";
                result.Data = data;
                File.Delete(filePath);
            }
            else
            {
                result.Code = 400;
                result.Info = request.Name + "算法出现异常";
                result.Data = null;
            }
            var serializeDataList = JsonConvert.SerializeObject(result, Formatting.None);
            using var scope = _serviceProvider.CreateScope();
            var service = scope.ServiceProvider.GetRequiredService<IRawParamClient>();
            await service.FrontClientAsync(serializeDataList);
            return result;
        }

        private string ReadResultFile(string path)
        {
            string line;
            try
            {
                // 创建一个 StreamReader 的实例来读取文件 
                // using 语句也能关闭 StreamReader
                using var sr = new StreamReader(path, Encoding.UTF8);
                line = sr.ReadLine();
            }
            catch (Exception e)
            {
                // 向用户显示出错消息
                _logger.LogError(e.Message);
                line = "文件不存在";
            }
            return line;
        }
    }
}