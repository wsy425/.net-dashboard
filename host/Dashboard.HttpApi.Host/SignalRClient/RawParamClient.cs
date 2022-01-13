using System.Threading.Tasks;
using Dashboard.HubClient;
using Dashboard.Sensors.S1;
using Dashboard.Sensors.S1.Dto;
using Dashboard.Sensors.S2;
using Dashboard.Sensors.S2.Dto;
using Dashboard.Sensors.S3;
using Dashboard.Sensors.S3.Dto;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Dashboard.SignalRClient
{
    public class RawParamClient : IRawParamClient
    {
       private HubConnection Connection { get; set; }
       private readonly IConfiguration _configuration;
       private readonly ILogger<RawParamClient> _logger;
       private readonly ISensorOneService _oneService;
       private readonly ISensorTwoService _twoService;
       private readonly ISensorThreeService _threeService;

       public RawParamClient(
           IConfiguration configuration, 
           ILogger<RawParamClient> logger, 
           ISensorOneService oneService, 
           ISensorThreeService threeService, 
           ISensorTwoService twoService)
       {
           _configuration = configuration;
           _logger = logger;
           _oneService = oneService;
           _threeService = threeService;
           _twoService = twoService;
       }

       internal async Task Initial()
       {
           var url = _configuration["SignalR:Url"];
           _logger.LogInformation("initialize");
           Connection = new HubConnectionBuilder()
               .WithUrl(url)
               .WithAutomaticReconnect()
               .ConfigureLogging(logging =>
               {
                   logging.SetMinimumLevel(LogLevel.Information);
                   logging.AddConsole();
               })
               .Build();

           BindEvents();

           await Connection.StartAsync();
       }
       
       private void BindEvents()
       {
           Connection.On<string>("RawDataComeS1", SaveDataS1);
           Connection.On<string>("RawDataComeS2", SaveDataS2);
           Connection.On<string>("RawDataComeS3", SaveDataS3);
           Connection.On<string>("AutoPCAS1", s=>s.Remove(0));
           Connection.On<string>("AutoPCAS2", s=>s.Remove(0));
           Connection.On<string>("AutoPCAS3", s=>s.Remove(0));
           Connection.On<string>("Front", s=>s.Remove(0));
       }
       
       private async Task SaveDataS1(string jsonData)
       {
           var createS1 = JsonConvert.DeserializeObject<CreateSensorOneDto>(jsonData);
           await _oneService.CreateAsync(createS1);
       }
       private async Task SaveDataS2(string jsonData)
       {
           var createS1 = JsonConvert.DeserializeObject<CreateSensorTwoDto>(jsonData);
           await _twoService.CreateAsync(createS1);
       }
       private async Task SaveDataS3(string jsonData)
       {
           var createS1 = JsonConvert.DeserializeObject<CreateSensorThreeDto>(jsonData);
           await _threeService.CreateAsync(createS1);
       }
       public async Task SpectrumClientAsync(string serializeData)
       {
           await Connection.SendAsync("SpectrumDataDeliver", serializeData);
       }
       public async Task ProphetClientAsync(string serializeData)
       {
           await Connection.SendAsync("ProphetPredictDeliver", serializeData);
       }
       public async Task GRUClientAsync(string serializeData)
       {
           await Connection.SendAsync("GRUPredictDeliver", serializeData);
       }
       public async Task ARIMAClientAsync(string serializeData)
       {
           await Connection.SendAsync("ARIMAPredictDeliver", serializeData);
       }
       public async Task FrontClientAsync(string serializeData)
       {
           await Connection.SendAsync("FrontDeliver", serializeData);
       }
    }
}