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
        
        // 实时数据 => 前端/后端
        [HubMethodName(nameof(DeliverRawData))]
        public async Task DeliverRawData(string data)
        {
            _logger.LogInformation(data);
            await Clients.Others.SendAsync("RawDataCome", data);
        }
        // 后端算法结果 => 前端
        [HubMethodName(nameof(FrontDeliver))]
        public async Task FrontDeliver(string data)
        {
            await Clients.Others.SendAsync("Front", data);
        }
        // Spectrum算法  后端 => 算法
        [HubMethodName(nameof(SpectrumDataDeliver))]
        public async Task SpectrumDataDeliver(string serializeData)
        {
            await Clients.Others.SendAsync("Spectrum", serializeData);
        }
        // PCA算法  后端 => 算法
        [HubMethodName(nameof(PCADataDeliver))]
        public async Task PCADataDeliver(string serializeData)
        {
            await Clients.Others.SendAsync("PCA",serializeData);
        }
        // Prophet算法  后端 => 算法
        [HubMethodName(nameof(ProphetPredictDeliver))]
        public async Task ProphetPredictDeliver(string serializeData)
        {
            await Clients.Others.SendAsync("ProphetPredict", serializeData);
        }
        // GRU算法  后端 => 算法
        [HubMethodName(nameof(GRUPredictDeliver))]
        public async Task GRUPredictDeliver(string serializeData)
        {
            await Clients.Others.SendAsync("GRUPredict", serializeData);
        }
        // ARIMA算法  后端 => 算法
        [HubMethodName(nameof(ARIMAPredictDeliver))]
        public async Task ARIMAPredictDeliver(string serializeData)
        {
            await Clients.Others.SendAsync("ARIMAPredict", serializeData);
        }
        // PCA自动算法  算法 => 前端
        public Task AutoPCADeliver(string serializeData)
        {
            return Clients.Others.SendAsync("AutoPCA", serializeData);
        }
    }
}