using System.Threading.Tasks;
using Dashboard.Parameters;
using Volo.Abp.Application.Services;

namespace Dashboard.Manual
{
    public interface IManualService : IApplicationService
    {
        Task<ManualResponse> GetManualResultAsync(ManualRequest request);
    }
}