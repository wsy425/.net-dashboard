using System.Threading.Tasks;
using Dashboard.HubClient;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using ILogger = Serilog.ILogger;

namespace Dashboard.SignalRClient
{
    public abstract class RawParamClient : IRawParamClient
    {
       private HubConnection Connection { get; set; }
       private readonly IConfiguration _configuration;
       private readonly ILogger _logger;

       protected RawParamClient(
           IConfiguration configuration, 
           ILogger logger)
       {
           _configuration = configuration;
           _logger = logger;
       }

       internal async Task Initial()
       {
           var url = _configuration["SignalR:Url"];
           _logger.Information("initialize");
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
           Connection.On<string>("RawDataCome", SaveDatas);
           Connection.On<string>("AutoPCA", s => s.Remove(0));
       }
       
       private async Task SaveDatas(string jsonData)
       {
           _logger.Information(jsonData);
       }
       public async Task SpectrumClientAsync(string SerializeData)
       {
           await Connection.SendAsync("SpectrumDataDeliver", SerializeData);
       }

       public async Task PCAClientAsync(string SerializeData)
       {
           await Connection.SendAsync("PCADataDeliver", SerializeData);
       }

       public async Task ProphetClientAsync(string SerializeData)
       {
           await Connection.SendAsync("ProphetPredictDeliver", SerializeData);
       }

       public async Task GRUClientAsync(string SerializeData)
       {
           await Connection.SendAsync("GRUPredictDeliver", SerializeData);
       }
        
       public async Task ARIMAClientAsync(string SerializeData)
       {
           await Connection.SendAsync("ARIMAPredictDeliver", SerializeData);
       }
    }
}