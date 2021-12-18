using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.BLOB.Dto;
using Volo.Abp.Application.Services;

namespace Dashboard.BLOB
{
    public interface IBlobService : IApplicationService
    {
        Task<BlobUploadResultDto> CreateAsync(BlobUploadInputDto input);

        Task<BlobFileDto> GetAsync(string name);

        Task<BlogDeleteDto> DeleteAsync(string name);

        List<string> GetListAsync(string name);
    }
}