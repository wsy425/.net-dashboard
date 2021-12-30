using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.BLOB.Dto;
using Volo.Abp.Application.Services;

namespace Dashboard.BLOB
{
    public interface IFileService : IBlobBaseService
    {
        Task<List<string>> GetListAsync(string name);
    }
}