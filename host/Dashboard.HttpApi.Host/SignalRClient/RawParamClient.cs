using System.Threading.Tasks;
using Dashboard.HubClient;
using Dashboard.Sensors.S1;
using Dashboard.Sensors.S1.Dto;
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

       public RawParamClient(
           IConfiguration configuration, 
           ILogger<RawParamClient> logger, 
           ISensorOneService oneService)
       {
           _configuration = configuration;
           _logger = logger;
           _oneService = oneService;
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
           Connection.On<string>("RawDataCome", SaveDataS1);
           Connection.On<string>("AutoPCA", s=>s.Remove(0));
       }
       
       private async Task SaveDataS1(string jsonData)
       {
           var createS1 = JsonConvert.DeserializeObject<CreateSensorOneDto>(jsonData);
           await _oneService.CreateAsync(createS1);
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
    }
}