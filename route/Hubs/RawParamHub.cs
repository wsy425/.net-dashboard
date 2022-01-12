using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace route.Hubs
{
    public class RawParamHub : Hub
    {
        private readonly ILogger _logger;
        public RawParamHub(ILogger<RawParamHub> logger)
        {
            _logger = logger;
        }
        
        # region 三路实时数据分发通道
        // 实时数据 => 前端/后端
        [HubMethodName(nameof(DeliverRawDataS1))]
        public async Task DeliverRawDataS1(string data)
        {
            _logger.LogInformation(data);
            await Clients.Others.SendAsync("RawDataComeS1", data);
        }
        [HubMethodName(nameof(DeliverRawDataS2))]
        public async Task DeliverRawDataS2(string data)
        {
            _logger.LogError(data);
            await Clients.Others.SendAsync("RawDataComeS2", data);
        }
        [HubMethodName(nameof(DeliverRawDataS3))]
        public async Task DeliverRawDataS3(string data)
        {
            _logger.LogWarning(data);
            await Clients.Others.SendAsync("RawDataComeS3", data);
        }
        # endregion
        
        # region 三路自动PCA算法
        [HubMethodName(nameof(AutoPCADeliverS1))]
        public Task AutoPCADeliverS1(string serializeData)
        {
            _logger.LogInformation(serializeData);
            return Clients.Others.SendAsync("AutoPCAS1", serializeData);
        }
        [HubMethodName(nameof(AutoPCADeliverS2))]
        public Task AutoPCADeliverS2(string serializeData)
        {
            return Clients.Others.SendAsync("AutoPCAS2", serializeData);
        }
        [HubMethodName(nameof(AutoPCADeliverS3))]
        public Task AutoPCADeliverS3(string serializeData)
        {
            return Clients.Others.SendAsync("AutoPCAS3", serializeData);
        }
        # endregion
        
        # region 后端算法结果 => 前端
        [HubMethodName(nameof(FrontDeliver))]
        public async Task FrontDeliver(string data)
        {
            await Clients.Others.SendAsync("Front", data);
        }
        # endregion
        
        # region 后端 => 算法
        // Spectrum算法
        [HubMethodName(nameof(SpectrumDataDeliver))]
        public async Task SpectrumDataDeliver(string serializeData)
        {
            await Clients.Others.SendAsync("Spectrum", serializeData);
        }
        // Prophet算法
        [HubMethodName(nameof(ProphetPredictDeliver))]
        public async Task ProphetPredictDeliver(string serializeData)
        {
            await Clients.Others.SendAsync("ProphetPredict", serializeData);
        }
        // GRU算法
        [HubMethodName(nameof(GRUPredictDeliver))]
        public async Task GRUPredictDeliver(string serializeData)
        {
            await Clients.Others.SendAsync("GRUPredict", serializeData);
        }
        // ARIMA算法
        [HubMethodName(nameof(ARIMAPredictDeliver))]
        public async Task ARIMAPredictDeliver(string serializeData)
        {
            await Clients.Others.SendAsync("ARIMAPredict", serializeData);
        }
        # endregion
    }
}