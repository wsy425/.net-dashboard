using System.Threading.Tasks;
using Dashboard.BLOB.Dto;
using Volo.Abp.Application.Services;

namespace Dashboard.BLOB
{
    public interface IBlobBaseService : IApplicationService
    {
        Task<BlobUploadResultDto> CreateAsync(BlobUploadInputDto input);

        Task<BlobFileDto> GetAsync(string name);

        Task<BlogDeleteDto> DeleteAsync(string name);
    }
}