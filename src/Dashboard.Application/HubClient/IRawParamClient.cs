using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace Dashboard.HubClient
{
    public interface IRawParamClient : ISingletonDependency
    {
        Task SpectrumClientAsync(string SerializeData);
        Task ProphetClientAsync(string SerializeData);
        Task GRUClientAsync(string SerializeData);
        Task ARIMAClientAsync(string SerializeData);
    }
}