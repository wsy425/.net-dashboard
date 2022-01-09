using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Dashboard.HubClient;
using Dashboard.Parameters;
using Dashboard.Sensors.S1;
using Dashboard.Sensors.S2;
using Dashboard.Sensors.S2.Dto;
using Dashboard.SensorsEntity;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.Validation;

namespace Dashboard.SensorServices
{
    public class SensorTwoService : CrudAppService<
            S2,
            SensorTwoDto,
            string,
            PagedAndSortedResultRequestDto,
            CreateSensorTwoDto
        >,
        ISensorTwoService
    {
        private readonly IServiceProvider _serviceProvider;
        
        public SensorTwoService(
            IRepository<S2, string> repository, 
            IServiceProvider serviceProvider) : base(repository)
        {
            _serviceProvider = serviceProvider;
        }

        public async Task ManualAsync(FormGroup requestBody)
        {
            requestBody.Algorithm = requestBody.Algorithm.ToLowerInvariant().Trim();
            requestBody.Feature = requestBody.Feature.ToLowerInvariant();
            switch (requestBody.Algorithm)
            {
                case "spectrum":
                    await SpectrumAsync(requestBody);
                    break;
                case "prophet":
                    await ProphetAsync(requestBody);
                    break;
                case "gru":
                    await GruAsync(requestBody);
                    break;
                case "arima":
                    await ArimaAsync(requestBody);
                    break;
                default:
                    throw new AbpValidationException("此算法不存在！请重新选择算法");
            }
        }
        
        // 频谱接口
        private async Task SpectrumAsync(FormGroup requestBody)
        {
            var dataList = ReadOnlyRepository
                .Where(param => param.Id.CompareTo(requestBody.StartTime) >= 0 
                                && param.Id.CompareTo(requestBody.EndTime) <= 0)
                .ToList();
            var count = dataList.Count;
            var filterList = new ArrayList();
            for (var i = 0; i < count; i += requestBody.Interval)
            {
                filterList.Add(dataList[i].Id);
                filterList.Add(dataList[i].GetType().GetProperty(requestBody.Parameter)?.GetValue(dataList[i]));
            }

            var specList = new ArrayList();
            specList.Add(filterList);
            specList.Add(requestBody.Feature);
            specList.Add(requestBody.Windows);
            var serializeDataList = JsonConvert.SerializeObject(specList, Formatting.None);
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRawParamClient>();
                await service.SpectrumClientAsync(serializeDataList);
            }
        }
        // Prophet算法接口
        private async Task ProphetAsync(FormGroup requestBody)
        {
            var dataList = ReadOnlyRepository
                .Where(param => 
                    param.Id.CompareTo(requestBody.StartTime) >= 0 
                    && param.Id.CompareTo(requestBody.EndTime) <= 0)
                .ToList();
            var list = new ArrayList();
            var time_list = new List<string>();
            for (var i = 0; i < dataList.Count; i += requestBody.Interval)
            {
                if (requestBody.Parameter == null)
                {
                    throw new AbpValidationException("参数不能为空！");
                }
                time_list.Add(dataList[i].Id);
                list.Add(dataList[i].GetType().GetProperty(requestBody.Parameter)?.GetValue(dataList[i]));
            }
            var prophetList = new ArrayList();
            prophetList.Add(list);
            prophetList.Add(time_list);
            prophetList.Add(requestBody.Feature);
            prophetList.Add(requestBody.Windows);
            prophetList.Add(requestBody.PredictStep);
            var serializeDataList = JsonConvert.SerializeObject(prophetList, Formatting.None);
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRawParamClient>();
                await service.ProphetClientAsync(serializeDataList);
            }
        }
        // GRUPredict算法接口
        private async Task GruAsync(FormGroup requestBody)
        {
            var dataList = ReadOnlyRepository
                .Where(param => 
                    param.Id.CompareTo(requestBody.StartTime) >= 0 && param.Id.CompareTo(requestBody.EndTime) <= 0)
                .ToList();
            var list = new ArrayList();
            var time_list = new List<string>();
            var requestAttributes = requestBody.GetType().GetProperties();
            for (var i = 0; i < dataList.Count; i += requestBody.Interval)
            {
                time_list.Add(dataList[i].Id);
                list.Add(dataList[i].GetType().GetProperty(requestBody.Parameter)?.GetValue(dataList[i]));
            }
            var gruParams = new ArrayList();
            gruParams.Add(list);
            gruParams.Add(time_list);
            for (var i = 5; i < requestAttributes.Length; i++)
            {
                if(requestAttributes[i].Name == nameof(requestBody.PredictStep))
                    continue;
                gruParams.Add(requestAttributes[i].GetValue(requestBody));
            }
            var serializeDataList = JsonConvert.SerializeObject(gruParams, Formatting.None);
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRawParamClient>();
                await service.GRUClientAsync(serializeDataList);
            }
        }
        // ARIMA算法实现接口
        private async Task ArimaAsync(FormGroup requestBody)
        {
            var dataList = Repository
                .Where(param =>
                    param.Id.CompareTo(requestBody.StartTime) >= 0 && param.Id.CompareTo(requestBody.EndTime) <= 0)
                .ToList();
            var list = new ArrayList();
            var time_list = new List<string>();
            for (var i = 0; i < dataList.Count; i += requestBody.Interval)
            {
                time_list.Add(dataList[i].Id);
                list.Add(dataList[i].GetType().GetProperty(requestBody.Parameter)?.GetValue(dataList[i]));
            }
            var arimaParams = new ArrayList();
            arimaParams.Add(list);
            arimaParams.Add(time_list);
            arimaParams.Add(requestBody.Feature);
            arimaParams.Add(requestBody.Windows);
            arimaParams.Add(requestBody.P);
            arimaParams.Add(requestBody.I);
            arimaParams.Add(requestBody.Q);
            var serializeDataList = JsonConvert.SerializeObject(arimaParams, Formatting.None);
            using (var scope = _serviceProvider.CreateScope())
            {
                var service = scope.ServiceProvider.GetRequiredService<IRawParamClient>();
                await service.ARIMAClientAsync(serializeDataList);
            }
        }
    }
}