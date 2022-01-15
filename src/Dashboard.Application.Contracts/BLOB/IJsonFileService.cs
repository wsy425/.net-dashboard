using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Dashboard.BLOB.Dto;
using Dashboard.Parameters;
using Volo.Abp.Application.Services;

namespace Dashboard.BLOB
{
    public interface IJsonFileService : IApplicationService
    {
        Task<FileBaseDto> CreateAsync(List<string> data);

        FileBaseDto CreateAndUpdateAsync(Config config);

        BlobUploadInputDto DownLoadAsync();

        string GetConfigAsync(string name);
    }
}